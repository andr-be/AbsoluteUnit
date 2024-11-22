using AbsoluteUnit.Program.Parsers.ParserGroups;
using AbsoluteUnit.Program.Structures;

namespace AbsoluteUnit.Program.Commands
{
    public class Calculator(CommandGroup commandGroup)
    {
        public ICommand? Command { get; set; }
        public CommandGroup CommandGroup { get; init; } = commandGroup;

        public Calculator ParseCommandArguments(IMeasurementParser measurementParser)
        {
            Command = GetCommandType(CommandGroup, measurementParser);

            return this;
        }

        public List<Measurement> Calculate() => Command?.Run() ?? [];

        private static ICommand GetCommandType(CommandGroup commandGroup, IMeasurementParser parser) => commandGroup.CommandType switch
        {
            Program.Command.Convert => new Convert(commandGroup, parser),
            Program.Command.Express => new Express(commandGroup, parser),
            Program.Command.Simplify => new Simplify(commandGroup, parser),
            _ => throw new CommandError(ErrorCode.InvalidCommand, $"Command {commandGroup.CommandType} not recognised!")
        };
    }
}
