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
