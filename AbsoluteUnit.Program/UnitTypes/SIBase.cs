using AbsoluteUnit.Program.Structures;

namespace AbsoluteUnit.Program.UnitTypes;

public class SIBase : IUnitType
{
    public SIBase(Units unit) => UnitType = unit;
    public SIBase(string unitString) => UnitType = ((SIBase)ValidUnitStrings[unitString]).UnitType;

    public object UnitType { get; }

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
    
    public static Unit Kilogram(int exponent = 1) => new(new SIBase("g"), exponent, new(SIPrefix.Prefixes.Kilo));
    
    public static Unit Meter(int exponent = 1) => new(new SIBase("m"), exponent);
    
    public static Unit Second(int exponent = 1) => new(new SIBase("s"), exponent);
    
    public static Unit Ampere(int exponent = 1) => new(new SIBase("A"), exponent);
    
    public static Unit Kelvin(int exponent = 1) => new(new SIBase("K"), exponent);
    
    public static Unit Candela(int exponent = 1) => new(new SIBase("cd"), exponent);
    
    public static Unit Mole(int exponent = 1) => new(new SIBase("mol"), exponent);


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

    public string Symbol => UnitType switch
    {
        Units.Meter => "m",
        Units.Gram => "g",
        Units.Second => "s",
        Units.Ampere => "A",
        Units.Kelvin => "K",
        Units.Mole => "mol",
        Units.Candela => "cd",
        _ => throw new NotImplementedException($"Invalid SI Base Unit: {UnitType}")
    };

    public double ToBase(double value) => 
        UnitType is Units.Gram ? value / 1e3 : value;

    public double FromBase(double value) => 
        UnitType is Units.Gram ? value * 1e3 : value;

    public List<Unit> ExpressInBaseUnits(Unit unit) => 
        (UnitType is Units.Gram)
        ? [new(this, unit.Exponent, unit.Prefix)] 
        : [new(this, unit.Exponent)];

    public override bool Equals(object? obj) =>
        obj is SIBase other &&
        UnitType.Equals(other.UnitType);

    public override int GetHashCode() => 
        HashCode.Combine(UnitType, Symbol);
}
