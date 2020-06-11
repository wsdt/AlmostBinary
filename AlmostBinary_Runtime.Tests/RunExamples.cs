using AlmostBinary_GlobalConstants.Tests;
using AlmostBinary_Runtime.Tests.utils;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace AlmostBinary_Runtime.Tests
{
    public class RunExamples
    {
        [Fact]
        public void RunRepeat() => TestHelper.Run(IGlobalTestConstants.REPEAT);

        [Fact]
        public void RunCall() => TestHelper.Run(IGlobalTestConstants.CALL);

        [Fact]
        public void RunHelloWorld() => TestHelper.Run(IGlobalTestConstants.HELLO_WORLD);

        [Fact]
        public void RunIf() => TestHelper.Run(IGlobalTestConstants.IF);

        [Fact]
        public void RunInput() => TestHelper.Run(IGlobalTestConstants.INPUT);

        [Fact]
        public void RunVariable() => TestHelper.Run(IGlobalTestConstants.VARIABLE);
    }
}
