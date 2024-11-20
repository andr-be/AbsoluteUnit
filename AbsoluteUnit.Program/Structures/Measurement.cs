
using AbsoluteUnit.Program.UnitTypes;

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
        if (!IsLegalConversion(target)) 
            throw new ArgumentException($"Invalid conversion! -> {this} != {target}");
        
        var newUnits = target.Units;
        var normalisedQuantity = this.Quantity * Math.Pow(10, this.Exponent);
        var convertedQuantity = normalisedQuantity * QuantityConversionFactor(this, target);

        (double newQuantity, int newExponent) = standardForm
            ? StandardForm.RepresentInStandardForm(convertedQuantity)
            : (convertedQuantity, 0);
        
        return new Measurement(newUnits, newQuantity, newExponent);
    }

    /// <summary>
    /// The double value you need to multiple <b>current</b>'s quantity by to convert it to <b>target's<br/> units.
    /// </summary>
    /// <param name="current"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public static double QuantityConversionFactor(Measurement current, Measurement target)
    {
        /* To convert a quantity to another, we do so via a base unit (SIBase)
           If the unit is already SIBase, that factor is 1. 
        */
        var currentToBaseFactor = current.Units.AggregateToBaseConversionFactors();
        var targetFromBaseFactor = target.Units.AggregateFromBaseConversionFactors();

        return currentToBaseFactor * targetFromBaseFactor;
    }

    /// <summary>
    /// Returns a new measurement that's the same as the current one represented in base SI units. <br/>
    /// The quantity is converted and the exponent is left unchanged, so this is not a numerically simplified representation
    /// </summary>
    /// <returns>base SI representation Measurement</returns>
    public Measurement ExpressInBaseUnits()
    {
        var baseSIUnits = Units.SelectMany(u => u.ExpressInBaseUnits()).ToList();
        var newQuantity = Quantity * Units.AggregateToBaseConversionFactors();
        var newExponent = Exponent;

        // Combine like terms
        var combinedUnits = baseSIUnits
            .GroupBy(u => u.UnitType.UnitType)  // Group by the base unit type
            .Select(g =>
            {
                var totalExponent = g.Sum(u => u.Exponent);
                if (totalExponent == 0) return null;

                return (g.Key is SIBase.Units.Gram)
                    ? Unit.OfType(SIBase.Units.Gram, SIPrefix.Prefixes.Kilo, totalExponent)
                    : Unit.OfType(g.Key, exponent: totalExponent);

            })
            .Where(x => x is not null && x.Exponent != 0)  // Remove cancelled units
            .Select(x => Unit.OfType(x.UnitType.UnitType, x.Prefix.Prefix, x.Exponent))
            .ToList();

        return new(combinedUnits, newQuantity, newExponent);
    }

    /// <summary>
    /// Converts both this's units and the target's units to base representations and checks if they match <br/>
    /// If they don't match, you cannot convert from this to the target.
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
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

        return unitsEqual 
            && quantityEqual 
            && exponentEqual;
    }

    public override int GetHashCode() => 
        HashCode.Combine(Units, Quantity, Exponent);
}

public static class StandardForm
{
    public enum Style
    {
        /// <summary>
        /// Scientific notation where 0 < |x| < 10
        /// </summary>
        Scientific,

        /// <summary>
        /// Normalised notation where 1 < |x| < 10
        /// </summary>
        Normalised,

        /// <summary>
        /// Engineering notation where 1 <= |x| < 1000 and y ≡ 0 (mod 3), y ∈ ℤ 
        /// </summary>
        Engineering,
    }

    /// <summary>
    /// Simplifies a value to a chosen Standard Form representation
    /// </summary>
    /// <param name="quantity">the value to standardise</param>
    /// <param name="type">the scheme to standardise the value to</param>
    /// <returns>
    /// - Scientific:  x * 10^y; 0  < |x| <   10, y ∈ ℤ <br/>
    /// - Normalised:  x * 10^y; 1  < |x| <   10, y ∈ ℤ <br/>
    /// - Engineering: x * 10^y; 1 <= |x| < 1000, y ≡ 0 (mod 3), y ∈ ℤ
    /// </returns>
    /// <exception cref="NotImplementedException"></exception>
    public static (double newQuantity, int newExponent) RepresentInStandardForm(double quantity, Format? format = null)
    {
        if (quantity == 0) return (0, 0);

        var base10Log = Math.Log10(Math.Abs(quantity));

        var newExponent = (int)Math.Floor(base10Log);
        var newQuantity = quantity / Math.Pow(10, newExponent);

        return format?.Style switch
        {
            Style.Scientific => (newQuantity, newExponent),
            Style.Normalised => NormalisedForm(newQuantity, newExponent),
            Style.Engineering => EngineeringForm(newQuantity, newExponent),
            null => (quantity, 0),
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// if the quantity is below 1, bump the quantity up by *10, reduce the exponent by 1
        /// </summary>
        static (double quantity, int exponent) NormalisedForm(double initialQuantity, int initialExponent) =>
            (Math.Abs(initialQuantity) < 1)
                ? (initialQuantity * 10, initialExponent - 1)
                : (initialQuantity, initialExponent);

        /// <summary>
        /// reduce the exponent to the nearest multiple of 3 and normalise the quantity by the same amount
        /// </summary>
        static (double quantity, int exponent) EngineeringForm(double initialQuantity, int initialExponent) =>
        (
            quantity: initialQuantity * Math.Pow(10, initialExponent % 3),
            exponent: initialExponent - initialExponent % 3
        );
    }

    public record Format(bool InStandardForm = false, Style? Style = null)
    {
    }

}