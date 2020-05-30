using AlmostBinary_Compiler.utils;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AlmostBinary_Compiler
{
    class Parser
    {
        #region fields
        private static ILogger Log => Serilog.Log.ForContext<Parser>();
        static TokenList? _tokens;
        static Block? _currentBlock = null;
        static Stack<Block> _blockstack = new Stack<Block>();
        static List<Stmt> _tree = new List<Stmt>();
        #endregion

        #region properties 
        public List<Stmt> Tree { get => _tree; }
        #endregion

        #region ctor
        public Parser(TokenList t)
        {
            Log.Here().Information("Starting parser.");
            _tokens = t;
            if (_tokens == null) Log.Here().Warning("Tokenlist is null.");
            Log.Here().Debug($"Starting to parse tokenList -> {JsonSerializer.Serialize(_tokens.Tokens)}");

            while (true)
            {
                Token? tok;
                try
                {
                    tok = _tokens.GetToken();
                }
                catch
                {
                    Log.Here().Verbose("Reached end of file. Shutting down parser..");
                    break;
                }

                if (tok.TokenName == Lexer.Tokens.Import)
                {
                    Startup.Imports.Add(ParseImport());
                }
                else if (tok.TokenName == Lexer.Tokens.Function)
                {
                    Func func = Func.Parse(_tokens);

                    if (_currentBlock == null)
                    {
                        _currentBlock = func;
                    }
                    else
                    {
                        _currentBlock.AddStmt(new Return(null));
                        _tree.Add(_currentBlock);
                        _currentBlock = func;
                    }
                }
                else if (tok.TokenName == Lexer.Tokens.If)
                {
                    IfBlock ifblock = IfBlock.Parse(_tokens);

                    if (_currentBlock != null)
                    {
                        _blockstack.Push(_currentBlock);
                        _currentBlock = ifblock;
                    }
                }
                else if (tok.TokenName == Lexer.Tokens.ElseIf)
                {
                    ElseIfBlock elseifblock = ElseIfBlock.Parse(_tokens);

                    if (_currentBlock != null)
                    {
                        _blockstack.Push(_currentBlock);
                        _currentBlock = elseifblock;
                    }
                }
                else if (tok.TokenName == Lexer.Tokens.Else)
                {
                    if (_currentBlock != null)
                    {
                        _blockstack.Push(_currentBlock);
                        _currentBlock = new ElseBlock();
                    }
                }
                else if (tok.TokenName == Lexer.Tokens.Repeat)
                {
                    if (_currentBlock != null)
                    {
                        _blockstack.Push(_currentBlock);
                        _currentBlock = new RepeatBlock();
                    }
                }
                else if (tok.TokenName == Lexer.Tokens.Ident)
                {
                    if (_tokens.PeekToken().TokenName == Lexer.Tokens.Equal)
                    {
                        _tokens.Pos--;
                        Assign a = Assign.Parse(_tokens);
                        _currentBlock.AddStmt(a);
                    }
                    else if (_tokens.PeekToken().TokenName == Lexer.Tokens.LeftParan)
                    {
                        _tokens.Pos--;
                        Call c = Call.Parse(_tokens);
                        _currentBlock.AddStmt(c);
                    }
                }
                else if (tok.TokenName == Lexer.Tokens.Return)
                {
                    Return r = Return.Parse(_tokens);
                    _currentBlock.AddStmt(r);
                }
                else if (tok.TokenName == Lexer.Tokens.RightBrace)
                {
                    if (_currentBlock is Func)
                    {
                        _currentBlock.AddStmt(new Return(null));
                        _tree.Add(_currentBlock);
                        _currentBlock = null;
                    }
                    else if (_currentBlock is IfBlock || _currentBlock is ElseIfBlock || _currentBlock is ElseBlock)
                    {
                        _currentBlock.AddStmt(new EndIf());
                        Block block = _currentBlock;

                        if (_blockstack.Count > 0)
                        {
                            _currentBlock = _blockstack.Pop();
                            _currentBlock.AddStmt(block);
                        }
                    }
                    else if (_currentBlock is RepeatBlock)
                    {
                        Block block = _currentBlock;

                        if (_blockstack.Count > 0)
                        {
                            _currentBlock = _blockstack.Pop();
                            _currentBlock.AddStmt(block);
                        }
                    }
                }
                else if (tok.TokenName == Lexer.Tokens.EOF)
                {
                    _tree.Add(_currentBlock);
                }
            }
        }
        #endregion

        #region methods
        static string ParseImport()
        {
            string ret = "";
            Token t = _tokens.GetToken();

            if (t.TokenName == Lexer.Tokens.Ident)
            {
                ret = t.TokenValue;
            }

            return ret;
        }
        #endregion
    }
}
