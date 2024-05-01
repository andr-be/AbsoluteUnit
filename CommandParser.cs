using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public bool ValidArgumentCount { get; set; }

        public CommandParser(string[] args)
        {
            CommandType = ParseCommand(args[0]);
            Flags = GetFlags(args);
            ValidArgumentCount = IsValid(args);
        }

        private static Command ParseCommand(string command) => command switch
        {
            "--Convert" or "-C" => Command.Convert,
            "--Express" or "-E" => Command.Express,
            "--Simplify" or "-S" => Command.Simplify,
            _ => throw new ArgumentException("Invalid command given.")
        };

        private bool IsValid(string[] args) => CommandType switch
        {
            Command.Convert => args.Length == 2,
            Command.Express => args.Length == 1,
            Command.Simplify => args.Length == 1,
            _ => false
        };

        private List<Flag> GetFlags(string[] args)
        {
            var flags = new List<Flag>();

            // get all of the various flags
            var onlyFlags = args.Where(a => a[0] == '-');
            foreach (var flag in onlyFlags)
            {
                
            }

            return flags;
        }

    }
}
