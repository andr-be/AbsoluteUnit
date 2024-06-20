using AbsoluteUnit.Program.Structures;

namespace AbsoluteUnit.Program.Interfaces
{
    public interface ICommand
    {
        public abstract AbsMeasurement Execute();
    }

}
