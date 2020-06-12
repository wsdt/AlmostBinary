using System;
using System.Collections.Generic;
using System.Text;

namespace AlmostBinary_BlockhainLibrary
{
    public class Transaction
    {
        #region properties
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public int Amount { get; set; }
        #endregion

        #region ctor
        public Transaction(string fromAddress, string toAddress, int amount)
        {
            this.FromAddress = fromAddress;
            this.ToAddress = toAddress;
            this.Amount = amount;
        }
        #endregion
    }
}
