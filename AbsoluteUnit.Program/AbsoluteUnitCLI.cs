using AbsoluteUnit.Program;
using AbsoluteUnit.Program.Factories;
using AbsoluteUnit.Program.Commands;
using System;

namespace AbsoluteUnit
{
    public class AbsoluteUnitCLI
    {
        public static void Main(string[] args)
        {
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
            

            Console.WriteLine(commandGroup);
            Console.WriteLine(commandRunner.Command);
            Console.WriteLine(result);
        }

        private static string[][] testArguments =
        [
            ["--convert", "0.2330 in/µs", "m/s", "-sig", "3"],
            ["--convert", "20 m/s", "km/h"],
            ["--express", "100J"],
            ["--convert", "10 days", "hours"],
        ];
    }

    public static class ParserFactory
    {
        public static IUnitGroupParser CreateUnitGroupParser() => new UnitGroupParser();

        public static IUnitFactory CreateUnitFactory() => new UnitFactory();

        public static MeasurementParser CreateMeasurementParser() => new(CreateUnitGroupParser(), CreateUnitFactory());
    }
}
