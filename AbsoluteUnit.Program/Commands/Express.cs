using AbsoluteUnit.Program.Parsers.ParserGroups;
using AbsoluteUnit.Program.Structures;

namespace AbsoluteUnit.Program.Commands
{
    public class Express(CommandGroup commandGroup, IMeasurementParser measurementParser) : ICommand
    {
        public Measurement Input { get; init; } = measurementParser.ProcessMeasurement(commandGroup.CommandArguments[0]);

        public List<Measurement> Run() => [Input.ExpressInBaseUnits()];

        public override string ToString() =>
            $"{commandGroup.CommandType}:\t{Input} expressed in base units";
    }
}
