using AlmostBinary_Binarify.utils;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

        // TODO: To improve performance (seems to be the fastest way -> https://stackoverflow.com/questions/2036718/fastest-way-of-reading-and-writing-binary)
        public static Binary ToBinary_bak(this string data)
        {
            if (!BitConverter.IsLittleEndian)
            {
                throw new NotSupportedException("Only LittleEndian is supported by Binarify (for performance reasons).");
            }
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            unsafe
            {
                fixed (byte* p = buffer)
                {
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        *((int*)(p + i)) = buffer[i];
                    }
                }
            }

            return new Binary(binaryString: EscapeBinary(data, Encoding.UTF8.GetString(buffer)));
        }

        /// <summary>
        /// Converts a string to a binary-string.
        /// </summary>
        /// <param name="data">Non-binary string</param>
        /// <returns>Binary object containing a binary string</returns>
        public static Binary ToBinary(this string data)
        {
            StringBuilder sb = new StringBuilder();

            foreach (byte x in Encoding.UTF8.GetBytes(data))
            {
                sb.Append(Convert.ToString(x, 2).PadLeft(8, '0'));
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


        /// <summary>Converts all provided arguments to binary (except the first which is reserved for configuration).</summary>
        public static List<Binary> ConvertAllToBinary(string[] args)
        {
            List<Binary> binaries = new List<Binary>();
            Log.Here().Verbose($"Converting {args.Length} values to Binary.");

            for (int i = 0; i < args.Length; i++)
            {
                Binary b = args[i].ToBinary();
                Log.Here().Information($"Argument {i}: '{args[i]}' -> '{b.BinaryString}'");
                binaries.Add(b);
            }
            return binaries;
        }

        /// <summary>Converts all provided arguments back to the orginal string (except the first which is reserved for configuration).</summary>
        public static List<Binary> ConvertAllToString(string[] args)
        {
            List<Binary> binaries = new List<Binary>();
            Log.Here().Verbose($"Converting {args.Length} values to Strings");
            for (int i = 0; i < args.Length; i++)
            {
                Binary b = new Binary() { BinaryString = args[i] };
                Log.Here().Information($"Argument {i}: '{args[i]}' -> {b}");
                binaries.Add(b);
            }
            return binaries;
        }

        /// <summary>
        /// Container class for binary strings.
        /// </summary>
        public sealed class Binary
        {
            #region fields
            private static ILogger Log => Serilog.Log.ForContext<Binary>();
            #endregion

            #region properties
            public string BinaryString { get; set; }
            public string OriginalString { get; set; }
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
            /// Static method to remove the necessity to create a new instance.
            /// </summary>
            /// <param name="binaryStr">Value encoded in binary</param>
            /// <returns></returns>
            public static string BinaryToStr(string binaryStr)
            {
                string originalStr;
                // Unescape previously removed characters/key-words (e.g. double quotes) which can occur as part of a string or similar.
                string unEscapedBinary = binaryStr.Replace(ESCAPE_CHAR_DOUBLE_QUOTES, DOUBLE_QUOTES_BINARY);
                try
                {
                    List<Byte> byteList = new List<Byte>();

                    for (int i = 0; i < unEscapedBinary.Length; i += 8)
                    {
                        byteList.Add(Convert.ToByte(unEscapedBinary.Substring(i, 8), 2));
                    }
                    originalStr = Encoding.UTF8.GetString(byteList.ToArray());

                }
                catch (Exception ex)
                {
                    Log.Here().Error(ex, $"Provided binary doesn't seem to be valid -> '{unEscapedBinary}'");
                    throw;
                }
                Log.Here().Debug($"Converted binary string back to original string: '{unEscapedBinary}' -> '{originalStr}'");

                return originalStr;
            }

            /// <summary>
            /// Converts Binary back to original string.
            /// </summary>
            /// <returns>Non-binary string</returns>
            public override string ToString()
            {
                if (String.IsNullOrWhiteSpace(OriginalString))
                {
                    OriginalString = BinaryToStr(BinaryString);
                }
                return OriginalString;
            }

            public static implicit operator string(Binary input) => input.ToString();
            #endregion
        }
    }
}
