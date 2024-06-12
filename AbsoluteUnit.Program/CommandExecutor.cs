namespace AbsoluteUnit.Program
{
    public interface ICommand
    {
        public abstract AbsMeasurement Execute();
    }

    public class CommandExecutor(CommandGroup commandGroup, IMeasurementParser measurementParser)
    {
        public ICommand Command { get; set; } = GetCommandType(commandGroup, measurementParser);

        private readonly IMeasurementParser MeasurementParser = measurementParser;

        private static ICommand GetCommandType(CommandGroup commandGroup, IMeasurementParser measurementParser) => commandGroup.CommandType switch
        {
            AbsoluteUnit.Command.Convert => new Convert(measurementParser, commandGroup),
            AbsoluteUnit.Command.Express => new Express(measurementParser, commandGroup),
            AbsoluteUnit.Command.Simplify => new Simplify(measurementParser, commandGroup),
            _ => throw new ArgumentException($"Command {commandGroup.CommandType} not recognised!")
        };

        public AbsMeasurement Execute() => Command.Execute();
    }

    public class Convert : ICommand
    {
        public readonly AbsMeasurement FromUnit;
        public readonly AbsMeasurement ToUnit;
        public readonly double ConversionFactor;
        
        private CommandGroup CommandGroup { get; }

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
                .Select(ConversionToBase)
                .Aggregate((x, y) => x * y);
            
            var fromBase = toUnit.Units
                .Select(ConversionFromBase)
                .Aggregate((x, y) => x * y);

            return toBase * fromBase;
        }

        public AbsMeasurement Execute() => 
            new(ToUnit.Units, FromUnit.Quantity * ConversionFactor, FromUnit.Exponent);

        public override string ToString() => 
            $"{CommandGroup.CommandType}:\t{FromUnit} -> {string.Join(".", ToUnit.Units)} == {Execute()}";

        private static double ConversionFromBase(AbsUnit unit) => unit.Unit.FromBase(1) * PrefixValue(unit);
        private static double ConversionToBase(AbsUnit unit) => unit.Unit.ToBase(1) * PrefixValue(unit);
        private static double PrefixValue(AbsUnit unit) => Math.Pow(10.0, unit.Prefix.Prefix.Factor());
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
