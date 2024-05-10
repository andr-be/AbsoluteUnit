namespace AbsoluteUnit
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                CommandParser commandParser = new(args);
                
                Console.WriteLine($"Command: {commandParser.CommandType}\n");
                int i = 1; foreach (var arg in commandParser.CommandArguments) 
                    Console.WriteLine($"{i++}: {arg}");

                Console.WriteLine($"Flags: {commandParser.Flags.Count}");
                i = 1; foreach (var flag in commandParser.Flags)
                    Console.WriteLine($"{i++}: {flag}");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
