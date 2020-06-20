using AlmostBinary_Binarify;
using AlmostBinary_Binarify.utils;
using AlmostBinary_Runtime.utils;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using static AlmostBinary_Binarify.BinaryConverter;

namespace AlmostBinary_Runtime
{
    class Lexer
    {
        #region fields
        private static ILogger Log => Serilog.Log.ForContext<Lexer>();
        public List<Func> funcs = new List<Func>();
        public List<Block> blocks = new List<Block>();
        public Buffer code = new Buffer();
        #endregion

        #region ctor
        public Lexer(string c)
        {
            Log.Here().Information("Starting Lexer.");
            c = c.Replace(((char)13).ToString(), "");

            Func currentFunc = null;
            Block currentBlock = null;
            int blockNumber = 0;
            Stack<Block> blockstack = new Stack<Block>();

            foreach (string a in c.Replace("\r\n", "\n").Split('\n').Select(b => b.Trim()))
            {
                Log.Here().Verbose($"Current row: '{a}'");

                if (a.StartsWith(":"))
                {
                    string op = a.Substring(1);

                    if (currentFunc == null)
                    {
                        currentFunc = new Func(op, code.buffer.Count);
                    }
                    else
                    {
                        code.Write(Opcodes.ret);
                        funcs.Add(currentFunc);
                        currentFunc = new Func(op, code.buffer.Count);
                    }
                }
                else if (a.StartsWith("."))
                {
                    string name = a.Substring(1);
                    Label l = new Label(name, code.buffer.Count());
                    currentFunc.labels.Add(l);
                }
                else if (a.StartsWith("pushInt "))
                {
                    // Enable if runtime takes responsibility for converting int literals
                    //string intLiteral = ITokens.IPartial.INT_LITERAL.BinaryString;
                    //string temp = a.Substring(10);
                    //temp = temp.Substring(
                    //        temp.IndexOf(intLiteral) + intLiteral.Length, temp.LastIndexOf(intLiteral) - intLiteral.Length
                    //    );
                    //int value = Convert.ToInt32(new Binary(binaryString: temp.Trim()).ToString());
                    int value = Convert.ToInt32(a.Substring(8));
                    code.Write(Opcodes.pushInt);
                    code.Write(value);
                }
                else if (a.StartsWith("pushString "))
                {
                    string quoteStr = ITokens.IPartial.STRING_LITERAL.BinaryString;
                    string temp = a.Substring(11);
                    string value = temp.Substring(temp.IndexOf(quoteStr) + quoteStr.Length, temp.LastIndexOf(quoteStr) - quoteStr.Length);
                    value = new Binary(binaryString: value.Trim()).ToString();
                    code.Write(Opcodes.pushString);
                    code.Write(value);
                }
                else if (a.StartsWith("pushVar "))
                {
                    string name = a.Substring(8);
                    code.Write(Opcodes.pushVar);
                    code.Write(name);
                }
                else if (a == "print")
                {
                    code.Write(Opcodes.print);
                }
                else if (a == "printLine")
                {
                    code.Write(Opcodes.printLine);
                }
                else if (a == "read")
                {
                    code.Write(Opcodes.read);
                }
                else if (a == "readLine")
                {
                    code.Write(Opcodes.readLine);
                }
                else if (a == "halt")
                {
                    code.Write(Opcodes.halt);
                }
                else if (a == "inputInt32")
                {
                    code.Write(Opcodes.inputInt32);
                }
                else if (a == "inputString")
                {
                    code.Write(Opcodes.inputString);
                }
                else if (a == "mine")
                {
                    code.Write(Opcodes.bc_mine);
                }
                else if (a == "createBlockchain")
                {
                    code.Write(Opcodes.bc_createBlockchain);
                }
                else if (a == "createTransaction")
                {
                    code.Write(Opcodes.bc_createTransaction);
                }
                else if (a == "pop")
                {
                    code.Write(Opcodes.pop);
                }
                else if (a == "popa")
                {
                    code.Write(Opcodes.popa);
                }
                else if (a.StartsWith("decVar "))
                {
                    string name = a.Substring(7);
                    code.Write(Opcodes.decVar);
                    code.Write(name);
                }
                else if (a.StartsWith("setVar "))
                {
                    string name = a.Substring(7);
                    code.Write(Opcodes.setVar);
                    code.Write(name);
                }
                else if (a == "add")
                {
                    code.Write(Opcodes.add);
                }
                else if (a == "sub")
                {
                    code.Write(Opcodes.sub);
                }
                else if (a == "mul")
                {
                    code.Write(Opcodes.mul);
                }
                else if (a == "div")
                {
                    code.Write(Opcodes.div);
                }
                else if (a == "clear")
                {
                    code.Write(Opcodes.clear);
                }
                else if (a == "ife")
                {
                    if (currentBlock != null)
                    {
                        blockstack.Push(currentBlock);
                    }

                    currentBlock = new IfBlock(blockNumber);
                    code.Write(Opcodes.ife);
                    code.Write(blockNumber);
                    blockNumber++;
                }
                else if (a == "ifn")
                {
                    if (currentBlock != null)
                    {
                        blockstack.Push(currentBlock);
                    }

                    currentBlock = new IfBlock(blockNumber);
                    code.Write(Opcodes.ifn);
                    code.Write(blockNumber);
                    blockNumber++;
                }
                else if (a == "elseife")
                {
                    if (currentBlock != null)
                    {
                        blockstack.Push(currentBlock);
                    }

                    currentBlock = new ElseIfBlock(blockNumber);
                    code.Write(Opcodes.elseife);
                    code.Write(blockNumber);
                    blockNumber++;
                }
                else if (a == "elseifn")
                {
                    if (currentBlock != null)
                    {
                        blockstack.Push(currentBlock);
                    }

                    currentBlock = new ElseIfBlock(blockNumber);
                    code.Write(Opcodes.elseifn);
                    code.Write(blockNumber);
                    blockNumber++;
                }
                else if (a == "else")
                {
                    if (currentBlock != null)
                    {
                        blockstack.Push(currentBlock);
                    }

                    currentBlock = new ElseBlock(blockNumber);
                    code.Write(Opcodes.els);
                    code.Write(blockNumber);
                    blockNumber++;
                }
                else if (a == "endif")
                {
                    code.Write(Opcodes.endif);
                    currentBlock.endBlock = code.buffer.Count();
                    blocks.Add(currentBlock);

                    if (blockstack.Count > 0)
                    {
                        currentBlock = blockstack.Pop();
                    }
                    else
                    {
                        currentBlock = null;
                    }
                }
                else if (a.StartsWith("call "))
                {
                    string name = a.Substring(5);
                    code.Write(Opcodes.call);
                    code.Write(name);
                }
                else if (a.StartsWith("goto "))
                {
                    string name = a.Substring(5);
                    code.Write(Opcodes.got);
                    code.Write(name);
                }
                else if (a == "ret")
                {
                    code.Write(Opcodes.ret);
                }
            }

            code.Write(Opcodes.ret);
            funcs.Add(currentFunc);
        }
        #endregion
    }
}
