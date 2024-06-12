namespace AbsoluteUnit.Program
{
    public interface ICommand
    {
        public abstract AbsMeasurement Execute();
    }

    public class CommandExecutor
    {
        public ICommand Command { get; set; }

        private IMeasurementParser MeasurementParser { get; set; }

        public CommandExecutor(
            IMeasurementParser measurementParser,
            CommandGroup commandGroup
        ){
            MeasurementParser = measurementParser;
            Command = commandGroup.CommandType switch
            {
                AbsoluteUnit.Command.Convert => new Convert(MeasurementParser, commandGroup),
                AbsoluteUnit.Command.Express => new Express(MeasurementParser, commandGroup),
                AbsoluteUnit.Command.Simplify => new Simplify(MeasurementParser, commandGroup),
                _ => throw new ArgumentException($"Command {commandGroup.CommandType} not recognised!")
            };
        }

        public AbsMeasurement Execute() => Command.Execute();
    }

    public class Convert : ICommand
    {
        private CommandGroup CommandGroup { get; }

        private AbsMeasurement FromUnit;
        private AbsMeasurement ToUnit;
        
        private double ConversionFactor;

        public Convert(IMeasurementParser measurementParser, CommandGroup commandGroup)
        {
            CommandGroup = commandGroup;
            FromUnit = measurementParser.ProcessMeasurement(commandGroup.CommandArguments[0]);
            ToUnit = measurementParser.ProcessMeasurement(commandGroup.CommandArguments[1], unitOnly: true);
            ConversionFactor = GetConversionFactor(FromUnit, ToUnit);
        }

        private static double GetConversionFactor(AbsMeasurement fromUnit, AbsMeasurement toUnit)
        {
            var toBase = fromUnit.Units
                .Select(u => u.Unit.ToBase(1))
                .Aggregate((x, y) => x * y);

            var fromBase = toUnit.Units
                .Select(u => u.Unit.FromBase(1))
                .Aggregate((x, y) => x * y);

            return toBase * fromBase;
        }

        public AbsMeasurement Execute() => 
            new(ToUnit.Units, FromUnit.Quantity * ConversionFactor, FromUnit.Exponent);

        public override string ToString() => 
            $"{CommandGroup.CommandType}:\t{FromUnit} -> {string.Join(".", ToUnit.Units)} == {Execute()}";
    }

    public class Simplify(IMeasurementParser measurementParser, CommandGroup commandGroup) : ICommand
    {
        private CommandGroup CommandGroup { get; } = commandGroup;

        public AbsMeasurement Execute()
        {
            throw new NotImplementedException();
        }
    }

    public class Express(IMeasurementParser measurementParser, CommandGroup commandGroup) : ICommand
    {
        private CommandGroup CommandGroup { get; } = commandGroup;

        public AbsMeasurement Execute()
        {
            throw new NotImplementedException();
        }
    }

}
