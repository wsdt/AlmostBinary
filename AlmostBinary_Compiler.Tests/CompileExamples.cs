using AlmostBinary_GlobalConstants.Tests;
using System.IO;
using AlmostBinary_Runtime.Tests;
using System.Threading.Tasks;
using Xunit;
using AlmostBinary_Compiler.Tests.Utils;

namespace AlmostBinary_Compiler.Tests
{
    public class CompileExamples
    {
        [Fact]
        public void CompileRepeat() => TestHelper.Compile(IGlobalTestConstants.REPEAT);

        [Fact]
        public void CompileCall() => TestHelper.Compile(IGlobalTestConstants.CALL);

        [Fact]
        public void CompileHelloWorld() => TestHelper.Compile(IGlobalTestConstants.HELLO_WORLD);

        [Fact]
        public void CompileIf() => TestHelper.Compile(IGlobalTestConstants.IF);

        [Fact]
        public void CompileInput() => TestHelper.Compile(IGlobalTestConstants.INPUT);

        [Fact]
        public void CompileVariable() => TestHelper.Compile(IGlobalTestConstants.VARIABLE);
    }
}
