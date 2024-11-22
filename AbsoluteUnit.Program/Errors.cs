using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AbsoluteUnit.Program
{
    public enum ErrorCode
    {
        [Display(Name = "ERROR 100")]
        InvalidCommand,

        [Display(Name = "ERROR 200")]
        UnrecognisedUnit,

        [Display(Name = "ERROR 300")]
        InvalidMeasurement,

        [Display(Name = "ERROR 400")]
        InvalidNumber,

        [Display(Name = "ERROR 500")]
        InvalidConversion,

        [Display(Name = "ERROR 600")]
        UnrecognisedFlag,

        [Display(Name = "ERROR 700")]
        BadFlagArgument,

        [Display(Name = "ERROR 800")]
        BadArgumentCount,

        [Display(Name = "ERROR 900")]
        ParseError,

        [Display(Name = "ERROR 1000")]
        InvalidUnitString,
    }

    public static class ErrorCodeExtensions
    {
        public static string ToDisplayString(this ErrorCode code)
        {
            var field = code.GetType().GetField(code.ToString());
            var attribute = field?.GetCustomAttribute<DisplayAttribute>();
            return attribute?.Name ?? code.ToString();
        }
    }


    public class CommandError(ErrorCode code, string message, Exception? inner = null) 
        : Exception(message)
    {
        public ErrorCode Code { get; } = code;
    }
}