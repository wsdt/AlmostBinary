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
    class Parser
    {
        #region fields
        private static ILogger Log => Serilog.Log.ForContext<Parser>();
        private static List<Stmt>? _tree;
        static TokenList? tokens;
        static Block? currentBlock;
        static Stack<Block>? blockstack;
        static bool running;
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
            Log.Here().Verbose($"Starting to parse tokenList -> {JsonSerializer.Serialize(tokens.Tokens)}");

            while (running)
            {
                Token? tok;
                try
                {
                    tok = tokens.GetToken();
                }
                catch (Exception ex)
                {
                    throw new Exception($"Could not get token from token list -> {JsonSerializer.Serialize(tokens)}", ex);
                }

                if (tok.TokenName == Lexer.Tokens.Import)
                {
                    Startup.Imports.Add(ParseImport());
                }
                else if (tok.TokenName == Lexer.Tokens.Function)
                {
                    Func func = Func.Parse(tokens);

                    if (currentBlock == null)
                    {
                        currentBlock = func;
                    }
                    else
                    {
                        currentBlock.AddStmt(new Return(null));
                        _tree.Add(currentBlock);
                        currentBlock = func;
                    }
                }
                else if (tok.TokenName == Lexer.Tokens.If)
                {
                    IfBlock ifblock = IfBlock.Parse(tokens);

                    if (currentBlock != null)
                    {
                        blockstack.Push(currentBlock);
                        currentBlock = ifblock;
                    }
                }
                else if (tok.TokenName == Lexer.Tokens.ElseIf)
                {
                    ElseIfBlock elseifblock = ElseIfBlock.Parse(tokens);

                    if (currentBlock != null)
                    {
                        blockstack.Push(currentBlock);
                        currentBlock = elseifblock;
                    }
                }
                else if (tok.TokenName == Lexer.Tokens.Else)
                {
                    if (currentBlock != null)
                    {
                        blockstack.Push(currentBlock);
                        currentBlock = new ElseBlock();
                    }
                }
                else if (tok.TokenName == Lexer.Tokens.Repeat)
                {
                    if (currentBlock != null)
                    {
                        blockstack.Push(currentBlock);
                        currentBlock = new RepeatBlock();
                    }
                }
                else if (tok.TokenName == Lexer.Tokens.Ident)
                {
                    if (tokens.Peek().TokenName == Lexer.Tokens.Equal)
                    {
                        tokens.pos = tokens.pos + 2;
                        bool isCallAssignment = tokens.Peek().TokenName == Lexer.Tokens.LeftParan;
                        tokens.pos = tokens.pos - 3;

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
                    else if (tokens.Peek().TokenName == Lexer.Tokens.LeftParan)
                    {
                        tokens.pos--;
                        Call c = Call.Parse(tokens);
                        currentBlock.AddStmt(c);
                    }
                }
                else if (tok.TokenName == Lexer.Tokens.Return)
                {
                    Return r = Return.Parse(tokens);
                    currentBlock.AddStmt(r);
                }
                else if (tok.TokenName == Lexer.Tokens.RightParan)
                {
                    if (currentBlock is Func)
                    {
                        currentBlock.AddStmt(new Return(null));
                        _tree.Add(currentBlock);
                        currentBlock = null;
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
                else if (tok.TokenName == Lexer.Tokens.EOF)
                {
                    _tree.Add(currentBlock);
                    running = false;
                }
            }
        }

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
