namespace AbsoluteUnit
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                CommandParser commandParser = new(args);
                Console.WriteLine($"{commandParser.CommandType} mode selected!");
                for (int i = 1; i < args.Length; i++) Console.WriteLine($"arg [{i}]: {args[i]}");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
