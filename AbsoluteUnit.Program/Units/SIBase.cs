namespace AbsoluteUnit.Program.Units;

public class SIBase : IUnit
{
    public enum Units
    {
        Meter,
        Gram,
        Second,
        Ampere,
        Kelvin,
        Mole,
        Candela
    }

    public SIBase(Units unit) => Unit = unit;
    public SIBase(string unitString) => Unit = ((SIBase)ValidUnitStrings[unitString]).Unit;

    public object Unit { get; }

    public static Dictionary<string, object> ValidUnitStrings { get; } = new()
    {
        { "m", new SIBase(Units.Meter) },
        { "g", new SIBase(Units.Gram) },
        { "s", new SIBase(Units.Second) },
        { "A", new SIBase(Units.Ampere) },
        { "K", new SIBase(Units.Kelvin) },
        { "mol", new SIBase(Units.Mole) },
        { "cd", new SIBase(Units.Candela) },
    };

    public string Symbol => Unit switch
    {
        Units.Meter => "m",
        Units.Gram => "g",
        Units.Second => "s",
        Units.Ampere => "A",
        Units.Kelvin => "K",
        Units.Mole => "mol",
        Units.Candela => "cd",
        _ => throw new NotImplementedException($"Invalid SI Base Unit: {Unit}")
    };

    public double ToBase(double value) => value;

    public double FromBase(double value) => value;

    public override bool Equals(object? obj) =>
        obj is SIBase other &&
        Unit.Equals(other.Unit);

}
