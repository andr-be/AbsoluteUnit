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
        OutputFlags OutputFlags { get; init; } = new(commandGroup.Flags, debug);

        /// <summary>
        /// Provides a fully formatted output string for the Calculator's commands and arguments.
        /// </summary>
        /// <summary>
        /// Provides a fully formatted output string for the Calculator's commands and arguments.
        /// </summary>
        public string FormatOutput()
        {
            var outputComponents = new List<string>();

            if (OutputFlags.Debug)
            {
                outputComponents.Add(CommandGroup.ToString());
                outputComponents.Add(Calculator.Command?.ToString() ?? "");
            }

            var resultLine = ResultsString(Calculator.CommandGroup, Result);
            if (OutputFlags.Verbose)
            {
                resultLine += " " + ConversionFactorString(Calculator.Command!, Result) + ")";
            }
            outputComponents.Add(resultLine);

            return string.Join('\n', outputComponents) + '\n';
        }

        /// <summary>
        /// Calculate the number of decimal places quantity should be represented with using logarithmic approach
        /// </summary>
        /// <param name="quantity">The quantity to calculate precision for</param>
        /// <returns>Number of decimal places to display</returns>
        static int CalculateAutoPrecision(double quantity)
        {
            if (quantity is 0 
                or double.NaN 
                or double.PositiveInfinity 
                or double.NegativeInfinity) return 0;

            var logQuantity = Math.Log10(Math.Abs(quantity));
            var magnitude = Math.Floor(logQuantity);

            return magnitude switch
            {
                >= 3 => 0,
                < -1 => 4,
                _ => 3 - (int)magnitude,
            };
        }

        /// <summary>
        /// Works out the amount you multiply the original input by to return the result
        /// </summary>
        /// <param name="command"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        static string ConversionFactorString(ICommand command, Measurement result)
        {
            var originalQuantity = command.Input.Quantity;

            var newQuantity = result.Quantity;

            var factor = (command is not Commands.Convert convert)
                ? newQuantity / originalQuantity
                : convert.ConversionFactor;

            var originalQuantityString = PreConversionQuantity(command);

            return $"({originalQuantityString} x{factor}";
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
        static string PreConversionQuantity(ICommand command)
        {
            var originalQuantity = command.Input.Quantity * Math.Pow(10, command.Input.Exponent);
            var autoPrecision = CalculateAutoPrecision(originalQuantity);
            return $"{RoundedQuantityString(originalQuantity, autoPrecision)}";
        }
        
        /// <summary>
        /// generates a formatted, rounded result to return to the user
        /// </summary>
        /// <param name="commandGroup"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        static string ResultsString(CommandGroup commandGroup, Measurement result)
        {
            bool inStandardForm = commandGroup.Flags
                .TryGetValue(Flag.StandardForm, out var _);

            (result.Quantity, result.Exponent) = StandardForm.RepresentInStandardForm
            (
                result.Quantity * Math.Pow(10, result.Exponent), 
                new(inStandardForm, StandardForm.Style.Scientific)
            );
                
            bool precisionProvided = commandGroup.Flags
                .TryGetValue(Flag.DecimalPlaces, out var decimalPrecision);

            if (!precisionProvided)
                decimalPrecision = CalculateAutoPrecision(result.Quantity);

            string resultString = RoundedQuantityString(result.Quantity, decimalPrecision);

            return $"Result:\t\t{resultString + ExponentString(result)} {UnitString(result)}";
        }

        /// <summary>
        /// Weird StackOverflow decimal rounding hack for string format; don't think too much about it
        /// </summary>
        /// <param name="decimalPrecision">the decimal precision (???)</param>
        /// <returns>the formatted </returns>
        static string RoundedQuantityString(double rawQuantity, int decimalPrecision)
        {
            try
            {
                var rounded = string.Format(new NumberFormatInfo() { NumberDecimalDigits = decimalPrecision }, "{0:F}", new decimal(rawQuantity));
                return rounded;
            }
            catch (Exception e)
            {

                throw new CommandError(ErrorCode.InvalidNumber, $"result outside of calculation bounds: {rawQuantity}", e);
            }
        }

        /// <summary>
        /// generates a string representation of the units in Result.
        /// </summary>
        /// <param name="units"></param>
        /// <returns></returns>
        static string UnitString(Measurement result) => string.Join(".", result.Units);
        
    }

    internal record OutputFlags(Dictionary<Flag, int> Flags, bool Debug)
    {
        public bool Debug { get; init; } = Debug;
        public bool Verbose { get; init; } = Flags.ContainsKey(Flag.VerboseCalculation);
        public bool StandardForm { get; init; } = Flags.ContainsKey(Flag.StandardForm);
        public bool Engineering { get; init; } = Flags.ContainsKey(Flag.Engineering);
        public (bool enabled, int value) SignificantFigures { get; init; } = (
            Flags.ContainsKey(Flag.SignificantFigures),
            Flags.GetValueOrDefault(Flag.SignificantFigures)
        );
        public (bool enabled, int value) DecimalPlaces { get; init; } = (
            Flags.ContainsKey(Flag.DecimalPlaces),
            Flags.GetValueOrDefault(Flag.DecimalPlaces)
        );
    }
}
