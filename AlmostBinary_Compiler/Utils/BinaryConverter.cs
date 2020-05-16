using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using AlmostBinary_Compiler.utils;

namespace AlmostBinary_Compiler.Utils
{
    public static class BinaryConverter
    {
        #region fields
        /// <summary>
        /// Using Binary as context as static classes cannot be used as type and extension method needs to be defined within a static class.
        /// </summary>
        private static ILogger Log => Serilog.Log.ForContext<Binary>();
        #endregion

        /// <summary>
        /// Converts a string to a binary-string.
        /// </summary>
        /// <param name="data">Non-binary string</param>
        /// <returns>Binary object containing a binary string</returns>
        public static Binary ToBinary(this string data)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in data.ToCharArray())
            {
                sb.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
            }

            Binary b = new Binary(binaryString: sb.ToString());
            Log.Here().Information($"Converted string '{data}' to binary-string '{b.BinaryString}'");
            return b;
        }

        /// <summary>
        /// Container class for binary strings.
        /// </summary>
        public sealed class Binary
        {
            #region fields
            private static ILogger Log => Serilog.Log.ForContext<Binary>();
            private string _binaryString;
            private string _originalString;
            #endregion

            #region properties
            public string BinaryString { get => _binaryString; set => _binaryString = value; }
            public string OriginalString { get => _originalString; set => _originalString = value; }
            #endregion

            #region ctor
            /// <summary>
            /// Not both params need to be set. If you do, ensure that binary conversion is correct.
            /// </summary>
            /// <param name="binaryString">Original string converted to binary</param>
            /// <param name="originalString">Non-binary string</param>
            public Binary(string binaryString = "", string originalString = "")
            {
                this.BinaryString = binaryString;
                this.OriginalString = originalString;
            }
            #endregion

            #region methods
            /// <summary>
            /// Converts Binary back to original string.
            /// </summary>
            /// <returns>Non-binary string</returns>
            public override string ToString()
            {
                if (String.IsNullOrWhiteSpace(_originalString))
                {
                    List<Byte> byteList = new List<Byte>();

                    for (int i = 0; i< _binaryString.Length; i+= 8)
                    {
                        byteList.Add(Convert.ToByte(_binaryString.Substring(i, 8), 2));
                    }
                    _originalString = Encoding.ASCII.GetString(byteList.ToArray());
                }
                Log.Here().Information($"Converted binary string back to original string: '{_binaryString}' -> '{_originalString}'");
                
                return _originalString;
            }

            public static implicit operator string(Binary input) => input.ToString();
            #endregion
        }
    }
}
