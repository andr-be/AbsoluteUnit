using AbsoluteUnit.Program.Structures;

namespace AbsoluteUnit.Program.Commands
{
    public class Runner(CommandGroup commandGroup)
    {
        public ICommand? Command { get; set; }
        public CommandGroup CommandGroup { get; init; } = commandGroup;

        public Runner ParseCommandArguments(IMeasurementParser measurementParser)
        {
            Command = GetCommandType(CommandGroup, measurementParser);

            return this;
        }

        public Measurement Run() => Command?.Run() ?? new();

        private static ICommand GetCommandType(CommandGroup commandGroup, IMeasurementParser parser) => commandGroup.CommandType switch
        {
            Factories.Command.Convert => new Convert(commandGroup, parser),
            Factories.Command.Express => new Express(commandGroup, parser),
            Factories.Command.Simplify => new Simplify(commandGroup, parser),
            _ => throw new ArgumentException($"Command {commandGroup.CommandType} not recognised!")
        };
    }
}
