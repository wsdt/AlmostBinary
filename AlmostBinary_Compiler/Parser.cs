﻿using AlmostBinary_Compiler.utils;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace AlmostBinary_Compiler
{
    class Parser
    {
        #region fields
        private static ILogger Log => Serilog.Log.ForContext<Parser>();
        private List<string> _imports = new List<string>();
        TokenList? _tokens;
        Block? _currentBlock = null;
        Stack<Block> _blockstack = new Stack<Block>();
        List<Stmt> _tree = new List<Stmt>();
        #endregion

        #region properties 
        public List<string> Imports { get => _imports; set => _imports = value; }
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

                switch (tok.TokenName)
                {
                    case Lexer.Tokens.Import: Imports.Add(ParseImport()); break;
                    case Lexer.Tokens.Function: ParseFunction(); break;
                    case Lexer.Tokens.If: ParseIf(); break;
                    case Lexer.Tokens.ElseIf: ParseElseIf(); break;
                    case Lexer.Tokens.Else: ParseElse(); break;
                    case Lexer.Tokens.Repeat: ParseRepeat(); break;
                    case Lexer.Tokens.Ident: ParseIdent(); break;
                    case Lexer.Tokens.Return: ParseReturn(); break;
                    case Lexer.Tokens.RightBrace: ParseRightBrace(); break;
                    case Lexer.Tokens.EOF: ParseEOF(); break;
                    case Lexer.Tokens.LeftParan: _tokens.Pos -= 2; break;
                    case Lexer.Tokens.LeftBrace: break; // do not increment as GetToken() will use next one
                    default: throw new Exception($"Unexpected token: '{tok.TokenValue}' (Type: {tok.TokenName})");
                }
            }
        }
        #endregion

        #region methods
        private string ParseImport()
        {
            string ret;
            Token t = _tokens.GetToken();

            if (t.TokenName == Lexer.Tokens.Ident)
            {
                ret = t.TokenValue;
            } else
            {
                throw new Exception($"Expected an identifier after an import statement: TokenName->'{t.TokenName}', TokenValue->'{t.TokenValue}'");
            }

            return ret;
        }

        private void ParseFunction()
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

        private void ParseIf()
        {
            IfBlock ifblock = IfBlock.Parse(_tokens);

            if (_currentBlock != null)
            {
                _blockstack.Push(_currentBlock);
                _currentBlock = ifblock;
            }
        }

        private void ParseElseIf()
        {
            ElseIfBlock elseifblock = ElseIfBlock.Parse(_tokens);

            if (_currentBlock != null)
            {
                _blockstack.Push(_currentBlock);
                _currentBlock = elseifblock;
            }
        }

        private void ParseElse()
        {
            if (_currentBlock != null)
            {
                _blockstack.Push(_currentBlock);
                _currentBlock = new ElseBlock();
            }
        }

        private void ParseRepeat()
        {
            if (_currentBlock != null)
            {
                _blockstack.Push(_currentBlock);
                _currentBlock = new RepeatBlock();
            }
        }

        private void ParseIdent()
        {
            if (_tokens.PeekToken().TokenName == Lexer.Tokens.Equal)
            {
                _tokens.Pos--;
                Assign a = Assign.Parse(_tokens);
                _tokens.Pos++;
                _currentBlock.AddStmt(a);
            }
            else if (_tokens.PeekToken().TokenName == Lexer.Tokens.LeftParan)
            {
                _tokens.Pos--;
                Call c = Call.Parse(_tokens);
                _currentBlock.AddStmt(c);
            }
        }

        private void ParseReturn()
        {
            Return r = Return.Parse(_tokens);
            _currentBlock.AddStmt(r);
        }

        private void ParseRightBrace()
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

        private void ParseEOF()
        {
            _tree.Add(_currentBlock);
        }
        #endregion
    }
}
