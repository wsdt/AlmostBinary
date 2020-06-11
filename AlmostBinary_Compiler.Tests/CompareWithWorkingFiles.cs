using AlmostBinary_GlobalConstants.Tests;
using System.IO;
using System;
using System.Diagnostics;
using Xunit;
using System.Text.RegularExpressions;
using AlmostBinary_Compiler.Tests.Utils;

namespace AlmostBinary_Compiler.Tests
{
    public class CompareWithWorkingFiles
    {
        private async void Compare(string fileName)
        {
            await AlmostBinary_Compiler.Tests.Utils.TestHelper.Compile(fileName).ContinueWith((_) =>
            {
                fileName += $".{IGlobalTestConstants.COMPILED_FILE_TYPE}";
                Assert.Equal(
                    TestHelper.TrimReplaceAll(
                        TestHelper.ReadFile(Path.Combine(IGlobalTestConstants.COMPILED_PATH, fileName))),
                    TestHelper.TrimReplaceAll(
                        TestHelper.ReadFile(Path.Combine(IGlobalTestConstants.WORKING_PATH, fileName))));
            });

            //TestHelper.Compile(fileName, () => {
            //    fileName += $".{IGlobalTestConstants.COMPILED_FILE_TYPE}";
            //    Assert.Equal(
            //        TestHelper.TrimReplaceAll(
            //            TestHelper.ReadFile(Path.Combine(IGlobalTestConstants.COMPILED_PATH, fileName))),
            //        TestHelper.TrimReplaceAll(
            //            TestHelper.ReadFile(Path.Combine(IGlobalTestConstants.WORKING_PATH, fileName))));
            //});
        }

        [Fact]
        public void CompareRepeat() => Compare(IGlobalTestConstants.REPEAT);

        [Fact]
        public void CompareCall() => Compare(IGlobalTestConstants.CALL);

        [Fact]
        public void CompareHelloWorld() => Compare(IGlobalTestConstants.HELLO_WORLD);

        [Fact]
        public void CompareIf() => Compare(IGlobalTestConstants.IF);

        [Fact]
        public void CompareInput() => Compare(IGlobalTestConstants.INPUT);

        [Fact]
        public void CompareVariable() => Compare(IGlobalTestConstants.VARIABLE);
    }
}