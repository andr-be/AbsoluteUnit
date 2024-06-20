using AbsoluteUnit.Program;
using AbsoluteUnit.Program.Interfaces;
using AbsoluteUnit.Program.Parsers;
using AbsoluteUnit.Program.Factories;
using AbsoluteUnit.Program.Commands;

namespace AbsoluteUnit
{
    internal class AbsoluteUnitCLI
    {
        static void Main(string[] args)
        {

            var unitGroupParser = ParserFactory.CreateUnitGroupParser();
            var unitFactory = ParserFactory.CreateUnitFactory();
            var measurementParser = new MeasurementParser(unitGroupParser, unitFactory);

            var commandGroup = new CommandParser(args).CommandGroup;
            var commandExecutor = new Executor(commandGroup, measurementParser);

            var result = commandExecutor.Execute();

            Console.WriteLine(commandGroup);
            Console.WriteLine(commandExecutor.Command);
            Console.WriteLine(result);

            //var convertedResult = commandExecutor.Execute();
        }
    }

    public static class ParserFactory
    {
        public static IUnitGroupParser CreateUnitGroupParser() => new UnitGroupParser();

        public static IUnitFactory CreateUnitFactory() => new UnitFactory();
    }
}
