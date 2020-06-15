using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static AlmostBinary_Binarify.BinaryConverter;

namespace AlmostBinary_Binarify.Tests
{
    [TestClass]
    public class BinaryConversion
    {
        [TestMethod]
        public void ConvertOneStrToBinary_OriginalStr_BinaryStr()
        {
            Binary b = "helloworld".StrToBinary();
            Assert.AreEqual("01101000011001010110110001101100011011110111011101101111011100100110110001100100", b.BinaryString);
        }

        [TestMethod]
        public void ConvertStrArrToBinary_OriginalStrArr_BinaryStrArr()
        {
            List<Binary> binaries = ConvertAllToBinary(new string[] { "helloworld", "123", "צהü@^.,;'#´\"" });
            Assert.AreEqual(3, binaries.Count);
            Assert.AreEqual("01101000011001010110110001101100011011110111011101101111011100100110110001100100", binaries.ElementAt(0).BinaryString);
            Assert.AreEqual("001100010011001000110011", binaries.ElementAt(1).BinaryString);
            Assert.AreEqual("11000011101101101100001110100100110000111011110001000000010111100010111000101100001110110010011100100011110000101011010000100010", binaries.ElementAt(2).BinaryString);
        }

        [TestMethod]
        public void ConvertOneBinaryStrToStr_BinaryStr_OriginalStr()
        {
            Assert.AreEqual("helloworld", "01101000011001010110110001101100011011110111011101101111011100100110110001100100".BinaryStrToStr());
        }

        [TestMethod]
        public void ConvertBinaryStrArrToStrArr_BinaryStrArr_OriginalStrArr()
        {
            List<Binary> binaries = ConvertAllToString(new string[] { 
                "01101000011001010110110001101100011011110111011101101111011100100110110001100100",
                "001100010011001000110011",
                "11000011101101101100001110100100110000111011110001000000010111100010111000101100001110110010011100100011110000101011010000100010"
            });
            Assert.AreEqual(3, binaries.Count);
            Assert.AreEqual("helloworld", binaries.ElementAt(0));
            Assert.AreEqual("123", binaries.ElementAt(1));
            Assert.AreEqual("צהü@^.,;'#´\"", binaries.ElementAt(2));
        }
    }
}
