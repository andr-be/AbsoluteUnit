using AbsoluteUnit.Program.Structures;

namespace AbsoluteUnit.Program.Commands
{
    public class Express : ICommand
    {
        private CommandGroup CommandGroup { get; }
        private Measurement InputMeasurement { get; }

        public Express(CommandGroup commandGroup, IMeasurementParser measurementParser)
        {
            CommandGroup = commandGroup;
            InputMeasurement = measurementParser.ProcessMeasurement(commandGroup.CommandArguments[0]);
        }

        public Measurement Run() => InputMeasurement.ExpressInBaseUnits();

        public override string ToString() =>
            $"{CommandGroup.CommandType}: {InputMeasurement} expressed in base units";
    }
}
