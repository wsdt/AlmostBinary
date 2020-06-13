using System;
using System.Collections.Generic;
using System.Text;
using static AlmostBinary_Binarify.BinaryConverter;

namespace AlmostBinary_Binarify
{
    /// <summary>
    /// To avoid multiple allocations and better overall performance
    /// </summary>
    public interface ITokens
    {
        public interface IPartial
        {
            public static readonly Binary INT_LITERAL = new Binary(originalString: "~");
            public static readonly Binary STRING_LITERAL = new Binary(originalString: "\"");
        }

        public static readonly Binary[] ESCAPABLE_KEYWORDS = new Binary[] { 
            IMPORT, FUNCTION, IF, ELSEIF, ELSE, REPEAT, RETURN, OP_ADD, OP_SUB, OP_MUL, OP_DIV, OP_DOUBLE_EQUAL,
            OP_NOT_EQUAL, OP_EQUAL, LEFT_PARAN, RIGHT_PARAN, LEFT_BRACE, RIGHT_BRACE, COMMA, PERIOD, IPartial.INT_LITERAL,
            IPartial.STRING_LITERAL
        };

        public static readonly Binary IMPORT = new Binary(originalString: "import");
        public static readonly Binary FUNCTION = new Binary(originalString: "function");
        public static readonly Binary IF = new Binary(originalString: "if");
        public static readonly Binary ELSEIF = new Binary(originalString: "elseif");
        public static readonly Binary ELSE = new Binary(originalString: "else");
        public static readonly Binary REPEAT = new Binary(originalString: "repeat");
        public static readonly Binary RETURN = new Binary(originalString: "return");
        public const string WHITESPACE_REGEX = "[ \\t]+";
        public const string NEWLINE_REGEX = "\\n";
        public static readonly Binary OP_ADD = new Binary(originalString: "+");
        public static readonly Binary OP_SUB = new Binary(originalString: "-");
        public static readonly Binary OP_MUL = new Binary(originalString: "*");
        public static readonly Binary OP_DIV = new Binary(originalString: "/");
        public static readonly Binary OP_DOUBLE_EQUAL = new Binary(originalString: "==");
        public static readonly Binary OP_NOT_EQUAL = new Binary(originalString: "!=");
        public static readonly Binary OP_EQUAL = new Binary(originalString: "=");
        public static readonly Binary LEFT_PARAN = new Binary(originalString: "(");
        public static readonly Binary RIGHT_PARAN = new Binary(originalString: ")");
        public static readonly Binary LEFT_BRACE = new Binary(originalString: "{");
        public static readonly Binary RIGHT_BRACE = new Binary(originalString: "}");
        public static readonly Binary COMMA = new Binary(originalString: ",");
        public static readonly Binary PERIOD = new Binary(originalString: ".");
        public static readonly string STRING_LITERAL_REGEX = $"{IPartial.STRING_LITERAL.BinaryString}.*?{IPartial.STRING_LITERAL.BinaryString}";
        public static readonly string INT_LITERAL_REGEX = $"{IPartial.INT_LITERAL.BinaryString}[0-1]*{IPartial.INT_LITERAL.BinaryString}";
        public const string IDENT_REGEX = "[0-1][0-1]*";
    }
}
