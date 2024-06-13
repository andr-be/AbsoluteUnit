namespace AbsoluteUnit.Program
{
    public interface ICommand
    {
        public abstract AbsMeasurement Execute();
    }

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
            AbsoluteUnit.Command.Convert => new Convert(commandGroup, MeasurementParser),
            AbsoluteUnit.Command.Express => new Express(commandGroup, MeasurementParser),
            AbsoluteUnit.Command.Simplify => new Simplify(commandGroup, MeasurementParser),
            _ => throw new ArgumentException($"Command {commandGroup.CommandType} not recognised!")
        };
    }

    public class Convert : ICommand
    {
        public AbsMeasurement FromUnit { get; }
        public AbsMeasurement ToUnit { get; }
        public double ConversionFactor { get; }

        private CommandGroup CommandGroup { get; }

        public Convert(CommandGroup commandGroup, IMeasurementParser measurementParser)
        {
            CommandGroup = commandGroup;
            FromUnit = measurementParser.ProcessMeasurement(commandGroup.CommandArguments[0]);
            ToUnit = measurementParser.ProcessMeasurement(commandGroup.CommandArguments[1], unitOnly: true);
            ConversionFactor = GetConversionFactor();
        }

        private double GetConversionFactor()
        {
            var toBase = FromUnit.Units
                .Select(u => u.ConversionToBase())
                .Aggregate((x, y) => x * y);
            
            var fromBase = ToUnit.Units
                .Select(u => u.ConversionFromBase())
                .Aggregate((x, y) => x * y);

            return toBase * fromBase;
        }

        public AbsMeasurement Execute() => 
            new(ToUnit.Units, FromUnit.Quantity * ConversionFactor, FromUnit.Exponent);

        public override string ToString() => 
            $"{CommandGroup.CommandType}:\t{FromUnit} -> {string.Join(".", ToUnit.Units)} == {Execute()}";
    }

    public class Simplify(CommandGroup commandGroup, IMeasurementParser measurementParser) : ICommand
    {
        private CommandGroup CommandGroup { get; } = commandGroup;

        public AbsMeasurement Execute()
        {
            throw new NotImplementedException();
        }
    }

    public class Express(CommandGroup commandGroup, IMeasurementParser measurementParser) : ICommand
    {
        private CommandGroup CommandGroup { get; } = commandGroup;

        public AbsMeasurement Execute()
        {
            throw new NotImplementedException();
        }
    }

}
