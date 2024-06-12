namespace AbsoluteUnit.Program;

public class AbsUnit(IUnit unit, int exponent, SIPrefix prefix)
{
    public IUnit Unit { get; set; } = unit;
    public int Exponent { get; set; } = exponent;
    public SIPrefix Prefix { get; set; } = prefix;

    public override string ToString()
    {
        string exponent = (Exponent != 1) ? "^" + Exponent : "";
        return $"{Prefix}{Unit.Symbol}{exponent}";
    }
}

public class AbsMeasurement
{
    public List<AbsUnit> Units { get; set; }
    public double Quantity { get; set; }
    public int Exponent { get; set; }

    public AbsMeasurement(AbsUnit unit, double quantity = 0.0, int exponent = 1)
    {
        Units = [unit];
        Quantity = quantity;
        Exponent = exponent;
    }

    public AbsMeasurement(List<AbsUnit> units, double quantity = 0.0, int exponent = 1)
    {
        Units = units;
        Quantity = quantity;
        Exponent = exponent;
    }

    public override string ToString() => $"{Quantity}e{Exponent} {string.Join(".", Units)}";
}
