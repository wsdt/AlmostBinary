using AlmostBinary_Compiler.utils;
using AlmostBinary_Compiler.Utils;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AlmostBinary_Compiler
{
    public sealed class Lexer
    {
        #region fields
        private static ILogger Log => Serilog.Log.ForContext<Lexer>();
        private readonly Dictionary<Tokens, string> _tokens;
        private readonly Dictionary<Tokens, MatchCollection> _regExMatchCollection;
        private string _inputString;
        private int _index;
        #endregion

        #region enums
        public enum Tokens
        {
            Undefined = 0,
            Import = 1,
            Function = 2,
            If = 3,
            ElseIf = 4,
            Else = 5,
            While = 6,
            Repeat = 7,
            Return = 8,
            IntLiteral = 9,
            StringLiteral = 10,
            Ident = 11,
            Whitespace = 12,
            NewLine = 13,
            Add = 14,
            Sub = 15,
            Mul = 16,
            Div = 17,
            Equal = 18,
            DoubleEqual = 19,
            NotEqual = 20,
            LeftParan = 21,
            RightParan = 22,
            LeftBrace = 23,
            RightBrace = 24,
            Comma = 25,
            Period = 26,
            EOF = 27
        }
        #endregion

        #region properties
        public string InputString
        {
            set
            {
                _inputString = value;
                PrepareRegex();
            }
        }
        #endregion

        #region ctor
        public Lexer()
        {
            Log.Here().Information("Starting Lexer.");

            _tokens = new Dictionary<Tokens, string>();
            _regExMatchCollection = new Dictionary<Tokens, MatchCollection>();
            _index = 0;
            _inputString = string.Empty;

            _tokens.Add(Tokens.Import, "import"); //.ToBinary() something not working yet (take a look at IntLiteral)
            _tokens.Add(Tokens.Function, "function");
            _tokens.Add(Tokens.If, "if");
            _tokens.Add(Tokens.ElseIf, "elseif");
            _tokens.Add(Tokens.Else, "else");
            _tokens.Add(Tokens.Repeat, "repeat");
            _tokens.Add(Tokens.Return, "return");
            _tokens.Add(Tokens.StringLiteral, "\".*?\"");
            _tokens.Add(Tokens.IntLiteral, "[0-9][0-9]*"); // TODO: might be the problem that 01010101s not working properly as keywords.
            _tokens.Add(Tokens.Ident, "[a-zA-Z_][a-zA-Z0-9_]*");
            _tokens.Add(Tokens.Whitespace, "[ \\t]+");
            _tokens.Add(Tokens.NewLine, "\\n");
            _tokens.Add(Tokens.Add, "\\+");
            _tokens.Add(Tokens.Sub, "\\-");
            _tokens.Add(Tokens.Mul, "\\*");
            _tokens.Add(Tokens.Div, "\\/");
            _tokens.Add(Tokens.DoubleEqual, "\\==");
            _tokens.Add(Tokens.NotEqual, "\\!=");
            _tokens.Add(Tokens.Equal, "\\=");
            _tokens.Add(Tokens.LeftParan, "\\(");
            _tokens.Add(Tokens.RightParan, "\\)");
            _tokens.Add(Tokens.LeftBrace, "\\{");
            _tokens.Add(Tokens.RightBrace, "\\}");
            _tokens.Add(Tokens.Comma, "\\,");
            _tokens.Add(Tokens.Period, "\\.");
        }
        #endregion

        #region methods
        private void PrepareRegex()
        {
            _regExMatchCollection.Clear();
            foreach (KeyValuePair<Tokens, string> pair in _tokens)
            {
                _regExMatchCollection.Add(pair.Key, Regex.Matches(_inputString, pair.Value));
            }
        }

        public void ResetParser()
        {
            _index = 0;
            _inputString = string.Empty;
            _regExMatchCollection.Clear();
            Log.Here().Verbose("Resetted parser.");
        }

        public Token? GetToken()
        {
            if (_index >= _inputString.Length)
                return null;

            foreach (KeyValuePair<Tokens, MatchCollection> pair in _regExMatchCollection)
            {
                foreach (Match match in pair.Value)
                {
                    if (match.Index == _index)
                    {
                        _index += match.Length;
                        Log.Here().Verbose($"Extracting new token: {pair.Key} -> Number of matches: '{pair.Value.Count}'");
                        return new Token(pair.Key, match.Value);
                    }

                    if (match.Index > _index)
                    {
                        break;
                    }
                }
            }
            _index++;
            return new Token(Tokens.Undefined, string.Empty);
        }

        public PeekToken? Peek() => Peek(new PeekToken(_index, new Token(Tokens.Undefined, string.Empty)));

        public PeekToken? Peek(PeekToken peekToken)
        {
            Log.Here().Verbose($"Received peekToken -> {JsonSerializer.Serialize(peekToken)}");
            int oldIndex = _index;

            _index = peekToken.TokenIndex;

            if (_index >= _inputString.Length)
            {
                Log.Here().Verbose($"Index greater than inputString length. Returning null and setting oldIndex '{oldIndex}' to _index -> Index: '{_index}', Length-Inputstring: {_inputString.Length}");
                _index = oldIndex;
                return null;
            }

            foreach (KeyValuePair<Tokens, string> pair in _tokens)
            {
                Regex r = new Regex(pair.Value);
                Match m = r.Match(_inputString, _index);

                if (m.Success && m.Index == _index)
                {
                    _index += m.Length;
                    PeekToken pt = new PeekToken(_index, new Token(pair.Key, m.Value));
                    Log.Here().Verbose($"Created new peek token as inputString matched -> OldIndex: '{oldIndex}', Index: '{_index}', PeekToken: '{JsonSerializer.Serialize(pt)}'");
                    _index = oldIndex;
                    return pt;
                }
            }
            PeekToken pt2 = new PeekToken(_index + 1, new Token(Tokens.Undefined, string.Empty));
            Log.Here().Verbose($"Created new empty peek token -> OldIndex: '{oldIndex}', Index: '{_index}', PeekToken: '{JsonSerializer.Serialize(pt2)}'");
            _index = oldIndex;
            return pt2;
        }
    }
    #endregion


    #region inner_classes
    public class PeekToken
    {
        #region fields
        private static ILogger Log => Serilog.Log.ForContext<PeekToken>();
        #endregion

        #region properties
        public int TokenIndex { get; set; }
        public Token TokenPeek { get; set; }
        #endregion

        #region ctor
        public PeekToken(int index, Token value)
        {
            TokenIndex = index;
            TokenPeek = value;
            Log.Here().Verbose($"Created new peekToken -> Name: '{TokenIndex}', Value: '{TokenPeek}'");
        }
        #endregion
    }

    public class Token
    {
        #region fields
        private static ILogger Log => Serilog.Log.ForContext<Token>();
        #endregion

        #region properties
        public Lexer.Tokens TokenName { get; set; }
        public string TokenValue { get; set; }
        #endregion

        #region ctor
        public Token(Lexer.Tokens name, string value)
        {
            TokenName = name;
            TokenValue = value;
            Log.Here().Verbose($"Created new token -> Name: '{TokenName}', Value: '{TokenValue}'");
        }
        #endregion
    }

    public class TokenList
    {
        #region fields
        private static ILogger Log => Serilog.Log.ForContext<TokenList>();
        private List<Token> _tokens;
        private int _pos = 0;
        #endregion

        #region properties
        public List<Token> Tokens { get => _tokens; set => _tokens = value; }
        public int Pos
        {
            get => _pos;
            set
            {
                Log.Here().Verbose($"Token position updated: {_pos}->{value}");
                _pos = value;
            }
        }
        #endregion

        #region ctor
        public TokenList(List<Token> tokens)
        {
            Tokens = tokens;
            Log.Here().Verbose($"Created new tokenList -> {JsonSerializer.Serialize(tokens)}");
        }
        #endregion

        #region methods
        public bool HasNextToken() => (Pos + 1) >= Tokens.Count;

        public Token GetToken()
        {
            if (HasNextToken()) throw new Exception($"Tokenlist exhausted. Tokenposition faulty | Current-Token: {Tokens[Pos].TokenValue}, Current-Position: {Pos}");
            Log.Here().Warning($"Token: {Tokens[Pos].TokenValue}, {Pos}->{Pos+1}, Next token (curr pos): {Tokens[Pos+1].TokenValue}");
            Token t = Tokens[_pos++];
            Log.Here().Verbose($"Getting token '{t.TokenName}:{t.TokenValue}' on position '{Pos}' from {JsonSerializer.Serialize(Tokens)}.");
            return t;
        }

        public Token PeekToken()
        {
            Token t = Tokens[Pos];
            Log.Here().Verbose($"Peeking token '{t.TokenName}:{t.TokenValue}' on position '{Pos}' from {JsonSerializer.Serialize(Tokens)}.");
            return t;
        }
        #endregion
    }
    #endregion
}
