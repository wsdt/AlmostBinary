using System;
using System.Security.Cryptography;
using System.Text;

namespace AlmostBinary_BlockhainLibrary
{
    public class Block
    {
        #region properties
        public int Index { get; set; }
        public DateTime TimeStamp { get; set; }
        public string PreviousHash { get; set; }
        public string Hash { get; set; }
        public string Data { get; set; }
        #endregion

        #region ctor
        public Block(DateTime timeStamp, string previousHash, string data)
        {
            this.Index = 0;
            this.TimeStamp = timeStamp;
            this.PreviousHash = previousHash;
            this.Data = data;
            this.Hash = this.CalculateHash();
        }
        #endregion

        #region methods
        public string CalculateHash()
        {
            SHA512 sha512 = SHA512.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes($"{this.TimeStamp}-{this.PreviousHash == ""}-{this.Data}");
            byte[] outputBytes = sha512.ComputeHash(inputBytes);
            return Convert.ToBase64String(outputBytes);
        }
        #endregion
    }
}
