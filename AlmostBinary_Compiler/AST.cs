using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmostBinary_Compiler
{
 
    class Stmt { }

    class Expr { }

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
