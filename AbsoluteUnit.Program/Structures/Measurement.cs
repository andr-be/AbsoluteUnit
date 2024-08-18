
namespace AbsoluteUnit.Program.Structures;

public class Measurement(
    List<Unit>? units = null,
    double quantity = 1.0,
    int exponent = 0)
{
    public List<Unit> Units { get; set; } = units ?? [];
    public double Quantity { get; set; } = quantity;
    public int Exponent { get; set; } = exponent;

    public Measurement(Unit unit, double quantity = 0.0, int exponent = 0)
        : this([unit], quantity, exponent)
    {
    }

    public Measurement ConvertTo(Measurement target, bool standardForm=false)
    {
        var newUnits = target.Units;
        var normalisedQuantity = this.Quantity * Math.Pow(10, this.Exponent);
        var convertedQuantity = CalculateNewQuantity(target, normalisedQuantity);

        (double newQuantity, int newExponent) = standardForm
            ? RepresentInStandardForm(convertedQuantity)
            : (convertedQuantity, 0);
        
        return new Measurement(newUnits, newQuantity, newExponent);
    }

    double CalculateNewQuantity(Measurement target, double quantity) => quantity * QuantityConversionFactor(target);

    public static (double newQuantity, int newExponent) RepresentInStandardForm(double convertedQuantity)
    {
        double factorOfTen = 0.0;
        double exponent = 0;

        while (convertedQuantity > factorOfTen)
            factorOfTen = Math.Pow(10, exponent++);
        
        return
        (
            newQuantity: convertedQuantity / (factorOfTen / 10),
            newExponent: (int)exponent
        );
    }

    public double QuantityConversionFactor(Measurement target)
    {
        // To convert a quantity to another, we do so via a base unit (SIBase)
        // If the unit is already SIBase, that factor is 1.
        var currentToBaseFactor = this.Units.AggregateToBaseConversionFactors();
        var targetFromBaseFactor = target.Units.AggregateFromBaseConversionFactors();

        return currentToBaseFactor * targetFromBaseFactor;
    }

    public Measurement ExpressInBaseUnits() => new
    (
        Units.SelectMany(u => u.BaseConversion()).ToList(),
        Quantity * Units.AggregateToBaseConversionFactors(),
        Exponent
    );

    public bool IsValidConversion(Measurement target)
    {
        var currentUnits = new Measurement(Units).ExpressInBaseUnits();
        var targetUnits = new Measurement(target.Units).ExpressInBaseUnits();

        return currentUnits.Units.SequenceEqual(targetUnits.Units);
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
