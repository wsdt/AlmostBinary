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
                    Func func = ParseFunc();

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
                    IfBlock ifblock = ParseIf();

                    if (_currentBlock != null)
                    {
                        _blockstack.Push(_currentBlock);
                        _currentBlock = ifblock;
                    }
                }
                else if (tok.TokenName == Lexer.Tokens.ElseIf)
                {
                    ElseIfBlock elseifblock = ParseElseIf();

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
                        Assign a = ParseAssign();
                        _currentBlock.AddStmt(a);
                    }
                    else if (_tokens.PeekToken().TokenName == Lexer.Tokens.LeftParan)
                    {
                        _tokens.Pos--;
                        Call c = ParseCall();
                        _currentBlock.AddStmt(c);
                    }
                }
                else if (tok.TokenName == Lexer.Tokens.Return)
                {
                    Return r = ParseReturn();
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

        static Func ParseFunc()
        {
            string ident = "";
            List<string> vars = new List<string>();

            if (_tokens.PeekToken().TokenName == Lexer.Tokens.Ident)
            {
                ident = _tokens.GetToken().TokenValue.ToString();
            }

            if (_tokens.PeekToken().TokenName == Lexer.Tokens.LeftParan)
            {
                _tokens.Pos++;
            }

            if (_tokens.PeekToken().TokenName == Lexer.Tokens.RightParan)
            {
                _tokens.Pos++;
            }
            else
            {
                vars = ParseFuncArgs();
            }

            if (_tokens.PeekToken().TokenName == Lexer.Tokens.LeftBrace)
            {
                _tokens.Pos++;
            }

            return new Func(ident, vars);
        }

        static IfBlock ParseIf()
        {
            IfBlock ret = null;
            Symbol op = 0;

            if (_tokens.PeekToken().TokenName == Lexer.Tokens.LeftParan)
            {
                _tokens.Pos++;
            }

            Expr lexpr = ParseExpr();

            if (_tokens.PeekToken().TokenName == Lexer.Tokens.DoubleEqual)
            {
                op = Symbol.doubleEqual;
                _tokens.Pos++;
            }
            else if (_tokens.PeekToken().TokenName == Lexer.Tokens.NotEqual)
            {
                op = Symbol.notEqual;
                _tokens.Pos++;
            }

            Expr rexpr = ParseExpr();

            if (_tokens.PeekToken().TokenName == Lexer.Tokens.RightParan)
            {
                _tokens.Pos++;
            }

            ret = new IfBlock(lexpr, op, rexpr);

            return ret;
        }

        static ElseIfBlock ParseElseIf()
        {
            ElseIfBlock ret = null;
            Symbol op = 0;

            if (_tokens.PeekToken().TokenName == Lexer.Tokens.LeftParan)
            {
                _tokens.Pos++;
            }

            Expr lexpr = ParseExpr();

            if (_tokens.PeekToken().TokenName == Lexer.Tokens.DoubleEqual)
            {
                op = Symbol.doubleEqual;
                _tokens.Pos++;
            }
            else if (_tokens.PeekToken().TokenName == Lexer.Tokens.NotEqual)
            {
                op = Symbol.notEqual;
                _tokens.Pos++;
            }

            Expr rexpr = ParseExpr();

            if (_tokens.PeekToken().TokenName == Lexer.Tokens.RightParan)
            {
                _tokens.Pos++;
            }

            ret = new ElseIfBlock(lexpr, op, rexpr);

            return ret;
        }

        static Assign ParseAssign()
        {
            Assign ret = null;
            string ident = "";

            Token t = _tokens.GetToken();
            ident = t.TokenValue.ToString();

            _tokens.Pos++;

            Expr value = ParseExpr();

            ret = new Assign(ident, value);

            return ret;
        }

        static Call ParseCall()
        {
            string ident = "";
            Token tok = _tokens.GetToken();
            List<Expr> args = new List<Expr>();

            if (tok.TokenName == Lexer.Tokens.Ident)
            {
                ident = tok.TokenValue.ToString();
            }

            if (_tokens.PeekToken().TokenName == Lexer.Tokens.LeftParan)
            {
                _tokens.Pos++;
            }

            if (_tokens.PeekToken().TokenName == Lexer.Tokens.RightParan)
            {
                _tokens.Pos++;
            }
            else
            {
                args = ParseCallArgs();
            }

            return new Call(ident, args);
        }

        static Return ParseReturn()
        {
            return new Return(ParseExpr());
        }

        static Expr ParseExpr()
        {
            Expr ret = null;
            Token t = _tokens.GetToken();

            if (_tokens.PeekToken().TokenName == Lexer.Tokens.LeftParan)
            {
                string ident = "";

                if (t.TokenName == Lexer.Tokens.Ident)
                {
                    ident = t.TokenValue.ToString();
                }

                _tokens.Pos++;

                if (_tokens.PeekToken().TokenName == Lexer.Tokens.RightParan)
                {
                    ret = new CallExpr(ident, new List<Expr>());
                }
                else
                {
                    ret = new CallExpr(ident, ParseCallArgs());
                }
            }
            else if (t.TokenName == Lexer.Tokens.IntLiteral)
            {
                IntLiteral i = new IntLiteral(Convert.ToInt32(t.TokenValue.ToString()));
                ret = i;
            }
            else if (t.TokenName == Lexer.Tokens.StringLiteral)
            {
                StringLiteral s = new StringLiteral(t.TokenValue.ToString());
                ret = s;
            }
            else if (t.TokenName == Lexer.Tokens.Ident)
            {
                string ident = t.TokenValue.ToString();

                Ident i = new Ident(ident);
                ret = i;
            }
            else if (t.TokenName == Lexer.Tokens.LeftParan)
            {
                Expr e = ParseExpr();

                if (_tokens.PeekToken().TokenName == Lexer.Tokens.RightParan)
                {
                    _tokens.Pos++;
                }

                ParanExpr p = new ParanExpr(e);

                if (_tokens.PeekToken().TokenName == Lexer.Tokens.Add)
                {
                    _tokens.Pos++;
                    Expr expr = ParseExpr();
                    ret = new MathExpr(p, Symbol.add, expr);
                }
                else if (_tokens.PeekToken().TokenName == Lexer.Tokens.Sub)
                {
                    _tokens.Pos++;
                    Expr expr = ParseExpr();
                    ret = new MathExpr(p, Symbol.sub, expr);
                }
                else if (_tokens.PeekToken().TokenName == Lexer.Tokens.Mul)
                {
                    _tokens.Pos++;
                    Expr expr = ParseExpr();
                    ret = new MathExpr(p, Symbol.mul, expr);
                }
                else if (_tokens.PeekToken().TokenName == Lexer.Tokens.Div)
                {
                    _tokens.Pos++;
                    Expr expr = ParseExpr();
                    ret = new MathExpr(p, Symbol.div, expr);
                }
                else
                {
                    ret = p;
                }
            }

            if (_tokens.PeekToken().TokenName == Lexer.Tokens.Add 
                || _tokens.PeekToken().TokenName == Lexer.Tokens.Sub 
                || _tokens.PeekToken().TokenName == Lexer.Tokens.Mul 
                || _tokens.PeekToken().TokenName == Lexer.Tokens.Div)
            {
                Expr lexpr = ret;
                Symbol op = 0;

                if (_tokens.PeekToken().TokenName == Lexer.Tokens.Add)
                {
                    op = Symbol.add;
                }
                else if (_tokens.PeekToken().TokenName == Lexer.Tokens.Sub)
                {
                    op = Symbol.sub;
                }
                else if (_tokens.PeekToken().TokenName == Lexer.Tokens.Mul)
                {
                    op = Symbol.mul;
                }
                else if (_tokens.PeekToken().TokenName == Lexer.Tokens.Div)
                {
                    op = Symbol.div;
                }

                _tokens.Pos++;

                Expr rexpr = ParseExpr();

                ret = new MathExpr(lexpr, op, rexpr);
            }

            return ret;
        }

        static List<string> ParseFuncArgs()
        {
            List<string> ret = new List<string>();

            while (true)
            {
                Token tok = _tokens.GetToken();

                if (tok.TokenName == Lexer.Tokens.Ident)
                {
                    ret.Add(tok.TokenValue.ToString());
                }

                if (_tokens.PeekToken().TokenName == Lexer.Tokens.Comma)
                {
                    _tokens.Pos++;
                }
                else if (_tokens.PeekToken().TokenName == Lexer.Tokens.RightParan)
                {
                    _tokens.Pos++;
                    break;
                }
            }

            return ret;
        }

        static List<Expr> ParseCallArgs()
        {
            List<Expr> ret = new List<Expr>();

            while (true)
            {
                ret.Add(ParseExpr());

                if (_tokens.PeekToken().TokenName == Lexer.Tokens.Comma)
                {
                    _tokens.Pos++;
                }
                else if (_tokens.PeekToken().TokenName == Lexer.Tokens.RightParan)
                {
                    _tokens.Pos++;
                    break;
                }
            }

            return ret;
        }
        #endregion
    }
}
