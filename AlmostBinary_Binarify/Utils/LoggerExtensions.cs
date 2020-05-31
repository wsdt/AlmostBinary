using Serilog;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace AlmostBinary_Binarify.Utils
{
    public static class LoggerExtensions
    {
        public static ILogger Here(this ILogger logger,
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int sourceLineNumber = 0)
                => logger
                    .ForContext("MemberName", memberName)
                    .ForContext("LineNumber", sourceLineNumber);
    }
}
