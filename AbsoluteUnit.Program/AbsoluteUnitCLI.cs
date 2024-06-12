using AbsoluteUnit.Program;

namespace AbsoluteUnit
{
    internal class AbsoluteUnitCLI
    {
        static void Main(string[] args)
        {
            var commandGroup = new CommandParser(args).CommandGroup;
            var measurementString = commandGroup.CommandArguments[0];

            var unitGroupParser = ParserFactory.CreateUnitGroupParser();
            var unitFactory = ParserFactory.CreateUnitFactory();
            var measurementParser = new MeasurementParser(unitGroupParser, unitFactory, measurementString);

            var measurementGroup = measurementParser.ProcessMeasurement();
            
            Console.WriteLine(commandGroup);
            Console.WriteLine(measurementGroup);
        }
    }

    public static class ParserFactory
    {
        public static IUnitGroupParser CreateUnitGroupParser() => new UnitGroupParser();

        public static IUnitFactory CreateUnitFactory() => new UnitFactory();
    }
}
