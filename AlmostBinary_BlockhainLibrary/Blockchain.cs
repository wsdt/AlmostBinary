using System;
using System.Collections.Generic;
using System.Text;

namespace AlmostBinary_BlockhainLibrary
{
    public class Blockchain
    {
        #region properties
        public IList<Block> Chain { set; get; }
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
            return new Block(DateTime.Now, null, "{}");
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
            block.Hash = block.CalculateHash();
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
    }
    #endregion
}
