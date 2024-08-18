using AbsoluteUnit.Program;
using AbsoluteUnit.Program.Commands;
using AbsoluteUnit.Program.Factories;

namespace AbsoluteUnit
{
    public class AbsoluteUnitCLI
    {
        public static void Main(string[] args)
        {
            var debug = true;
            
            if (args is not null && args.Length > 0) Run(args, debug);

            foreach (var test in testArguments)
            {
                try
                {
                    Run(test, debug);
                }
                catch (Exception e)
                {
                    Console.WriteLine("\n" + e.ToString() + "\n");
                }
            }
        }

        public static void Run(string[] args, bool debug)
        {
            var commandFactory = new CommandFactory(args);
            var commandGroup = commandFactory.ParseArguments();

            // intentionally left long to remind you to fix this mess
            var commandRunner = new Runner(commandGroup)
                .ParseCommandArguments(ParserFactory.CreateMeasurementParser());

            var result = commandRunner.Run();

            var formatter = new OutputFactory(commandGroup, result, commandRunner, debug);
            Console.WriteLine(formatter.FormatOutput());
        }

        private readonly static string[][] testArguments =
        [
            ["--convert", "0.2330 in/µs", "m/s", "-dec", "2"],
            ["--convert", "0.2330 in/µs", "m/s", "-std"],

            ["--convert", "20 m/s", "km/h", "-std", "-ver"],
            ["--convert", "100mi/h", "m/s", "-dec", "4", "-ver"],
            ["--convert", "10 days", "hours", "-std", "-ver"],

            ["--express", "100J"],
            ["--express", "69.420 mi/h"],

            ["--simplify", "10 kg.m.s^-2"]
        ];
    }

    public static class ParserFactory
    {
        public static IUnitGroupParser CreateUnitGroupParser() => new UnitGroupParser();

        public static IUnitFactory CreateUnitFactory() => new UnitFactory();

        public static MeasurementParser CreateMeasurementParser() => new(CreateUnitGroupParser(), CreateUnitFactory());
    }
}
