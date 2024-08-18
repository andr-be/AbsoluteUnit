using AbsoluteUnit.Program.Commands;
using AbsoluteUnit.Program.Structures;
using System.Diagnostics;

namespace AbsoluteUnit.Program.Factories
{
    public class CLIOutputFactory(CommandGroup commandGroup, Measurement result, Runner runner, bool debug=false)
    {
        /// This class will control all of the CLI's output such that I can format it with Command Arguments!
        CommandGroup CommandGroup { get; init; } = commandGroup;
        Measurement Result { get; init; } = result;
        Runner Runner { get; init; } = runner;

        bool DebugOutput { get; init; } = debug;
        bool VerboseOutput { get; init; } = commandGroup.Flags.ContainsKey(Flag.VerboseCalculation);

        // Console.WriteLine(string.Format(new NumberFormatInfo() { NumberDecimalDigits = 2 }, "{0:F}", new decimal(1234.567)));

        public string FormatOutput()
        {
            var resultString = "";
            
            if (DebugOutput) resultString += CommandGroup + "\n";

            resultString += Runner.Command + "\n";
            
            resultString += $"Result:\t\t{Result}";

            if (VerboseOutput && Runner.Command is Commands.Convert convert) 
                resultString += $" (x{convert.ConversionFactor})";
            
            return resultString + "\n";
        }
    }
}
