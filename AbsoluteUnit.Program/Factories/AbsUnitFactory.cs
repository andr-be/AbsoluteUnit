using AbsoluteUnit.Program.Structures;
using AbsoluteUnit.Program.Units;

namespace AbsoluteUnit.Program.Factories;

public class AbsUnitFactory
{
    private SIPrefix Prefix = new(SIPrefix.Prefixes._None);
    private IUnitType UnitType = new SIBase(SIBase.Units.Meter);
    private int Exponent = 1;

    public Unit Build() => new(UnitType, Exponent, Prefix);

    public AbsUnitFactory WithPrefix(SIPrefix.Prefixes prefix)
    {
        Prefix = new(prefix);
        return this;
    }

    public AbsUnitFactory WithUnit(IUnitType unit)
    {
        UnitType = unit;
        return this;
    }

    public AbsUnitFactory WithExponent(int exponent)
    {
        Exponent = exponent;
        return this;
    }

    private static Unit CreateSIUnit(
        string symbol,
        SIPrefix.Prefixes prefix = SIPrefix.Prefixes._None,
        int exponent = 1
        ) => new AbsUnitFactory()
            .WithUnit(new SIBase(symbol))
            .WithExponent(exponent)
            .WithPrefix(prefix)
            .Build();

    public static Unit Kilogram(int exponent = 1) => CreateSIUnit("g", SIPrefix.Prefixes.Kilo, exponent);
    public static Unit Meter(int exponent = 1) => CreateSIUnit("m", exponent: exponent);
    public static Unit Second(int exponent = 1) => CreateSIUnit("s", exponent: exponent);
    public static Unit Ampere(int exponent = 1) => CreateSIUnit("A", exponent: exponent);
    public static Unit Kelvin(int exponent = 1) => CreateSIUnit("K", exponent: exponent);
    public static Unit Candela(int exponent = 1) => CreateSIUnit("cd", exponent: exponent);
    public static Unit Mole(int exponent = 1) => CreateSIUnit("mol", exponent: exponent);
}
