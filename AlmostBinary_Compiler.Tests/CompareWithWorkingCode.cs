//using AlmostBinary_HelperMethods.Tests;
//using AlmostBinary_GlobalConstants.Tests;
//using AlmostBinary_Compiler.Tests;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.IO;
//using AlmostBinary_Runtime.Tests;
//using System.Text.RegularExpressions;
//using System;
//using System.Threading.Tasks;

//namespace AlmostBinary_Compiler.Tests
//{
//    [TestClass]
//    public class CompareWithWorkingCode
//    {
//        private static RegexOptions regOptions = RegexOptions.None;
//        private static Regex multipleSpacesRegex = new Regex("[ ]{2,}", regOptions);

//        /// <summary>
//        /// Bug #4, try to call directly for better testing experience (e.g. exceptions when file not found, etc.)
//        /// </summary>
//        /// <param name="fileName"></param>
//        public static string Compile(string uncompiledCode) => AlmostBinary_Compiler.Program.Compile(new string[] { "--inline-code", uncompiledCode });

//        public static string TrimReplaceAll(string code)
//        {
//            return multipleSpacesRegex.Replace(code.Replace("\n", " ").Replace("\r", " "), " ");
//        }

//        [TestMethod]
//        public void CompareRepeat()
//        {
//            Assert.AreEqual(
//                TrimReplaceAll(Compile(IUncompiledFileConstants.REPEAT)), TrimReplaceAll(ICompiledFileConstants.REPEAT)
//            );
//        }

//        [TestMethod]
//        public void CompareCall()
//        {
//            Assert.AreEqual(
//                 TrimReplaceAll(Compile(IUncompiledFileConstants.CALL)), TrimReplaceAll(ICompiledFileConstants.CALL)
//             );
//        }

//        [TestMethod]
//        public void CompareHelloWorld()
//        {
//            Assert.AreEqual(
//                 TrimReplaceAll(Compile(IUncompiledFileConstants.HELLO_WORLD)), TrimReplaceAll(ICompiledFileConstants.HELLO_WORLD)
//             );
//        }

//        [TestMethod]
//        public void CompareIf()
//        {
//            Assert.AreEqual(
//                 TrimReplaceAll(Compile(IUncompiledFileConstants.IF)), TrimReplaceAll(ICompiledFileConstants.IF)
//             );
//        }

//        [TestMethod]
//        public void CompareInput()
//        {
//            Assert.AreEqual(
//                 TrimReplaceAll(Compile(IUncompiledFileConstants.INPUT)), TrimReplaceAll(ICompiledFileConstants.INPUT)
//             );
//        }

//        [TestMethod]
//        public void CompareVariable()
//        {
//            Assert.AreEqual(
//                 TrimReplaceAll(Compile(IUncompiledFileConstants.VARIABLE)), TrimReplaceAll(ICompiledFileConstants.VARIABLE)
//             );
//        }
//    }
//}
