using AbsoluteUnit.Program;
using AbsoluteUnit.Program.Commands;
using AbsoluteUnit.Program.Factories;

namespace AbsoluteUnit
{
    public class AbsoluteUnitCLI
    {
        public static void Main(string[] args)
        {
            var debug = DebugCheck(args?.FirstOrDefault() ?? "");
            if (debug)
                args = args?.TakeLast(args.Length - 1).ToArray() ?? [];

            if (args is not null or [] && args.Length > 0) 
            {
                Run(args, debug);
            }
            else
            {
                Console.WriteLine("No arguments provided; starting in test mode...\n");
                foreach (var test in testArguments) Run(test, debug);
            }
        }

        static bool DebugCheck(string arg)
            => arg is "-d" or "--debug" or "-D" or "--DEBUG";

        public static void Run(string[] args, bool debug)
        {
            try
            {
                var commandFactory = new CommandFactory(args);

                var commandGroup = commandFactory.ParseArguments();

                var calculator = new Calculator(commandGroup)
                    .ParseCommandArguments(ParserFactory.CreateMeasurementParser());

                var result = calculator.Calculate();

                var writer = new OutputWriter(commandGroup, result[0], calculator, debug);

                Console.WriteLine("Arguments:\t" + string.Join(' ', args));
                Console.WriteLine(writer.FormatOutput());
            }
            catch (Exception e)
            {
                string errorInner = debug 
                    ? e.StackTrace ?? "<BAD STACKTRACE>"
                    : $"ERROR {Math.Abs(HashCode.Combine(e.Message) % 1000)}";
                string errorMessage = e.Message ?? "null";
                Console.WriteLine($"\n{errorInner} : {errorMessage}\n");
            }
        }

        private readonly static string[][] testArguments =
        [
            ["--convert", "0.2330 in/µs", "m/s", "-dec", "2"],
            ["--convert", "0.2330 in/µs", "m/s", "-std"],

            ["-c", "20 m/s", "km/h", "-std", "-ver"],
            ["-c", "100mi/h", "m/s", "-dec", "4", "-ver"],
            ["-c", "10 days", "hours", "-std", "-ver"],

            ["--express", "100J"],
            ["-e", "100 kJ", "-std"],
            ["-e", "69.420 mi/h"],

            ["--simplify", "10 kg.m.s^-2"],
            ["-s", "1000 kg.m.s^-2"],
            ["-s", "1e-12 m"],
          
            // Invalid input handling
            ["--convert", "0.2330 i/µs", "m/s", "-std"],
            ["--simplify", "10 k.m.s^-2"],
            ["--exprss", "100J"],
            ["--convert", "25m", "miles/hour"],
            ["--convert", "25m", "m/s"],
        ];
    }

    public static class ParserFactory
    {
        public static IUnitGroupParser CreateUnitGroupParser() => new UnitGroupParser();

        public static IUnitFactory CreateUnitFactory() => new UnitFactory();

        public static MeasurementParser CreateMeasurementParser() => new(CreateUnitGroupParser(), CreateUnitFactory());
    }
}
