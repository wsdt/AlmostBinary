using AlmostBinary_BlockhainLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;

namespace AlmostBinary_BlockchainLibrary.Tests
{
    [TestClass]
    public class BlockchainTest
    {
        [TestMethod]
        public void CreateBlockchain()
        {
            Blockchain abinCoin = new Blockchain();
            abinCoin.CreateTransaction(new Transaction("WSDT", "FooBar", 10));
            abinCoin.ProcessPendingTransactions("MiningUser");
            Console.WriteLine(JsonConvert.SerializeObject(abinCoin, Formatting.Indented));

            Assert.AreEqual(1, abinCoin.PendingTransactions.Count);
            Assert.IsNotNull(abinCoin.GetLatestBlock().PreviousHash);
            Assert.AreEqual(1, abinCoin.GetLatestBlock().Index);
            Assert.IsTrue(abinCoin.IsValid());
        }
    }
}
