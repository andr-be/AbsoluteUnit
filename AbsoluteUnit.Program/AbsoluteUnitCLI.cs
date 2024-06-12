using AbsoluteUnit.Program;

namespace AbsoluteUnit
{
    internal class AbsoluteUnitCLI
    {
        static void Main(string[] args)
        {
            var commandGroup = new CommandParser(args).CommandGroup;

            var unitGroupParser = ParserFactory.CreateUnitGroupParser();
            var unitFactory = ParserFactory.CreateUnitFactory();

            var measurementParser = new MeasurementParser(unitGroupParser, unitFactory);
            var commandExecutor = new CommandExecutor(measurementParser, commandGroup);
            
            Console.WriteLine(commandGroup);
            Console.WriteLine(commandExecutor.Command);

            //var convertedResult = commandExecutor.Execute();
        }
    }

    public static class ParserFactory
    {
        public static IUnitGroupParser CreateUnitGroupParser() => new UnitGroupParser();

        public static IUnitFactory CreateUnitFactory() => new UnitFactory();
    }
}
