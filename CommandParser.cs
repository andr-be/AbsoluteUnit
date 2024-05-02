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

        public Command CommandType { get; set; }
        public List<Flag> Flags { get; set; }
        public Dictionary<Flag, int> FormatArguments = [];

        private bool Valid;
        private int ExtraArguments = 0;

        public CommandParser(string[] args)
        {
            CommandType = ParseCommand(args[0]);

            var flagsAndArguments = args.Skip(1).ToArray();
            Flags = GetFlags(flagsAndArguments);

            var arguments = flagsAndArguments.Where(a => a[0] != '-').ToArray();
            if (!(Valid = ValidCount(arguments))) 
                throw new ArgumentException($"Invalid argument count: {arguments.Length}");
        }

        private static Command ParseCommand(string command) => command.ToLowerInvariant() switch
        {
            "--convert" or "-c" => Command.Convert,
            "--express" or "-e" => Command.Express,
            "--simplify" or "-s" => Command.Simplify,
            _ => throw new CommandNotRecognised($"Invalid command: {command}")
        };

        private bool ValidCount(string[] args) => CommandType switch
        {
            Command.Convert => args.Length == 2 + ExtraArguments,
            Command.Express or Command.Simplify => args.Length == 1 + ExtraArguments,
            _ => false
        };

        private List<Flag> GetFlags(string[] args)
        {
            var flags = new List<Flag>();

            // get all of the various flags
            for (int i = 0; i < args.Length; i++)
            {
                string arg;
                if (args[i][0] == '-') arg = args[i];
                else continue;

                Flag newFlag = ParseFlag(arg);
                if (newFlag.AddsArguments())
                {
                    ParseArgument(args[i+1], newFlag);
                }

                flags.Add(newFlag);
            }

            return flags;
        }

        private void ParseArgument(string arg, Flag newFlag)
        {
            try
            {
                var formatArgument = int.Parse(arg);
                FormatArguments.Add(newFlag, formatArgument);
                ExtraArguments++;
            }
            catch
            {
                throw new ArgumentException($"Invalid flag argument provided: {arg}");
            }
        }

        private static Flag ParseFlag(string flag) => flag.ToLowerInvariant() switch
        {
            "-ver" or "--verbose" => Flag.VerboseCalculation,
            "-dec" or "--decimal" => Flag.DecimalPlaces,
            "-sig" or "--significant" => Flag.SignificantFigures,
            "-std" or "--standard" => Flag.StandardForm,
            "-eng" or "--engineering" => Flag.Engineering,
            _ => throw new FlagNotRecognised($"Invalid flag: {flag}")
        };
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
        public static bool AddsArguments(this CommandParser.Flag flag) => flag switch
        {
            CommandParser.Flag.SignificantFigures or CommandParser.Flag.DecimalPlaces => true,
            _ => false,
        };

    }
}
