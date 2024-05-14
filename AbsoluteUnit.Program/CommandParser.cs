using EnumExtension;

namespace AbsoluteUnit
{
    public class CommandParser
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

        public Command CommandType { get; }
        public List<string> CommandArguments { get; }
        public Dictionary<Flag, int> Flags { get; } = [];

        private int ExtraArguments = 0;

        public CommandParser(string[] args)
        {
            CommandType = ParseCommand(args.First());

            Flags = GetFlags(args.Skip(1).ToArray());

            CommandArguments = GetCommandArguments(args.Skip(1).ToArray());
        }

        /// <summary>
        /// returns the enum representation of a command, given the correct string
        /// </summary>
        /// <param name="commandString"></param>
        /// <returns>Command enum</returns>
        /// <exception cref="CommandNotRecognised"></exception>
        private static Command ParseCommand(string commandString) => commandString.ToLowerInvariant() switch
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
        private int CommandArgumentCount() => CommandType switch
        {
            Command.Convert => 2,
            Command.Express => 1,
            Command.Simplify => 1,
            _ => 0
        };

        /// <summary>
        /// iterates through the different arguments and creates a dictionary of flags and arguments
        /// </summary>
        /// <param name="args">user-provided CLI arguments</param>
        /// <returns>(Flag, int) dictionary of flags and arguments</returns>
        private Dictionary<Flag, int> GetFlags(string[] args)
        {
            var flags = new Dictionary<Flag, int>();
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i][0] != '-') 
                    continue;

                Flag newFlag = ParseFlag(args[i]);
                if (!newFlag.AddsArguments())
                {
                    flags.Add(newFlag, 0);
                    continue;
                }

                var arg = GetFlagArgument(args[i+1]);
                flags.Add(newFlag, arg);
                ExtraArguments++;
            }
            return flags;
        }

        /// <summary>
        /// a hacky way of getting all the command arguments by assuming they're listed first
        /// </summary>
        /// <param name="flagsAndArguments">all of the command line arguments except the first</param>
        /// <returns>a list of only the command arguments</returns>
        /// <exception cref="ArgumentException">invalid argument count error</exception>
        private List<string> GetCommandArguments(string[] flagsAndArguments)
        {
            var arguments = flagsAndArguments.Where(a => a[0] != '-').ToArray();

            if (CommandArgumentCount() + ExtraArguments == arguments.Length)
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
    using AbsoluteUnit;

    public static class Extensions
    {
        /// <summary>
        /// tells you if a specific flag requires additional arguments
        /// </summary>
        /// <param name="flag">the flag to check</param>
        /// <returns>true if flag needs extra arguments, false if not</returns>
        public static bool AddsArguments(this CommandParser.Flag flag) => flag switch
        {
            CommandParser.Flag.DecimalPlaces => true,
            CommandParser.Flag.SignificantFigures => true,
            _ => false,
        };
    }
}
