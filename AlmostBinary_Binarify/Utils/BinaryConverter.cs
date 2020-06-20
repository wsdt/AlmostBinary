using AlmostBinary_Binarify.utils;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using static AlmostBinary_Binarify.CommandLineOptions;

namespace AlmostBinary_Binarify
{
    public static class BinaryConverter
    {
        #region fields
        /// <summary>
        /// Using Binary as context as static classes cannot be used as type and extension method needs to be defined within a static class.
        /// </summary>
        private static ILogger Log => Serilog.Log.ForContext<Binary>();
        private static readonly RegexOptions regOptions = RegexOptions.None;
        private static readonly Regex multipleSpacesRegex = new Regex("[ ]{2,}", regOptions);
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

            return new Binary(binaryString: Encoding.UTF8.GetString(buffer));
        }

        private static string AddLeadingZerosAccordingToBitArch(string binaryStr, BitArch arch = DEFAULT_BIT_ARCH)
        {
            // Ensure that every string (even those longer than current architecture, e.g. 80 bits > x64) is padded left to arch -> 80 bit -> 128 bits although x64 arch.
            int countFragments = (binaryStr.Length / (int)arch); // needs to be an int
            binaryStr = binaryStr.PadLeft((int)arch + (countFragments * (int)arch), '0');
            return binaryStr;
        }

        /// <summary>
        /// Converts a string to a binary-string.
        /// </summary>
        /// <param name="data">Non-binary string</param>
        /// <returns>Binary object containing a binary string</returns>
        public static Binary StrToBinary(this string data, BitArch arch = DEFAULT_BIT_ARCH) => new Binary(binaryString: data.StrToBinaryStr(arch), bitArch: arch);

        public static string StrToBinaryStr(this string data, BitArch arch = DEFAULT_BIT_ARCH)
        {
            StringBuilder sb = new StringBuilder();

            foreach (byte x in Encoding.UTF8.GetBytes(data))
            {
                sb.Append(Convert.ToString(x, 2).PadLeft(8, '0'));
            }
            string binaryStr = sb.ToString();
            if (binaryStr.Length > (int)arch) Log.Here().Information($"Binary-string length exceeded word-size: '{binaryStr}' ({binaryStr.Length}), {arch}");
            binaryStr = AddLeadingZerosAccordingToBitArch(binaryStr, arch);

            Log.Here().Debug($"Converted string '{data}' to binary-string '{binaryStr}'");
            return binaryStr;
        }

        /// <summary>
        /// Static method to remove the necessity to create a new instance.
        /// </summary>
        /// <param name="binaryStr">Value encoded in binary</param>
        /// <returns></returns>
        public static string BinaryStrToStr(this string binaryStr, BitArch arch = BitArch.x64)
        {
            string originalStr;
            try
            {
                List<Byte> byteList = new List<Byte>();

                for (int i = 0; i < binaryStr.Length; i += 8)
                {
                    byteList.Add(Convert.ToByte(binaryStr.Substring(i, 8), 2));
                }
                originalStr = Encoding.UTF8.GetString(byteList.ToArray())
                    .Replace("\0", string.Empty); // remove null from string (caused by greater bit-Architectures (see commandLineArgs))
            }
            catch (Exception ex)
            {
                Log.Here().Error(ex, $"Provided binary doesn't seem to be valid -> '{binaryStr}'");
                throw;
            }
            Log.Here().Debug($"Converted binary string back to original string: '{binaryStr}' -> '{originalStr}'");
            return originalStr;
        }

        /// <summary>Converts all provided arguments to binary (except the first which is reserved for configuration).</summary>
        public static List<Binary> ConvertAllToBinary(string[] args, BitArch arch = DEFAULT_BIT_ARCH)
        {
            List<Binary> binaries = new List<Binary>();
            Log.Here().Verbose($"Converting {args.Length} values to Binary.");

            for (int i = 0; i < args.Length; i++)
            {
                Binary b = args[i].StrToBinary(arch);
                Log.Here().Information($"Argument {i}: '{args[i]}' -> '{b.BinaryString}'");
                binaries.Add(b);
            }
            return binaries;
        }

        /// <summary>Converts all provided arguments back to the orginal string (except the first which is reserved for configuration).</summary>
        public static List<Binary> ConvertAllToString(string[] args, BitArch arch = DEFAULT_BIT_ARCH)
        {
            List<Binary> binaries = new List<Binary>();
            Log.Here().Verbose($"Converting {args.Length} values to Strings");
            for (int i = 0; i < args.Length; i++)
            {
                Binary b = new Binary(binaryString: args[i], bitArch: arch);
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
            public string BinaryString { get; private set; }
            public string OriginalString { get; private set; }
            public BitArch BitArchitecture { get; private set; }
            #endregion

            #region ctor
            /// <summary>
            /// Not both params need to be set. If you do, ensure that binary conversion is correct.
            /// </summary>
            /// <param name="binaryString">Original string converted to binary</param>
            /// <param name="originalString">Non-binary string</param>
            public Binary(string binaryString = "", string originalString = "", BitArch bitArch = DEFAULT_BIT_ARCH)
            {
                bool isBinaryStrEmpty = String.IsNullOrWhiteSpace(binaryString);
                bool isOriginalStrEmpty = String.IsNullOrWhiteSpace(originalString);
                this.BitArchitecture = bitArch;

                // Is BinaryStr not provided but originalStr, then convert automatically, otherwise just empty str
                this.BinaryString = isBinaryStrEmpty && !isOriginalStrEmpty ? originalString.StrToBinaryStr(bitArch) : binaryString;
                // Is OriginalStr not provided but binaryStr, then convert automatically, otherwise just empty str
                this.OriginalString = isOriginalStrEmpty && !isBinaryStrEmpty ? BinaryStrToStr(binaryString, bitArch) : originalString;
                Log.Here().Verbose($"Loaded and generated binary object -> {JsonSerializer.Serialize(this)}");
            }
            #endregion

            #region methods
            /// <summary>
            /// Converts Binary back to original string.
            /// </summary>
            /// <returns>Non-binary string</returns>
            public override string ToString()
            {
                if (String.IsNullOrWhiteSpace(OriginalString))
                {
                    OriginalString = BinaryString.BinaryStrToStr(this.BitArchitecture);
                }
                return OriginalString;
            }

            public static implicit operator string(Binary input) => input.ToString();
            #endregion
        }
    }
}
