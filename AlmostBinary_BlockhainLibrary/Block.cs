using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

namespace AlmostBinary_BlockhainLibrary
{
    public class Block
    {
        #region properties
        public int Index { get; set; }
        public DateTime TimeStamp { get; set; }
        public string PreviousHash { get; set; }
        public string Hash { get; set; }
        public IList<Transaction> Transactions { get; set; }
        public int Nonce { get; set; }
        #endregion

        #region ctor
        public Block(DateTime timeStamp, string previousHash, IList<Transaction> transactions)
        {
            this.Index = 0;
            this.TimeStamp = timeStamp;
            this.PreviousHash = previousHash;
            this.Transactions = transactions;
            this.Hash = this.CalculateHash();
        }
        #endregion

        #region methods
        public string CalculateHash()
        {
            SHA512 sha512 = SHA512.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes($"{this.TimeStamp}-{this.PreviousHash ?? ""}-{JsonConvert.SerializeObject(this.Transactions)}-{this.Nonce}");
            byte[] outputBytes = sha512.ComputeHash(inputBytes);
            return Convert.ToBase64String(outputBytes);
        }

        public void Mine(int difficulty)
        {
            var leadingZeros = new string('0', difficulty);
            while (this.Hash == null || this.Hash.Substring(0, difficulty) != leadingZeros)
            {
                this.Nonce++;
                this.Hash = this.CalculateHash();
            }
        }
        #endregion
    }
}
