using System.Collections.Generic;

namespace AlmostBinary_Compiler
{
    internal sealed class Compiler
    {
        #region fields
        string code;
        int repeats = 0;
        #endregion

        #region ctor
        public Compiler(List<Stmt> list)
        {
            code = "";
            CompileStmtList(list);
        }
        #endregion

        #region methods
        private void CompileStmtList(List<Stmt> statements)
        {
            foreach (Stmt s in statements)
            {
                if (s is Func)
                {
                    CompileFunc((Func)s);
                }
                else if (s is IfBlock)
                {
                    CompileIf((IfBlock)s);
                }
                else if (s is ElseIfBlock)
                {
                    CompileElseIf((ElseIfBlock)s);
                }
                else if (s is ElseBlock)
                {
                    CompileElse((ElseBlock)s);
                }
                else if (s is EndIf)
                {
                    Write("endif");
                }
                else if (s is RepeatBlock)
                {
                    CompileRepeat((RepeatBlock)s);
                }
                else if (s is Assign)
                {
                    CompileAssign((Assign)s);
                }
                else if (s is Call)
                {
                    CompileCall((Call)s);
                }
                else if (s is Return)
                {
                    if (((Return)s).Expr == null)
                    {
                        Write("ret");
                    }
                    else
                    {
                        CompileExpr(((Return)s).Expr);
                        Write("ret");
                    }
                }
            }
        }

        private void CompileFunc(Func data)
        {
            Write(":" + data.Ident);

            foreach (string s in data.Vars)
            {
                Write("setVar " + s);
            }

            CompileStmtList(data.Statements);
        }

        private void CompileIf(IfBlock data)
        {
            CompileExpr(data.LeftExpr);
            CompileExpr(data.RightExpr);

            if (data.Op == Symbol.doubleEqual)
            {
                Write("ife");
            }
            else if (data.Op == Symbol.notEqual)
            {
                Write("ifn");
            }

            CompileStmtList(data.Statements);
        }

        private void CompileElseIf(ElseIfBlock data)
        {
            CompileExpr(data.LeftExpr);
            CompileExpr(data.RightExpr);

            if (data.Op == Symbol.doubleEqual)
            {
                Write("elseife");
            }
            else if (data.Op == Symbol.notEqual)
            {
                Write("elseifn");
            }

            CompileStmtList(data.Statements);
        }

        private void CompileElse(ElseBlock data)
        {
            Write("else");
            CompileStmtList(data.Statements);
        }

        private void CompileRepeat(RepeatBlock data)
        {
            string name = ".repeat" + repeats.ToString();
            repeats++;
            Write(name);
            CompileStmtList(data.Statements);
            Write("goto " + name);
        }

        private void CompileAssign(Assign data)
        {
            CompileExpr(data.Value);
            Write("setVar " + data.Ident);
        }

        private void CompileCall(Call data)
        {
            data.Args.Reverse();

            foreach (Expr e in data.Args)
            {
                CompileExpr(e);
            }

            Write("call " + data.Ident);
        }

        private void CompileExpr(Expr data)
        {
            if (data is IntLiteral)
            {
                Write("pushInt " + ((IntLiteral)data).Value);
            }
            else if (data is StringLiteral)
            {
                Write("pushString " + ((StringLiteral)data).Value);
            }
            else if (data is Ident)
            {
                Write("pushVar " + ((Ident)data).Value);
            }
            else if (data is CallExpr)
            {
                foreach (Expr e in ((CallExpr)data).Args)
                {
                    CompileExpr(e);
                }

                Write("call " + ((CallExpr)data).Ident);
            }
            else if (data is MathExpr)
            {
                CompileExpr(((MathExpr)data).LeftExpr);
                CompileExpr(((MathExpr)data).RightExpr);

                if (((MathExpr)data).Op == Symbol.add)
                {
                    Write("add");
                }
                else if (((MathExpr)data).Op == Symbol.sub)
                {
                    Write("sub");
                }
                else if (((MathExpr)data).Op == Symbol.mul)
                {
                    Write("mul");
                }
                else if (((MathExpr)data).Op == Symbol.div)
                {
                    Write("div");
                }
            }
            else if (data is ParanExpr)
            {
                CompileExpr(((ParanExpr)data).Value);
            }
        }

        private void Write(string data)
        {
            if (code == "")
            {
                code += data;
            }
            else
            {
                code += "\n" + data;
            }
        }

        public string GetCode()
        {
            return code;
        }
    }
    #endregion
}
