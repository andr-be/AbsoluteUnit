using AbsoluteUnit.Program.Parsers.ParserGroups;
using AbsoluteUnit.Program.Structures;

namespace AbsoluteUnit.Program.Commands
{
    public class Express : ICommand
    {
        private CommandGroup CommandGroup { get; }
        private Measurement Input { get; }

        public Express(CommandGroup commandGroup, IMeasurementParser measurementParser)
        {
            CommandGroup = commandGroup;
            Input = measurementParser.ProcessMeasurement(commandGroup.CommandArguments[0]);
        }

        public List<Measurement> Run() => [Input.ExpressInBaseUnits()];

        public override string ToString() =>
            $"{CommandGroup.CommandType}:\t{Input} expressed in base units";
    }
}
