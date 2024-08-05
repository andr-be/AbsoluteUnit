using AbsoluteUnit.Program;
using AbsoluteUnit.Program.Interfaces;
using AbsoluteUnit.Program.Factories;
using AbsoluteUnit.Program.Commands;

namespace AbsoluteUnit
{
    internal class AbsoluteUnitCLI
    {
        static void Main(string[] args)
        {
            var command = new CommandFactory(args);
            var commandGroup = command.ParseArguments();

            var commandRunner = new Runner(
                    commandGroup, 
                    new MeasurementParser(
                        ParserFactory.CreateUnitGroupParser(), 
                        ParserFactory.CreateUnitFactory()
                        )
                    );

            var result = commandRunner.Execute();

            Console.WriteLine(commandGroup);
            Console.WriteLine(commandRunner.Command);
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
