namespace AbsoluteUnit.Program.Interfaces;

public interface IUnit
{
    object Unit { get; }
    string Symbol { get; }
    double ToBase(double value);
    double FromBase(double value);
}
