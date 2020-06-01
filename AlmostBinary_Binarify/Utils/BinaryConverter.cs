using AlmostBinary_Binarify.utils;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlmostBinary_Binarify
{
    public static class BinaryConverter
    {
        #region fields
        /// <summary>
        /// Using Binary as context as static classes cannot be used as type and extension method needs to be defined within a static class.
        /// </summary>
        private static ILogger Log => Serilog.Log.ForContext<Binary>();
        private const string DOUBLE_QUOTES_BINARY = "00100010";
        private const string ESCAPE_CHAR_DOUBLE_QUOTES = "0000000000000000";
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

            Binary b = new Binary(binaryString: EscapeBinary(data, sb.ToString()));
            Log.Here().Debug($"Converted string '{data}' to binary-string '{b.BinaryString}'");
            return b;
        }

        private static string EscapeBinary(string originalStr, string unEscapedBinaryStr)
        {
            string escapedBinary = unEscapedBinaryStr;

            if (!originalStr.Contains("\""))
            {
                Log.Here().Verbose($"Escaping binary for '\"' -> {unEscapedBinaryStr}");
                escapedBinary = unEscapedBinaryStr.Replace(DOUBLE_QUOTES_BINARY, ESCAPE_CHAR_DOUBLE_QUOTES);
            }

            return escapedBinary;
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
                    // Unescape previously removed characters/key-words (e.g. double quotes) which can occur as part of a string or similar.
                    string unEscapedBinary = _binaryString.Replace(ESCAPE_CHAR_DOUBLE_QUOTES, DOUBLE_QUOTES_BINARY);
                    try
                    {
                        List<Byte> byteList = new List<Byte>();

                        for (int i = 0; i < unEscapedBinary.Length; i += 8)
                        {
                            byteList.Add(Convert.ToByte(unEscapedBinary.Substring(i, 8), 2));
                        }
                        _originalString = Encoding.ASCII.GetString(byteList.ToArray());

                    }
                    catch (Exception ex)
                    {
                        Log.Here().Error(ex, $"Provided binary doesn't seem to be valid -> '{unEscapedBinary}'");
                        throw;
                    }
                    Log.Here().Debug($"Converted binary string back to original string: '{unEscapedBinary}' -> '{_originalString}'");
                }

                return _originalString;
            }

            public static implicit operator string(Binary input) => input.ToString();
            #endregion
        }
    }
}
