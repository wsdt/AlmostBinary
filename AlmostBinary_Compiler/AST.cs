using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmostBinary_Compiler
{
    class Stmt { }

    class Expr {
        #region methods
        internal static Expr Parse(TokenList tokens)
        {
            Expr ret = null;
            Token t = tokens.GetToken();

            if (tokens.Peek().TokenName == Lexer.Tokens.LeftParan)
            {
                string ident = "";

                if (t.TokenName == Lexer.Tokens.Ident)
                {
                    ident = t.TokenValue.ToString();
                }

                tokens.pos++;

                if (tokens.Peek().TokenName == Lexer.Tokens.RightParan)
                {
                    ret = new CallExpr(ident, new List<Expr>());
                }
                else
                {
                    ret = new CallExpr(ident, Call.ParseArgs(tokens));
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
                Expr e = Expr.Parse(tokens);

                if (tokens.Peek().TokenName == Lexer.Tokens.RightParan)
                {
                    tokens.pos++;
                }

                ParanExpr p = new ParanExpr(e);

                if (tokens.Peek().TokenName == Lexer.Tokens.Add)
                {
                    tokens.pos++;
                    Expr expr = Expr.Parse(tokens);
                    ret = new MathExpr(p, Symbol.add, expr);
                }
                else if (tokens.Peek().TokenName == Lexer.Tokens.Sub)
                {
                    tokens.pos++;
                    Expr expr = Expr.Parse(tokens);
                    ret = new MathExpr(p, Symbol.sub, expr);
                }
                else if (tokens.Peek().TokenName == Lexer.Tokens.Mul)
                {
                    tokens.pos++;
                    Expr expr = Expr.Parse(tokens);
                    ret = new MathExpr(p, Symbol.mul, expr);
                }
                else if (tokens.Peek().TokenName == Lexer.Tokens.Div)
                {
                    tokens.pos++;
                    Expr expr = Expr.Parse(tokens);
                    ret = new MathExpr(p, Symbol.div, expr);
                }
                else
                {
                    ret = p;
                }
            }

            if (tokens.Peek().TokenName == Lexer.Tokens.Add || tokens.Peek().TokenName == Lexer.Tokens.Sub || tokens.Peek().TokenName == Lexer.Tokens.Mul || tokens.Peek().TokenName == Lexer.Tokens.Div)
            {
                Expr lexpr = ret;
                Symbol op = 0;

                if (tokens.Peek().TokenName == Lexer.Tokens.Add)
                {
                    op = Symbol.add;
                }
                else if (tokens.Peek().TokenName == Lexer.Tokens.Sub)
                {
                    op = Symbol.sub;
                }
                else if (tokens.Peek().TokenName == Lexer.Tokens.Mul)
                {
                    op = Symbol.mul;
                }
                else if (tokens.Peek().TokenName == Lexer.Tokens.Div)
                {
                    op = Symbol.div;
                }

                tokens.pos++;

                Expr rexpr = Expr.Parse(tokens);

                ret = new MathExpr(lexpr, op, rexpr);
            }

            return ret;
        }
#endregion
    }

    class Block : Stmt
    {
        #region fields
        public List<Stmt> statements;
        #endregion

        #region ctor
        public Block()
        {
            statements = new List<Stmt>();
        }
        #endregion

        #region methods
        public void AddStmt(Stmt stmt)
        {
            statements.Add(stmt);
        }
        #endregion
    }

    class Func : Block
    {
        #region fields
        public string ident;
        public List<string> vars;
        #endregion

        #region ctor
        public Func(string i, List<string> v)
        {
            ident = i;
            vars = v;
        }
        #endregion

        #region methods
        internal static Func Parse(TokenList tokens)
        {
            string ident = "";
            List<string> vars = new List<string>();

            if (tokens.Peek().TokenName == Lexer.Tokens.Ident)
            {
                ident = tokens.GetToken().TokenValue.ToString();
            }

            if (tokens.Peek().TokenName == Lexer.Tokens.LeftParan)
            {
                tokens.pos++;
            }

            if (tokens.Peek().TokenName == Lexer.Tokens.RightParan)
            {
                tokens.pos++;
            }
            else
            {
                vars = Func.ParseArgs(tokens);
            }

            if (tokens.Peek().TokenName == Lexer.Tokens.LeftBrace)
            {
                tokens.pos++;
            }

            return new Func(ident, vars);
        }

        internal static List<string> ParseArgs(TokenList tokens)
        {
            List<string> ret = new List<string>();

            while (true)
            {
                Token tok = tokens.GetToken();

                if (tok.TokenName == Lexer.Tokens.Ident)
                {
                    ret.Add(tok.TokenValue.ToString());
                }

                if (tokens.Peek().TokenName == Lexer.Tokens.Comma)
                {
                    tokens.pos++;
                }
                else if (tokens.Peek().TokenName == Lexer.Tokens.RightParan)
                {
                    tokens.pos++;
                    break;
                }
            }

            return ret;
        }
        #endregion
    }

    class IfBlock : Block
    {
        #region fields
        public Expr leftExpr;
        public Symbol op;
        public Expr rightExpr;
        #endregion

        #region ctor
        public IfBlock(Expr lexpr, Symbol o, Expr rexpr)
        {
            leftExpr = lexpr;
            op = o;
            rightExpr = rexpr;
        }
        #endregion

        #region methods
        internal static IfBlock Parse(TokenList tokens)
        {
            IfBlock ret = null;
            Symbol op = 0;

            if (tokens.Peek().TokenName == Lexer.Tokens.LeftParan)
            {
                tokens.pos++;
            }

            Expr lexpr = Expr.Parse(tokens);

            if (tokens.Peek().TokenName == Lexer.Tokens.DoubleEqual)
            {
                op = Symbol.doubleEqual;
                tokens.pos++;
            }
            else if (tokens.Peek().TokenName == Lexer.Tokens.NotEqual)
            {
                op = Symbol.notEqual;
                tokens.pos++;
            }

            Expr rexpr = Expr.Parse(tokens);

            if (tokens.Peek().TokenName == Lexer.Tokens.RightParan)
            {
                tokens.pos++;
            }

            ret = new IfBlock(lexpr, op, rexpr);

            return ret;
        }
        #endregion
    }

    class ElseIfBlock : Block
    {
        #region fields
        public Expr leftExpr;
        public Symbol op;
        public Expr rightExpr;
        #endregion

        #region ctor
        public ElseIfBlock(Expr lexpr, Symbol o, Expr rexpr)
        {
            leftExpr = lexpr;
            op = o;
            rightExpr = rexpr;
        }
        #endregion

        #region methods
        internal static ElseIfBlock Parse(TokenList tokens)
        {
            ElseIfBlock ret = null;
            Symbol op = 0;

            if (tokens.Peek().TokenName == Lexer.Tokens.LeftParan)
            {
                tokens.pos++;
            }

            Expr lexpr = Expr.Parse(tokens);

            if (tokens.Peek().TokenName == Lexer.Tokens.DoubleEqual)
            {
                op = Symbol.doubleEqual;
                tokens.pos++;
            }
            else if (tokens.Peek().TokenName == Lexer.Tokens.NotEqual)
            {
                op = Symbol.notEqual;
                tokens.pos++;
            }

            Expr rexpr = Expr.Parse(tokens);

            if (tokens.Peek().TokenName == Lexer.Tokens.RightParan)
            {
                tokens.pos++;
            }

            ret = new ElseIfBlock(lexpr, op, rexpr);

            return ret;
        }
        #endregion
    }

    class ElseBlock : Block { }

    class EndIf : Block { }

    class RepeatBlock : Block { }

    class Assign : Stmt
    {
        #region fields
        public string ident;
        public Expr value;
        #endregion

        #region ctor
        public Assign(string i, Expr v)
        {
            ident = i;
            value = v;
        }
        #endregion

        #region methods
        internal static Assign Parse(TokenList tokens)
        {
            Assign ret = null;
            string ident = "";

            Token t = tokens.GetToken();
            ident = t.TokenValue.ToString();

            tokens.pos++;

            Expr value = Expr.Parse(tokens);

            ret = new Assign(ident, value);

            return ret;
        }
        #endregion
    }

    class AssignCall : Stmt
    {
        #region fields
        public string ident;
        public Call call;
        #endregion

        #region ctor
        public AssignCall(string i, Call c)
        {
            ident = i;
            call = c;
        }
        #endregion

        #region methods
        /// <summary>
        /// Parses call assignment, e.g.: name = InputString()
        /// </summary>
        /// <returns></returns>
        internal static AssignCall Parse(TokenList tokens)
        {
            Token t = tokens.GetToken();
            tokens.pos++;

            return new AssignCall(t.TokenValue.ToString(), Call.Parse(tokens));
        }
        #endregion
    }

    class Call : Stmt
    {
        #region fields
        public string ident;
        public List<Expr> args;
        #endregion

        #region ctor
        public Call(string i, List<Expr> a)
        {
            ident = i;
            args = a;
        }
        #endregion

        #region methods
        internal static Call Parse(TokenList tokens)
        {
            string ident = "";
            Token tok = tokens.GetToken();
            List<Expr> args = new List<Expr>();

            if (tok.TokenName == Lexer.Tokens.Ident)
            {
                ident = tok.TokenValue.ToString();
            }

            if (tokens.Peek().TokenName == Lexer.Tokens.LeftParan)
            {
                tokens.pos++;
            }

            if (tokens.Peek().TokenName == Lexer.Tokens.RightParan)
            {
                tokens.pos++;
            }
            else
            {
                args = Call.ParseArgs(tokens);
            }

            return new Call(ident, args);
        }

        internal static List<Expr> ParseArgs(TokenList tokens)
        {
            List<Expr> ret = new List<Expr>();

            while (true)
            {
                ret.Add(Expr.Parse(tokens));

                if (tokens.Peek().TokenName == Lexer.Tokens.Comma)
                {
                    tokens.pos++;
                }
                else if (tokens.Peek().TokenName == Lexer.Tokens.RightParan)
                {
                    tokens.pos++;
                    break;
                }
            }

            return ret;
        }
        #endregion
    }

    class Return : Stmt
    {
        #region fields
        public Expr expr;
        #endregion

        #region ctor
        public Return(Expr e)
        {
            expr = e;
        }
        #endregion

        #region methods
        internal static Return Parse(TokenList tokens) => new Return(Expr.Parse(tokens));
        #endregion
    }

    class IntLiteral : Expr
    {
        #region fields
        public int value;
        #endregion

        #region ctor
        public IntLiteral(int v)
        {
            value = v;
        }
        #endregion
    }

    class StringLiteral : Expr
    {
        #region fields
        public string value;
        #endregion

        #region ctor
        public StringLiteral(string v)
        {
            value = v;
        }
        #endregion
    }

    class Ident : Expr
    {
        #region fields
        public string value;
        #endregion

        #region ctor
        public Ident(string v)
        {
            value = v;
        }
        #endregion
    }

    class MathExpr : Expr
    {
        #region fields
        public Expr leftExpr;
        public Symbol op;
        public Expr rightExpr;
        #endregion

        #region ctor
        public MathExpr(Expr lexpr, Symbol o, Expr rexpr)
        {
            leftExpr = lexpr;
            op = o;
            rightExpr = rexpr;
        }
        #endregion
    }

    class ParanExpr : Expr
    {
        #region fields
        public Expr value;
        #endregion

        #region ctor
        public ParanExpr(Expr v)
        {
            value = v;
        }
        #endregion
    }

    class CallExpr : Expr
    {
        #region fields
        public string ident;
        public List<Expr> args;
        #endregion

        #region ctor
        public CallExpr(string i, List<Expr> a)
        {
            ident = i;
            args = a;
        }
        #endregion
    }

    #region enums
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
    #endregion
}
