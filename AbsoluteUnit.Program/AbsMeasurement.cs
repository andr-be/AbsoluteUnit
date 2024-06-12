namespace AbsoluteUnit.Program;

public class AbsUnit(IUnit unit, int exponent, SIPrefix prefix)
{
    public IUnit Unit { get; set; } = unit;
    public int Exponent { get; set; } = exponent;
    public SIPrefix Prefix { get; set; } = prefix;

    public override string ToString() => 
        $"{Prefix}{Unit.Symbol}{((Exponent != 1) ? "^" + Exponent : "")}";
}

public class AbsMeasurement
{
    public List<AbsUnit> Units { get; set; }
    public double Quantity { get; set; }
    public int Exponent { get; set; }

    public AbsMeasurement(List<AbsUnit>? units = null, double quantity = 0.0, int exponent = 1)
    {
        Units = units ?? [];
        Quantity = quantity;
        Exponent = exponent;
    }

    public AbsMeasurement(AbsUnit unit, double quantity = 0.0, int exponent = 1)
        : this([unit], quantity, exponent)
    {
    }

    public override string ToString() => 
        $"{Quantity}e{Exponent} {string.Join(".", Units)}";
}
