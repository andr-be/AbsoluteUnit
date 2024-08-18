using AbsoluteUnit.Program;
using AbsoluteUnit.Program.Commands;
using AbsoluteUnit.Program.Factories;

namespace AbsoluteUnit
{
    public class AbsoluteUnitCLI
    {
        public static void Main(string[] args)
        {
            if (args is not null && args.Length > 0) Run(args);

            foreach (var test in testArguments)
            {
                try
                {
                    Run(test);
                }
                catch (Exception e)
                {
                    Console.WriteLine("\n" + e.ToString() + "\n");
                }
            }
        }

        public static void Run(string[] args)
        {
            var commandFactory = new CommandFactory(args);
            var commandGroup = commandFactory.ParseArguments();

            // intentionally left long to remind you to fix this mess
            var commandRunner = new Runner(commandGroup)
                .ParseCommandArguments(ParserFactory.CreateMeasurementParser());

            var result = commandRunner.Run();

            var formatter = new CLIOutputFactory(commandGroup, result, commandRunner);
            Console.WriteLine(formatter.FormatOutput());
        }

        private readonly static string[][] testArguments =
        [
            ["--convert", "0.2330 in/µs", "m/s", "-sig", "3"],
            ["--convert", "20 m/s", "km/h", "-std", "-ver"],
            ["--express", "100J"],
            ["--express", "69.420 mi/h"],
            ["--convert", "10 days", "hours", "-ver"],
            ["--convert", "100mi/h", "m/s"],
        ];
    }

    public static class ParserFactory
    {
        public static IUnitGroupParser CreateUnitGroupParser() => new UnitGroupParser();

        public static IUnitFactory CreateUnitFactory() => new UnitFactory();

        public static MeasurementParser CreateMeasurementParser() => new(CreateUnitGroupParser(), CreateUnitFactory());
    }
}
