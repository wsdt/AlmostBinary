using AlmostBinary_Compiler.utils;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlmostBinary_Compiler
{
    internal sealed class Parser
    {
        #region fields
        private static ILogger Log => Serilog.Log.ForContext<Parser>();
        private static List<Stmt>? _tree;
        static TokenList? tokens;
        static Block? currentBlock;
        static Stack<Block>? blockstack;
        static bool? running;
        #endregion

        #region properties
        public List<Stmt>? Tree { get => _tree; }
        #endregion

        #region ctor
        public Parser(TokenList t)
        {
            Log.Here().Information("Starting parser.");
            tokens = t;
            if (tokens == null) Log.Here().Warning("Tokenlist is null.");

            currentBlock = null;
            blockstack = new Stack<Block>();
            _tree = new List<Stmt>();
            running = true;

            Parse();
        }
        #endregion

        #region methods
        /// <summary>
        /// Parses tokenized list
        /// </summary>
        static void Parse()
        {
            Log.Here().Information($"Starting to parse tokenList -> {JsonSerializer.Serialize(tokens.Tokens)}");

            while (running ?? false)
            {
                Token? tok;
                try
                {
                    tok = tokens.GetSafeToken(ref running);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Could not get token from token list -> {JsonSerializer.Serialize(tokens)}", ex);
                }


                switch (tok.TokenName)
                {
                    case Lexer.Tokens.Import: TokenizeImport(); break;
                    case Lexer.Tokens.Function: TokenizeFunction(); break;
                    case Lexer.Tokens.If: TokenizeIf(); break;
                    case Lexer.Tokens.ElseIf: TokenizeElseIf(); break;
                    case Lexer.Tokens.Else: TokenizeElse(); break;
                    case Lexer.Tokens.Repeat: TokenizeRepeat(); break;
                    case Lexer.Tokens.Ident: TokenizeIdent(); break;
                    case Lexer.Tokens.Return: TokenizeReturn(); break;
                    case Lexer.Tokens.RightParan: TokenizeRightParan(); break;
                    case Lexer.Tokens.RightBrace: TokenizeRightBrace(); break;
                    case Lexer.Tokens.EOF: TokenizeEOF(); break;
                    default: Log.Here().Error($"Caught unknown token: {tok.TokenName}:{tok.TokenValue}"); running = false; break;
                }

                Log.Here().Information($"Current token: {tok.TokenValue}");
            }
        }

        #region tokenizers
        private static void TokenizeImport() => Startup.Imports.Add(ParseImport());

        private static void TokenizeFunction()
        {
            if (currentBlock != null)
            {
                currentBlock.AddStmt(new Return(null));
            }

            Func func = Func.Parse(tokens);
            currentBlock = func;
            _tree.Add(currentBlock);

            //if (currentBlock == null)
            //{
            //    currentBlock = func;
            //}
            //else
            //{
            //    currentBlock.AddStmt(new Return(null));
            //    _tree.Add(currentBlock);
            //    currentBlock = func;
            //}
        }

        private static void TokenizeIf()
        {
            IfBlock ifblock = IfBlock.Parse(tokens);

            if (currentBlock != null)
            {
                blockstack.Push(currentBlock);
                currentBlock = ifblock;
            }
        }

        private static void TokenizeElseIf()
        {
            ElseIfBlock elseifblock = ElseIfBlock.Parse(tokens);

            if (currentBlock != null)
            {
                blockstack.Push(currentBlock);
                currentBlock = elseifblock;
            }
        }

        private static void TokenizeElse()
        {
            if (currentBlock != null)
            {
                blockstack.Push(currentBlock);
                currentBlock = new ElseBlock();
            }
        }

        private static void TokenizeRepeat()
        {
            if (currentBlock != null)
            {
                blockstack.Push(currentBlock);
                currentBlock = new RepeatBlock();
                //_tree.Add(currentBlock);
                tokens.Pos++;
            }
        }

        private static void TokenizeIdent()
        {
            Token tok = tokens.PeekToken();
            switch (tok.TokenName)
            {
                case Lexer.Tokens.Equal: TokenizeAssign(); break;
                case Lexer.Tokens.LeftParan: TokenizeCall(); break;
                default: throw new Exception($"Unexpected token after identifier: {tok.TokenName}'{tok.TokenValue}'");
            }
        }

        private static void TokenizeAssign()
        {
            tokens.Pos = tokens.Pos + 2;
            bool isCallAssignment = tokens.PeekToken().TokenName == Lexer.Tokens.LeftParan;
            tokens.Pos = tokens.Pos - 3;

            if (isCallAssignment)
            {
                // variable = call()
                AssignCall ac = AssignCall.Parse(tokens);
                currentBlock.AddStmt(ac);
            }
            else
            {
                // regular variable assignment
                Assign a = Assign.Parse(tokens);
                currentBlock.AddStmt(a);
            }
        }

        private static void TokenizeCall() => currentBlock.AddStmt(Call.Parse(tokens));

        private static void TokenizeReturn()
        {
            Return r = Return.Parse(tokens);
            currentBlock.AddStmt(r);
        }

        private static void TokenizeRightParan()
        {
            if (currentBlock is Func)
            {
                currentBlock.AddStmt(new Return(null));
                _tree.Add(currentBlock);
                currentBlock = null;
                tokens.Pos--;
            }
            else if (currentBlock is IfBlock || currentBlock is ElseIfBlock || currentBlock is ElseBlock)
            {
                currentBlock.AddStmt(new EndIf());
                Block block = currentBlock;

                if (blockstack.Count > 0)
                {
                    currentBlock = blockstack.Pop();
                    currentBlock.AddStmt(block);
                }
                tokens.Pos++;
            }
            else if (currentBlock is RepeatBlock)
            {
                Block block = currentBlock;

                if (blockstack.Count > 0)
                {
                    currentBlock = blockstack.Pop();
                    currentBlock.AddStmt(block);
                }
            }
        }

        private static void TokenizeRightBrace()
        {
            if (currentBlock != null)
            {
                if (currentBlock is Func)
                {
                    currentBlock.AddStmt(new Return(null));
                    currentBlock = null;
                }

                if (blockstack.Count > 0)
                {
                    blockstack.Peek().AddStmt(currentBlock);
                    currentBlock = blockstack.Pop();
                }
            }
        }

        private static void TokenizeEOF()
        {
            _tree.Add(currentBlock);
            running = false;
        }
        #endregion

        static string ParseImport()
        {
            string ret = "";
            Token t = tokens.GetToken();

            if (t.TokenName == Lexer.Tokens.Ident)
            {
                ret = t.TokenValue;
            }

            return ret;
        }
    }
    #endregion
}
