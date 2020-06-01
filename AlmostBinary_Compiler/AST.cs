using System;
using System.Collections.Generic;

namespace AlmostBinary_Compiler
{

    class Stmt { }

    class Expr
    {
        #region methods
        public static Expr Parse(TokenList _tokens)
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
                    ret = new CallExpr(ident, Call.ParseArgs(_tokens));
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
                Expr e = Expr.Parse(_tokens);

                if (_tokens.PeekToken().TokenName == Lexer.Tokens.RightParan)
                {
                    _tokens.Pos++;
                }

                ParanExpr p = new ParanExpr(e);

                if (_tokens.PeekToken().TokenName == Lexer.Tokens.Add)
                {
                    _tokens.Pos++;
                    Expr expr = Expr.Parse(_tokens);
                    ret = new MathExpr(p, Symbol.add, expr);
                }
                else if (_tokens.PeekToken().TokenName == Lexer.Tokens.Sub)
                {
                    _tokens.Pos++;
                    Expr expr = Expr.Parse(_tokens);
                    ret = new MathExpr(p, Symbol.sub, expr);
                }
                else if (_tokens.PeekToken().TokenName == Lexer.Tokens.Mul)
                {
                    _tokens.Pos++;
                    Expr expr = Expr.Parse(_tokens);
                    ret = new MathExpr(p, Symbol.mul, expr);
                }
                else if (_tokens.PeekToken().TokenName == Lexer.Tokens.Div)
                {
                    _tokens.Pos++;
                    Expr expr = Expr.Parse(_tokens);
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

                Expr rexpr = Expr.Parse(_tokens);

                ret = new MathExpr(lexpr, op, rexpr);
            }

            return ret;
        }
        #endregion
    }

    #region statements
    class Block : Stmt
    {
        #region properties
        public List<Stmt> Statements { get; set; }
        #endregion

        #region ctor
        public Block()
        {
            Statements = new List<Stmt>();
        }
        #endregion

        #region methods
        public void AddStmt(Stmt stmt)
        {
            Statements.Add(stmt);
        }
        #endregion
    }

    class Assign : Stmt
    {
        #region properties
        public string Ident { get; set; }
        public Expr Value { get; set; }
        #endregion

        #region ctor
        public Assign(string i, Expr v)
        {
            Ident = i;
            Value = v;
        }
        #endregion

        #region methods
        public static Assign Parse(TokenList _tokens)
        {
            Assign ret = null;
            string ident = "";

            Token t = _tokens.GetToken();
            ident = t.TokenValue.ToString();

            _tokens.Pos++;

            Expr value = Expr.Parse(_tokens);

            ret = new Assign(ident, value);

            return ret;
        }
        #endregion
    }

    class Call : Stmt
    {
        #region properties
        public string Ident { get; set; }
        public List<Expr> Args { get; set; }
        #endregion

        #region ctor
        public Call(string i, List<Expr> a)
        {
            Ident = i;
            Args = a;
        }
        #endregion

        #region methods
        public static Call Parse(TokenList _tokens)
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
                args = Call.ParseArgs(_tokens);
            }

            return new Call(ident, args);
        }

        public static List<Expr> ParseArgs(TokenList _tokens)
        {
            List<Expr> ret = new List<Expr>();

            while (true)
            {
                ret.Add(Expr.Parse(_tokens));

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

    class Return : Stmt
    {
        #region properties
        public Expr Expr { get; set; }
        #endregion

        #region ctor
        public Return(Expr e)
        {
            Expr = e;
        }
        #endregion

        #region methods
        public static Return Parse(TokenList _tokens)
        {
            return new Return(Expr.Parse(_tokens));
        }
        #endregion
    }
    #endregion

    #region blocks
    class Func : Block
    {
        #region properties
        public string Ident { get; set; }
        public List<string> Vars { get; set; }
        #endregion

        #region ctor
        public Func(string i, List<string> v)
        {
            Ident = i;
            Vars = v;
        }
        #endregion

        #region methods
        public static Func Parse(TokenList _tokens)
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
                vars = ParseArgs(_tokens);
            }

            if (_tokens.PeekToken().TokenName == Lexer.Tokens.LeftBrace)
            {
                _tokens.Pos++;
            }

            return new Func(ident, vars);
        }

        public static List<string> ParseArgs(TokenList _tokens)
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
        #endregion
    }

    class IfBlock : Block
    {
        #region properties
        public Expr LeftExpr { get; set; }
        public Symbol Op { get; set; }
        public Expr RightExpr { get; set; }
        #endregion

        #region ctor
        public IfBlock(Expr lexpr, Symbol o, Expr rexpr)
        {
            LeftExpr = lexpr;
            Op = o;
            RightExpr = rexpr;
        }
        #endregion

        #region methods
        public static IfBlock Parse(TokenList _tokens)
        {
            IfBlock ret = null;
            Symbol op = 0;

            if (_tokens.PeekToken().TokenName == Lexer.Tokens.LeftParan)
            {
                _tokens.Pos++;
            }

            Expr lexpr = Expr.Parse(_tokens);

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

            Expr rexpr = Expr.Parse(_tokens);

            if (_tokens.PeekToken().TokenName == Lexer.Tokens.RightParan)
            {
                _tokens.Pos++;
            }

            ret = new IfBlock(lexpr, op, rexpr);

            return ret;
        }
        #endregion
    }

    class ElseIfBlock : Block
    {
        #region properties
        public Expr LeftExpr { get; set; }
        public Symbol Op { get; set; }
        public Expr RightExpr { get; set; }
        #endregion

        #region ctor
        public ElseIfBlock(Expr lexpr, Symbol o, Expr rexpr)
        {
            LeftExpr = lexpr;
            Op = o;
            RightExpr = rexpr;
        }
        #endregion

        #region methods
        public static ElseIfBlock Parse(TokenList _tokens)
        {
            ElseIfBlock ret = null;
            Symbol op = 0;

            if (_tokens.PeekToken().TokenName == Lexer.Tokens.LeftParan)
            {
                _tokens.Pos++;
            }

            Expr lexpr = Expr.Parse(_tokens);

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

            Expr rexpr = Expr.Parse(_tokens);

            if (_tokens.PeekToken().TokenName == Lexer.Tokens.RightParan)
            {
                _tokens.Pos++;
            }

            ret = new ElseIfBlock(lexpr, op, rexpr);

            return ret;
        }
        #endregion
    }

    class ElseBlock : Block { }

    class EndIf : Block { }

    class RepeatBlock : Block { }
    #endregion

    #region expressions
    class IntLiteral : Expr
    {
        #region properties
        public int Value { get; set; }
        #endregion

        #region ctor
        public IntLiteral(int v)
        {
            Value = v;
        }
        #endregion
    }

    class StringLiteral : Expr
    {
        #region properties
        public string Value { get; set; }
        #endregion

        #region ctor
        public StringLiteral(string v)
        {
            Value = v;
        }
        #endregion
    }

    class Ident : Expr
    {
        #region properties
        public string Value { get; set; }
        #endregion

        #region ctor
        public Ident(string v)
        {
            Value = v;
        }
        #endregion
    }

    class MathExpr : Expr
    {
        #region properties
        public Expr LeftExpr { get; set; }
        public Symbol Op { get; set; }
        public Expr RightExpr { get; set; }
        #endregion

        #region ctor
        public MathExpr(Expr lexpr, Symbol o, Expr rexpr)
        {
            LeftExpr = lexpr;
            Op = o;
            RightExpr = rexpr;
        }
        #endregion
    }

    class ParanExpr : Expr
    {
        #region properties
        public Expr Value { get; set; }
        #endregion

        #region ctor
        public ParanExpr(Expr v)
        {
            Value = v;
        }
        #endregion
    }

    class CallExpr : Expr
    {
        #region properties
        public string Ident { get; set; }
        public List<Expr> Args { get; set; }
        #endregion

        #region ctor
        public CallExpr(string i, List<Expr> a)
        {
            Ident = i;
            Args = a;
        }
        #endregion
    }
    #endregion

    enum Symbol
    {
        add = 0,
        sub = 1,
        mul = 2,
        div = 3,
        equal = 4,
        doubleEqual = 5,
        notEqual = 6,
        leftParan = 7,
        rightParan = 8,
        leftBrace = 9,
        rightbrace = 10
    }
}
