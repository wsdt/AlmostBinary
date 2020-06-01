using Serilog;
using System.Runtime.CompilerServices;

namespace AlmostBinary_Compiler.utils
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
