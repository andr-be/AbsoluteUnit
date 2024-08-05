using System.Data;
using AbsoluteUnit.Program.Factories;
using AbsoluteUnit.Program.Structures;
using EnumExtension;

namespace AbsoluteUnit.Program.Factories
{
    public enum Command
    {
        Convert,
        Express,
        Simplify,
    }
    public enum Flag
    {
        VerboseCalculation,
        DecimalPlaces,
        SignificantFigures,
        StandardForm,
        Engineering,
    }
}

namespace AbsoluteUnit.Program.Factories
{
    public class CommandFactory(string[] args)
    {
        public CommandGroup? CommandGroup { get; private set; }
        public string[] Arguments { get; init; } = args;

        private int ExtraArguments = 0;

        public CommandGroup ParseArguments()
        {
            try
            {
                var commandType = ParseCommandType(Arguments.First());
                var flags = ParseFlags(Arguments.Skip(1).ToArray());
                var commandArgs = ParseCommandArguments(commandType, Arguments.Skip(1).ToArray());

                CommandGroup = new(commandType, flags, commandArgs);

                return CommandGroup;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// returns the enum representation of a command, given the correct string
        /// </summary>
        /// <param name="commandString"></param>
        /// <returns>Command enum</returns>
        /// <exception cref="CommandNotRecognised"></exception>
        private static Command ParseCommandType(string commandString) => commandString.ToLowerInvariant() switch
        {
            "--convert" or "-c" => Command.Convert,
            "--express" or "-e" => Command.Express,
            "--simplify" or "-s" => Command.Simplify,
            _ => throw new CommandNotRecognised($"Invalid command: {commandString}")
        };

        /// <summary>
        /// returns the enum representation of a flag given the correct string
        /// </summary>
        /// <param name="flagString">the string to be parsed</param>
        /// <returns>Flag enum</returns>
        /// <exception cref="FlagNotRecognised"></exception>
        private static Flag ParseFlag(string flagString) => flagString.ToLowerInvariant() switch
        {
            "-ver" or "--verbose" => Flag.VerboseCalculation,
            "-dec" or "--decimal" => Flag.DecimalPlaces,
            "-sig" or "--significant" => Flag.SignificantFigures,
            "-std" or "--standard" => Flag.StandardForm,
            "-eng" or "--engineering" => Flag.Engineering,
            _ => throw new FlagNotRecognised($"Invalid flag: {flagString}")
        };

        /// <summary>
        /// contains the number of valid arguments for each command type
        /// </summary>
        /// <returns>the number of valid arguments</returns>
        private static int CommandArgumentCount(Command type) => type switch
        {
            Command.Convert => 2,
            Command.Express => 1,
            Command.Simplify => 1,
            _ => 0
        };

        /// <summary>
        /// iterates through the different arguments and creates a dictionary of flags and arguments
        /// currently uses a very basic 'just grab the value after the argument' approach
        /// </summary>
        /// <param name="args">user-provided CLI arguments</param>
        /// <returns>(Flag, int) dictionary of flags and arguments</returns>
        private Dictionary<Flag, int> ParseFlags(string[] args)
        {
            var flags = new Dictionary<Flag, int>();
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i][0] != '-')
                    continue;

                Flag newFlag = ParseFlag(args[i]);
                if (newFlag.AddsArguments())
                {
                    var arg = GetFlagArgument(args[i + 1]);
                    flags.Add(newFlag, arg);
                    ExtraArguments++;
                }
                else flags.Add(newFlag, 0);
            }
            return flags;
        }

        /// <summary>
        /// a hacky way of getting all the command arguments by assuming they're listed first
        /// </summary>
        /// <param name="flagsAndArguments">all of the command line arguments except the first</param>
        /// <returns>a list of only the command arguments</returns>
        /// <exception cref="ArgumentException">invalid argument count error</exception>
        private List<string> ParseCommandArguments(Command type, string[] flagsAndArguments)
        {
            var arguments = flagsAndArguments.Where(a => a[0] != '-').ToArray();

            if (CommandArgumentCount(type) + ExtraArguments == arguments.Length)
                return arguments.Take(arguments.Length - ExtraArguments).ToList();
            else
                throw new ArgumentException($"Invalid argument count: {arguments.Length}");
        }

        private static int GetFlagArgument(string flagArgString)
        {
            try
            {
                return int.Parse(flagArgString);
            }
            catch (Exception e)
            {
                throw new ArgumentException($"invalid flag argument: {flagArgString}", innerException: e);
            }
        }


    }

    public class CommandNotRecognised : Exception
    {
        public CommandNotRecognised() { }
        public CommandNotRecognised(string message) : base(message) { }
        public CommandNotRecognised(string message, Exception inner) : base(message, inner) { }
    }

    public class FlagNotRecognised : Exception
    {
        public FlagNotRecognised() { }
        public FlagNotRecognised(string message) : base(message) { }
        public FlagNotRecognised(string message, Exception inner) : base(message, inner) { }
    }
}

namespace EnumExtension
{
    public static class Extensions
    {
        /// <summary>
        /// tells you if a specific flag requires additional arguments
        /// </summary>
        /// <param name="flag">the flag to check</param>
        /// <returns>true if flag needs extra arguments, false if not</returns>
        public static bool AddsArguments(this Flag flag) => flag switch
        {
            Flag.DecimalPlaces => true,
            Flag.SignificantFigures => true,
            _ => false,
        };
    }
}
