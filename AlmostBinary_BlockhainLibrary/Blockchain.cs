using System;
using System.Collections.Generic;
using System.Text;

namespace AlmostBinary_BlockhainLibrary
{
    // TODO: Add p2p network -> https://www.c-sharpcorner.com/article/building-a-blockchain-in-net-core-p2p-network/
    public class Blockchain
    {
        #region properties
        public IList<Block> Chain { set; get; }
        public int Reward = 1;
        public int Difficulty { set; get; } = 2;
        public IList<Transaction> PendingTransactions = new List<Transaction>();
        #endregion

        #region ctor
        public Blockchain()
        {
            this.InitializeChain();
            this.AddGenesisBlock();
        }
        #endregion

        #region methods
        public void InitializeChain()
        {
            this.Chain = new List<Block>();
        }

        public Block CreateGenesisBlock()
        {
            return new Block(DateTime.Now, null, 
                new List<Transaction>() { new Transaction("0000000000000000", "0000000000000000", 0) });
        }

        public void AddGenesisBlock()
        {
            this.Chain.Add(this.CreateGenesisBlock());
        }

        public Block GetLatestBlock()
        {
            return this.Chain[this.Chain.Count - 1];
        }

        public void AddBlock(Block block)
        {
            Block latestBlock = this.GetLatestBlock();
            block.Index = latestBlock.Index + 1;
            block.PreviousHash = latestBlock.Hash;
            block.Mine(this.Difficulty);
            this.Chain.Add(block);
        }

        public bool IsValid()
        {
            for (int i = 1; i < this.Chain.Count; i++)
            {
                Block currentBlock = this.Chain[i];
                Block previousBlock = this.Chain[i - 1];

                if (currentBlock.Hash != currentBlock.CalculateHash()
                    || currentBlock.PreviousHash != previousBlock.Hash)
                {
                    return false;
                }
            }
            return true;
        }

        public void CreateTransaction(Transaction transaction) => PendingTransactions.Add(transaction);

        public void ProcessPendingTransactions(string minerAddress)
        {
            Block block = new Block(DateTime.Now, GetLatestBlock().Hash, this.PendingTransactions);
            AddBlock(block);
            PendingTransactions = new List<Transaction>();
            CreateTransaction(new Transaction(null, minerAddress, Reward));
        }
    }
    #endregion
}
