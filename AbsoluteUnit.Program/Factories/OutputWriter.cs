using AbsoluteUnit.Program.Commands;
using AbsoluteUnit.Program.Parsers.ParserGroups;
using AbsoluteUnit.Program.Structures;
using System;
using System.Globalization;

namespace AbsoluteUnit.Program.Factories
{
    /// <summary>
    /// This class generates the CLI's output, formatting and rounding in line with the flags provided by the user.
    /// </summary>
    /// <param name="commandGroup"></param>
    /// <param name="result"></param>
    /// <param name="runner"></param>
    /// <param name="debug"></param>
    public class OutputWriter(CommandGroup commandGroup, Measurement result, Calculator runner, bool debug = false)
    {
        CommandGroup CommandGroup { get; init; } = commandGroup;
        Measurement Result { get; init; } = result;
        Calculator Calculator { get; init; } = runner;

        bool DebugOutput { get; init; } = debug;
        bool VerboseOutput { get; init; } = commandGroup.Flags.ContainsKey(Flag.VerboseCalculation);

        /// <summary>
        /// Provides a fully formatted output string for the Calculator's commands and arguments.
        /// </summary>
        /// <returns></returns>
        public string FormatOutput()
        {
            var resultString = DebugOutput?
                CommandGroup + "\n" 
                : "";

            resultString += Calculator.Command + "\n";

            resultString += ResultsString(Calculator.CommandGroup, Result);

            if (VerboseOutput) resultString += Calculator.Command switch
            {
                Commands.Convert convert => ConversionFactorString(convert, Result),
                Commands.Express express => ExpressionFactorString(express, Result),
                Commands.Simplify simplify => SimplificationFactorString(simplify, Result),
                _ => "",
            };

            return resultString + "\n";
        }

        /// <summary>
        /// calculate the number of decimal places quantity should be represented with
        /// </summary>
        /// <param name="quantity"></param>
        /// <returns></returns>
        static int CalculateAutoPrecision(double quantity) => quantity switch
        {
            < 1e+3 and >= 1e+2 => 1,
            < 1e+2 and >= 1e+1 => 2,
            < 1e+1 and >= 1e-1 => 3,
            < 1e-1 => 4,
            _ => 0,
        };

        /// <summary>
        /// generate the verbose output conversion factor string that is appended to the results
        /// </summary>
        /// <param name="convert"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        static string ConversionFactorString(Commands.Convert convert, Measurement result) => 
            $"\t({PreConversionQuantity(convert, result)} x{RoundedConversionFactor(convert)})";


        private string SimplificationFactorString(Simplify simplify, Measurement result)
        {
            throw new NotImplementedException("Need to implement simplification factor.");
        }

        private string ExpressionFactorString(Express express, Measurement result)
        {
            throw new NotImplementedException("Need to implement simplification factor.");
        }

        /// <summary>
        /// represents the exponent as either a formatted value or empty string
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        static string ExponentString(Measurement result) => 
            (result.Exponent != 0) ? $"e{result.Exponent}" : "";

        /// <summary>
        /// generates the quantity prior to conversion; the original value input
        /// </summary>
        /// <param name="convert"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        static string PreConversionQuantity(Commands.Convert convert, Measurement result) =>
            $"{result.Quantity * Math.Pow(10, result.Exponent) / convert.ConversionFactor}";
        
        /// <summary>
        /// generates a formatted, rounded result to return to the user
        /// </summary>
        /// <param name="commandGroup"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        static string ResultsString(CommandGroup commandGroup, Measurement result)
        {
            bool precisionProvided = commandGroup.Flags
                .TryGetValue(Flag.DecimalPlaces, out var decimalPrecision);

            if (!precisionProvided)
                decimalPrecision = CalculateAutoPrecision(result.Quantity);

            string resultString = RoundedQuantityString(result.Quantity, decimalPrecision);

            return $"Result:\t\t{resultString + ExponentString(result)} {UnitString(result)}";
        }

        /// <summary>
        /// uses the autoPrecision routine to calculate how many decimal places the conversion factor should have
        /// </summary>
        /// <param name="convert"></param>
        /// <returns></returns>
        static string RoundedConversionFactor(Commands.Convert convert) => 
            RoundedQuantityString(convert.ConversionFactor, CalculateAutoPrecision(convert.ConversionFactor) + 1);

        /// <summary>
        /// Weird StackOverflow decimal rounding hack for string format; don't think too much about it
        /// </summary>
        /// <param name="decimalPrecision">the decimal precision (???)</param>
        /// <returns>the formatted </returns>
        static string RoundedQuantityString(double rawQuantity, int decimalPrecision) =>
            string.Format(new NumberFormatInfo() { NumberDecimalDigits = decimalPrecision }, "{0:F}", new decimal(rawQuantity));

        /// <summary>
        /// generates a string representation of the units in Result.
        /// </summary>
        /// <param name="units"></param>
        /// <returns></returns>
        static string UnitString(Measurement result) => string.Join(".", result.Units);
        
    }
}
