using AbsoluteUnit.Program.Structures;

namespace AbsoluteUnit.Program.Commands
{
    public class Simplify(CommandGroup commandGroup, IMeasurementParser measurementParser) : ICommand
    {
        private CommandGroup CommandGroup { get; } = commandGroup;

        public Measurement Run()
        {
            throw new NotImplementedException();
        }
    }

}
