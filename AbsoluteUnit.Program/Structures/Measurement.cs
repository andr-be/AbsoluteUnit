
namespace AbsoluteUnit.Program.Structures;

public class Measurement(
    List<Unit>? units = null,
    double quantity = 0.0,
    int exponent = 1)
{
    public List<Unit> Units { get; set; } = units ?? [];
    public double Quantity { get; set; } = quantity;
    public int Exponent { get; set; } = exponent;

    public Measurement(Unit unit, double quantity = 0.0, int exponent = 1)
        : this([unit], quantity, exponent)
    {
    }

    public Measurement ConvertTo(Measurement target) =>
        new(target.Units, Quantity * QuantityConversionFactor(target), Exponent);

    public double QuantityConversionFactor(Measurement target) =>
         Units.AggregateConversionFactors() / target.Units.AggregateConversionFactors();

    public Measurement ExpressInBaseUnits() => new
    (
        Units.SelectMany(u => u.BaseConversion()).ToList(),
        Quantity * Units.AggregateConversionFactors(),
        Exponent
    );

    public bool IsValidConversion(Measurement target)
    {
        var currentUnits = new Measurement(Units).ExpressInBaseUnits();
        var targetUnits = new Measurement(target.Units).ExpressInBaseUnits();

        return currentUnits.Equals(targetUnits);
    }

    public override string ToString() =>
        $"{Quantity}{(Exponent != 0 ? $"e{Exponent}" : "")} {string.Join(".", Units)}";

    public override bool Equals(object? obj)
    {
        if (obj is not Measurement other) return false;
     
        var unitsEqual = Units.SequenceEqual(other.Units);
        var quantityEqual = Quantity.Equals(other.Quantity);
        var exponentEqual = Exponent.Equals(other.Exponent);

        return unitsEqual && quantityEqual && exponentEqual;
    }

    public override int GetHashCode() => HashCode.Combine(Units, Quantity, Exponent);
    
}
