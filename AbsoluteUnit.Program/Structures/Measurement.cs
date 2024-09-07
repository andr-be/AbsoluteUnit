
namespace AbsoluteUnit.Program.Structures;

/// <summary>
/// A Scientific Measurement, able to be converted and simplified into different forms.
/// </summary>
/// <param name="units"></param>
/// <param name="quantity"></param>
/// <param name="exponent"></param>
public class Measurement(
    List<Unit>? units = null,
    double quantity = 1.0,
    int exponent = 0)
{
    /// <summary>
    /// A compound collection of Units
    /// </summary>
    public List<Unit> Units { get; set; } = units ?? [];
    /// <summary>
    /// The numerical value of the Measurement
    /// </summary>
    public double Quantity { get; set; } = quantity;
    /// <summary>
    /// TrueQuantity = Quantity * (10 ^ Exponent)
    /// </summary>
    public int Exponent { get; set; } = exponent;

    /// <summary>
    /// A Scientific Measurement, able to be converted and simplified into different forms.
    /// <i><br>Constructor for when you only supply a single Unit; adds it to collection</br></i>
    /// </summary>
    /// <param name="unit">the Unit to instantiate</param>
    /// <param name="quantity">numerical value of the Measurement</param>
    /// <param name="exponent">quantity needs to be multiplied by 10^exponent for proper size</param>
    public Measurement(Unit unit, double quantity = 0.0, int exponent = 0)
        : this([unit], quantity, exponent)
    {
    }

    /// <summary>
    /// Convert a Measurement into a different Measurement of equivalent base types
    /// </summary>
    /// <param name="target">the target Units (quantity and exponent are ignored)</param>
    /// <param name="standardForm">whether or not to reduce the </param>
    /// <returns>this Measurement converted to Target's units</returns>
    public Measurement ConvertTo(Measurement target, bool standardForm=false)
    {
        var newUnits = target.Units;
        var normalisedQuantity = this.Quantity * Math.Pow(10, this.Exponent);
        var convertedQuantity = normalisedQuantity * QuantityConversionFactor(this, target);

        (double newQuantity, int newExponent) = standardForm
            ? RepresentInStandardForm(convertedQuantity)
            : (convertedQuantity, 0);
        
        return new Measurement(newUnits, newQuantity, newExponent);
    }

    public static (double newQuantity, int newExponent) RepresentInStandardForm(double convertedQuantity)
    {
        double factorOfTen = 0.0;
        double exponent = 0;

        while (convertedQuantity / factorOfTen > 10)
            factorOfTen = Math.Pow(10, exponent++);
        
        return
        (
            newQuantity: convertedQuantity / factorOfTen,
            newExponent: (int)exponent - 1
        );
    }

    public static double QuantityConversionFactor(Measurement current, Measurement target)
    {
        // To convert a quantity to another, we do so via a base unit (SIBase)
        // If the unit is already SIBase, that factor is 1.
        var currentToBaseFactor = current.Units.AggregateToBaseConversionFactors();
        var targetFromBaseFactor = target.Units.AggregateFromBaseConversionFactors();

        return currentToBaseFactor * targetFromBaseFactor;
    }

    public Measurement ExpressInBaseUnits()
    {
        var newUnits = Units.SelectMany(u => u.ExpressInBaseUnits()).ToList();
        var newQuantity = Quantity * Units.AggregateToBaseConversionFactors();
        var newExponent = Exponent;

        return new (newUnits, newQuantity, newExponent);
    }

    public bool IsLegalConversion(Measurement target)
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
