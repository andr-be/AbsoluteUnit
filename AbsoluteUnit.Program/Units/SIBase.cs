using UnitType = AbsoluteUnit.Program.Structures.Unit;

namespace AbsoluteUnit.Program.Units;

public class SIBase : IUnitType
{
    public SIBase(Units unit) => Unit = unit;
    public SIBase(string unitString) => Unit = ((SIBase)ValidUnitStrings[unitString]).Unit;

    public object Unit { get; }

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
    
    public static UnitType Kilogram(int exponent = 1) => new(new SIBase("g"), exponent, new(SIPrefix.Prefixes.Kilo));
    
    public static UnitType Meter(int exponent = 1) => new(new SIBase("m"), exponent);
    
    public static UnitType Second(int exponent = 1) => new(new SIBase("s"), exponent);
    
    public static UnitType Ampere(int exponent = 1) => new(new SIBase("A"), exponent);
    
    public static UnitType Kelvin(int exponent = 1) => new(new SIBase("K"), exponent);
    
    public static UnitType Candela(int exponent = 1) => new(new SIBase("cd"), exponent);
    
    public static UnitType Mole(int exponent = 1) => new(new SIBase("mol"), exponent);


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

    public List<UnitType> ExpressInBaseUnits()
    {
        if (this is not SIBase baseUnit) 
            throw new InvalidOperationException("wtf you're not meant to be able to do that...");

        return baseUnit.Unit switch
        {
            Units.Gram => [Kilogram()],
            _ => [new(this)]
        };
    }

    public override bool Equals(object? obj) =>
        obj is SIBase other &&
        Unit.Equals(other.Unit);

    public override int GetHashCode() => HashCode.Combine(Unit, Symbol);
}
