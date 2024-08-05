using AbsoluteUnit.Program.Interfaces;
using AbsoluteUnit.Program.Structures;

namespace AbsoluteUnit.Program.Commands
{
    public class Runner
    {
        public ICommand Command { get; set; }

        private readonly IMeasurementParser MeasurementParser;

        public Runner(CommandGroup commandGroup, IMeasurementParser measurementParser)
        {
            MeasurementParser = measurementParser;
            Command = GetCommandType(commandGroup);
        }

        public AbsMeasurement Execute() => Command.Execute();

        private ICommand GetCommandType(CommandGroup commandGroup) => commandGroup.CommandType switch
        {
            Factories.Command.Convert => new Convert(commandGroup, MeasurementParser),
            Factories.Command.Express => new Express(commandGroup, MeasurementParser),
            Factories.Command.Simplify => new Simplify(commandGroup, MeasurementParser),
            _ => throw new ArgumentException($"Command {commandGroup.CommandType} not recognised!")
        };
    }
}
