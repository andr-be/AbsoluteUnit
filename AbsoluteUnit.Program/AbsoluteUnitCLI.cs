using AbsoluteUnit.Program;
using AbsoluteUnit.Program.Factories;
using AbsoluteUnit.Program.Commands;

namespace AbsoluteUnit
{
    internal class AbsoluteUnitCLI
    {
        static void Main(string[] args)
        {
            var commandFactory = new CommandFactory(args);
            var commandGroup = commandFactory.ParseArguments();

            var commandRunner = new Runner(commandGroup); 
            // intentionally left long to remind you to fix this mess
            commandRunner.ParseCommands(new MeasurementParser(ParserFactory.CreateUnitGroupParser(), ParserFactory.CreateUnitFactory()));
            
            var result = commandRunner.Run();

            Console.WriteLine(commandGroup);
            Console.WriteLine(commandRunner.Command);
            Console.WriteLine(result);
        }
    }

    public static class ParserFactory
    {
        public static IUnitGroupParser CreateUnitGroupParser() => new UnitGroupParser();

        public static IUnitFactory CreateUnitFactory() => new UnitFactory();
    }
}
