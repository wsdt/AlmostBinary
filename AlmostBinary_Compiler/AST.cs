using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmostBinary_Compiler
{
    class Stmt { }

    class Expr { }

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
