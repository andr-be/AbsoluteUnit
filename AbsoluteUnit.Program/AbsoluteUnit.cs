namespace AbsoluteUnit
{
    internal class AbsoluteUnit
    {
        static void Main(string[] args)
        {
            DebugProgram(args);
        }

        private static void DebugProgram(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("No arguments provided.");
                return;
            }
            try
            {
                CommandParser commandParser = new(args);

                Console.WriteLine($"Command: {commandParser.CommandType}\n");
                int i = 1;
                foreach (var arg in commandParser.CommandArguments)
                {
                    Console.WriteLine($"{i++}: {arg}");
                    try
                    {
                        MeasurementParser parser = new(arg);
                        DebugPrint(parser);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                Console.WriteLine($"Flags: {commandParser.Flags.Count}");
                i = 1;
                foreach (var flag in commandParser.Flags)
                    Console.WriteLine($"{i++}: {flag}");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e);
            }
        }

        private static void DebugPrint(MeasurementParser parser)
        {
            Console.WriteLine($"qty: {parser.Quantity}");
            Console.WriteLine($"exp: {parser.Exponent}");
            Console.WriteLine($"units:");
            foreach (var unitGroup in parser.Units.Groups)
            {
                Console.WriteLine($"  div: {unitGroup._DivMulti}");
                Console.WriteLine($"  sym: {unitGroup.UnitSymbol}");
                Console.WriteLine($"  exp: {unitGroup.Exponent}\n");
            }
        }
    }
}
