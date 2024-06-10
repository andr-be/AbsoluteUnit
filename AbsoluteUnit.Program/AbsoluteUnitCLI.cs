using AbsoluteUnit.Program;

namespace AbsoluteUnit
{
    internal class AbsoluteUnitCLI
    {
        static void Main(string[] args)
        {
            var commandGroup = new CommandParser(args).CommandGroup;
            Console.WriteLine(commandGroup);

            var measurementGroup = new MeasurementParser(commandGroup.CommandArguments[0])
                .ProcessMeasurement();

            Console.WriteLine(measurementGroup);

        }
    }
}
