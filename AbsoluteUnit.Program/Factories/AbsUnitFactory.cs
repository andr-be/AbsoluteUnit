using AbsoluteUnit.Program.Structures;
using AbsoluteUnit.Program.Units;

namespace AbsoluteUnit.Program.Factories;

public class AbsUnitFactory
{
    private SIPrefix Prefix = new(SIPrefix.Prefixes._None);
    private IUnit Unit = new SIBase(SIBase.Units.Meter);
    private int Exponent = 1;

    public AbsUnit Build() => new(Unit, Exponent, Prefix);

    public AbsUnitFactory WithPrefix(SIPrefix.Prefixes prefix)
    {
        Prefix = new(prefix);
        return this;
    }

    public AbsUnitFactory WithUnit(IUnit unit)
    {
        Unit = unit;
        return this;
    }

    public AbsUnitFactory WithExponent(int exponent)
    {
        Exponent = exponent;
        return this;
    }

    private static AbsUnit CreateSIUnit(
        string symbol,
        SIPrefix.Prefixes prefix = SIPrefix.Prefixes._None,
        int exponent = 1
        ) => new AbsUnitFactory()
            .WithUnit(new SIBase(symbol))
            .WithExponent(exponent)
            .WithPrefix(prefix)
            .Build();

    public static AbsUnit Kilogram(int exponent = 1) => CreateSIUnit("g", SIPrefix.Prefixes.Kilo, exponent);
    public static AbsUnit Meter(int exponent = 1) => CreateSIUnit("m", exponent: exponent);
    public static AbsUnit Second(int exponent = 1) => CreateSIUnit("s", exponent: exponent);
    public static AbsUnit Ampere(int exponent = 1) => CreateSIUnit("A", exponent: exponent);
    public static AbsUnit Kelvin(int exponent = 1) => CreateSIUnit("K", exponent: exponent);
    public static AbsUnit Candela(int exponent = 1) => CreateSIUnit("cd", exponent: exponent);
    public static AbsUnit Mole(int exponent = 1) => CreateSIUnit("mol", exponent: exponent);
}
