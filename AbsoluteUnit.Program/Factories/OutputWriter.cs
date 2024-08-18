using AbsoluteUnit.Program.Commands;
using AbsoluteUnit.Program.Structures;
using System.Globalization;

namespace AbsoluteUnit.Program.Factories
{
    public class OutputWriter(CommandGroup commandGroup, Measurement result, Calculator runner, bool debug = false)
    {
        /// This class will control all of the CLI's output such that I can format it with Command Arguments!
        CommandGroup CommandGroup { get; init; } = commandGroup;
        Measurement Result { get; init; } = result;
        Calculator Runner { get; init; } = runner;

        bool DebugOutput { get; init; } = debug;
        bool VerboseOutput { get; init; } = commandGroup.Flags.ContainsKey(Flag.VerboseCalculation);

        public string FormatOutput()
        {
            var resultString = DebugOutput?
                CommandGroup + "\n" 
                : "";

            resultString += Runner.Command + "\n";

            resultString += FormatResult();

            if (VerboseOutput) resultString += Runner.Command switch
            {
                Commands.Convert c => GetVerboseConversionFactor(c),
            //  Commands.Express e => GetVerboseExpressionFactor(e),
            //  Commands.Simplify s => GetVerboseSimplificationFactor(s),
                _ => "",
            };

            return resultString + "\n";
        }


        private string FormatResult()
        {
            bool precisionProvided = Runner.CommandGroup.Flags
                .TryGetValue(Flag.DecimalPlaces, out var decimalPrecision);
            
            if (!precisionProvided) 
                decimalPrecision = GetAutoPrecision(Result.Quantity);
            
            string resultString = RoundedQuantity(decimalPrecision);

            return $"Result:\t\t{resultString + GetResultExponent()} {ConcatenateUnits()}";
        }

        private string ConcatenateUnits() => string.Join(".", Result.Units);

        /// <summary>
        /// Weird stackoverflow decimal rounding hack for string format; don't think too much about it
        /// </summary>
        /// <param name="decimalPrecision">the decimal precision (???)</param>
        /// <returns>the formatted </returns>
        private string RoundedQuantity(int decimalPrecision) =>
            string.Format(new NumberFormatInfo() { NumberDecimalDigits = decimalPrecision }, "{0:F}", new decimal(Result.Quantity));

        private static int GetAutoPrecision(double quantity) => quantity switch
        {
            < 1000 and >= 10 => 1,
            < 10 and >= 1 => 2,
            < 1 and >= 0.1 => 3,
            < 0.1 => 4,
            _ => 0,
        };

        private string GetResultExponent() => (Result.Exponent != 0) ? $"e{Result.Exponent}" : "";
        private string GetVerboseConversionFactor(Commands.Convert convert) => 
            $" ({Result.Quantity * Math.Pow(10, Result.Exponent) / convert.ConversionFactor} x{convert.ConversionFactor})";
    }
}
