using AbsoluteUnit.Program;
using AbsoluteUnit.Program.Factories;
using AbsoluteUnit.Program.Commands;

namespace AbsoluteUnit
{
    public class AbsoluteUnitCLI
    {
        public static void Main(string[] args)
        {
            var commandFactory = new CommandFactory(args);
            var commandGroup = commandFactory.ParseArguments();

            // intentionally left long to remind you to fix this mess
            var commandRunner = new Runner(commandGroup)
                .ParseCommandArguments(new MeasurementParser(
                    ParserFactory.CreateUnitGroupParser(), 
                    ParserFactory.CreateUnitFactory()
                    )
                );

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
