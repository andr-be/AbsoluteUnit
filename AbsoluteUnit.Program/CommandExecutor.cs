using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbsoluteUnit.Program
{
    public interface ICommand
    {
        public abstract AbsMeasurement Execute();
    }

    public class CommandExecutor
    {
        public ICommand command;
        public CommandExecutor(CommandGroup commandGroup)
        {
            command = commandGroup.CommandType switch
            {
                Command.Convert => new Convert(commandGroup),
                Command.Express => new Express(commandGroup),
                Command.Simplify => new Simplify(commandGroup),
                _ => throw new ArgumentException($"Command {commandGroup.CommandType} not recognised!")
            };
        }
    }

    public class Convert(CommandGroup commandGroup) : ICommand
    {
        private CommandGroup CommandGroup { get; } = commandGroup;
        public AbsMeasurement Execute()
        {
            throw new NotImplementedException();
        }
    }

    public class Simplify(CommandGroup commandGroup) : ICommand
    {
        private CommandGroup CommandGroup { get; } = commandGroup;

        public AbsMeasurement Execute()
        {
            throw new NotImplementedException();
        }
    }

    public class Express(CommandGroup commandGroup) : ICommand
    {
        private CommandGroup CommandGroup { get; } = commandGroup;

        public AbsMeasurement Execute()
        {
            throw new NotImplementedException();
        }
    }

}
