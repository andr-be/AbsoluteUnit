namespace AbsoluteUnit.Program;

public class AbsUnitBuilder
{
    private SIPrefix Prefix = new(SIPrefix.Prefixes._None);
    private IUnit Unit = new SIBase(SIBase.Units.Meter);
    private int Exponent = 1;

    public AbsUnit Build() => new(Unit, Exponent, Prefix);

    public AbsUnitBuilder WithPrefix(SIPrefix prefix)
    {
        Prefix = prefix;
        return this;
    }

    public AbsUnitBuilder WithUnit(IUnit unit)
    {
        Unit = unit;
        return this;
    }

    public AbsUnitBuilder WithExponent(int exponent)
    {
        Exponent = exponent;
        return this;
    }

    private static AbsUnit CreateSIUnit(
        string symbol,
        SIPrefix.Prefixes prefix = SIPrefix.Prefixes._None,
        int exponent = 1
        ) => new AbsUnitBuilder()
            .WithUnit(new SIBase(symbol))
            .WithExponent(exponent)
            .WithPrefix(new SIPrefix(prefix))
            .Build();

    public static AbsUnit Kilogram(int exponent = 1) => CreateSIUnit("g", SIPrefix.Prefixes.Kilo, exponent);
    public static AbsUnit Meter(int exponent = 1) => CreateSIUnit("m", exponent: exponent);
    public static AbsUnit Second(int exponent = 1) => CreateSIUnit("s", exponent: exponent);
    public static AbsUnit Ampere(int exponent = 1) => CreateSIUnit("A", exponent: exponent);
    public static AbsUnit Kelvin(int exponent = 1) => CreateSIUnit("K", exponent: exponent);
    public static AbsUnit Candela(int exponent = 1) => CreateSIUnit("cd", exponent: exponent);
    public static AbsUnit Mole(int exponent = 1) => CreateSIUnit("mol", exponent: exponent);
}

public class AbsUnit(
    IUnit unit, 
    int exponent = 1, 
    SIPrefix? prefix = null
    )
{
    public IUnit Unit { get; set; } = unit;
    public int Exponent { get; set; } = exponent;
    public SIPrefix Prefix { get; set; } = prefix ?? new(SIPrefix.Prefixes._None);

    public override string ToString() => 
        $"{Prefix}{Unit.Symbol}{((Exponent != 1) ? "^" + Exponent : "")}";

    public double ConversionFromBase() => Unit.FromBase(1) * PrefixValue();

    public double ConversionToBase() => Unit.ToBase(1) / PrefixValue();

    public double PrefixValue() => Math.Pow(10.0, Prefix.Prefix.Factor());

    public List<AbsUnit> ExpressInBaseUnits() => MeasurementConverter.BaseConversion(this);

    public override bool Equals(object? obj)
    {
        if (obj is AbsUnit other)
        {
            return other.Unit.Equals(Unit)
                && other.Exponent.Equals(Exponent)
                && other.Prefix.Equals(Prefix);
        }
        return false;
    }
}

public class AbsMeasurement(
    List<AbsUnit>? units = null, 
    double quantity = 0.0, 
    int exponent = 1)
{
    public List<AbsUnit> Units { get; set; } = units ?? [];
    public double Quantity { get; set; } = quantity;
    public int Exponent { get; set; } = exponent;

    public AbsMeasurement(AbsUnit unit, double quantity = 0.0, int exponent = 1)
        : this([unit], quantity, exponent)
    {
    }

    public override string ToString() => 
        $"{Quantity}{((Exponent != 0) ? $"e{Exponent}" : "")} {string.Join(".", Units)}";

    public override bool Equals(object? obj)
    {
        if (obj is AbsMeasurement other)
        {
            return Units.SequenceEqual(other.Units)
                && Quantity.Equals(other.Quantity)
                && Exponent.Equals(other.Exponent);
        }
        else return false;
    }
}
