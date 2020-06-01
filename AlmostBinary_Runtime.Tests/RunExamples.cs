using AlmostBinary_HelperMethods.Tests;
using AlmostBinary_GlobalConstants.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlmostBinary_Runtime.Tests
{
    [TestClass]
    public class RunExamples
    {
        [TestMethod]
        public void RunRepeat() => RuntimeTestHelper.Run(IGlobalTestConstants.REPEAT);

        [TestMethod]
        public void RunCall() => RuntimeTestHelper.Run(IGlobalTestConstants.CALL);

        [TestMethod]
        public void RunHelloWorld() => RuntimeTestHelper.Run(IGlobalTestConstants.HELLO_WORLD);

        [TestMethod]
        public void RunIf() => RuntimeTestHelper.Run(IGlobalTestConstants.IF);

        [TestMethod]
        public void RunInput() => RuntimeTestHelper.Run(IGlobalTestConstants.INPUT);

        [TestMethod]
        public void RunVariable() => RuntimeTestHelper.Run(IGlobalTestConstants.VARIABLE);

    }
}
