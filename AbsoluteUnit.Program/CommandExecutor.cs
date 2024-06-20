using AbsoluteUnit.Program.Interfaces;
using AbsoluteUnit.Program.Structures;

namespace AbsoluteUnit.Program
{
    public class CommandExecutor
    {
        public ICommand Command { get; set; } 

        private readonly IMeasurementParser MeasurementParser;

        public CommandExecutor(CommandGroup commandGroup, IMeasurementParser measurementParser)
        {
            MeasurementParser = measurementParser;
            Command = GetCommandType(commandGroup);
        }
     
        public AbsMeasurement Execute() => Command.Execute();

        private ICommand GetCommandType(CommandGroup commandGroup) => commandGroup.CommandType switch
        {
            Program.Command.Convert => new Commands.Convert(commandGroup, MeasurementParser),
            Program.Command.Express => new Commands.Express(commandGroup, MeasurementParser),
            Program.Command.Simplify => new Commands.Simplify(commandGroup, MeasurementParser),
            _ => throw new ArgumentException($"Command {commandGroup.CommandType} not recognised!")
        };
    }
}
