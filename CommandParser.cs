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
        };

        public Command CommandType { get; set; }
        public bool ValidArgumentCount { get; set; }
        public string[] Flags { get; set; }

        public CommandParser(string[] args)
        {
            CommandType = ParseCommand(args[0]);
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
    }
}
