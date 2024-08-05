using AbsoluteUnit.Program.Structures;

namespace AbsoluteUnit.Program.Commands
{
    public class Runner(CommandGroup commandGroup)
    {
        public ICommand? Command { get; set; }
        public CommandGroup CommandGroup { get; init; } = commandGroup;

        private IMeasurementParser? MeasurementParser;

        public void ParseCommands(IMeasurementParser measurementParser)
        {
            MeasurementParser = measurementParser;
            Command = GetCommandType(CommandGroup);
        }

        public AbsMeasurement Run() => Command?.Run() ?? new();

        private ICommand GetCommandType(CommandGroup commandGroup) => commandGroup.CommandType switch
        {
            Factories.Command.Convert => new Convert(commandGroup, MeasurementParser!),
            Factories.Command.Express => new Express(commandGroup, MeasurementParser!),
            Factories.Command.Simplify => new Simplify(commandGroup, MeasurementParser!),
            _ => throw new ArgumentException($"Command {commandGroup.CommandType} not recognised!")
        };
    }
}
