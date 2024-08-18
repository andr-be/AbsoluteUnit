using AbsoluteUnit.Program.Structures;
using AbsoluteUnit.Program.Units;

namespace AbsoluteUnit.Tests;

[TestClass]
public class ConversionTests
{
    [TestMethod]
    public void Equals_OneMeterEqualsOneMeter()
    {
        var a = SIBase.Meter();
        var b = SIBase.Meter();

        Assert.AreEqual(a, b);
    }

    [TestMethod]
    public void Convert_MetersPerSecondToInchesPerMicrosecond_IsValidConversion()
    {
        var metersPerSecond = new Measurement
        (
            quantity: 5918.2,
            units: [
                SIBase.Meter(),
                SIBase.Second(-1)
            ]
        );

        var inchesPerMicroSecond = new Measurement
        (
            quantity: 0.2330,
            units:
            [
                Unit.OfType(USCustomary.Units.Inch),

                Unit.OfType(SIBase.Units.Second,
                    prefix: SIPrefix.Prefixes.Micro,
                    exponent: -1
                )
            ]
        );

        // Act
        var isValidForwards = metersPerSecond.IsValidConversion(target: inchesPerMicroSecond);
        var isValidBackwards = inchesPerMicroSecond.IsValidConversion(target: metersPerSecond);

        // Assert
        Assert.IsTrue(isValidForwards && isValidBackwards, 
            $"isValidForwards = {isValidForwards} && isValidBackwards = {isValidBackwards}");
    }

    [TestMethod]
    public void Convert_MetersPerSecondToPoundsPerSecondSquared_IsNotValidConversion()
    {
        // Arrange
        var metersPerSecond = new Measurement
        (
            [
                SIBase.Meter(),
                SIBase.Second(-1)
            ]
        );

        var poundsPerSecondSquared = new Measurement
        (
            [
                Unit.OfType(USCustomary.Units.Pound),
                SIBase.Second(-2)
            ]
        );

        // Act
        var isValid = metersPerSecond.IsValidConversion(target: poundsPerSecondSquared);

        // Assert
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void Convert_OneSquareKilometer_ToOneMillionSquareMeters()
    {
        // Arrange
        var squareKilometer = new Measurement
        (
            quantity: 1.0,
            units: [ Unit.OfType(SIBase.Units.Meter, 
                     SIPrefix.Prefixes.Kilo, exponent: 2) ]
        );

        var squareMeter = new Measurement
        (
            quantity: 1_000_000.0,
            units: [ Unit.OfType(SIBase.Units.Meter, exponent: 2) ]
        );

        // Act
        var result = squareKilometer.ConvertTo(squareMeter);

        // Assert
        AssertEqualityWithConfidence(squareMeter, result);
    }

    [TestMethod]
    public void Convert_OneCubicKilometer_ToOneBillionSquareMeters()
    {
        // Arrange
        var squareKilometer = new Measurement
        (
            quantity: 1.0,
            units: [ Unit.OfType(SIBase.Units.Meter,
                     SIPrefix.Prefixes.Kilo, exponent: 3) ]
        );

        var squareMeter = new Measurement
        (
            quantity: 1_000_000_000.0,
            units: [Unit.OfType(SIBase.Units.Meter, exponent: 3)]
        );

        // Act
        var result = squareKilometer.ConvertTo(squareMeter);

        // Assert
        AssertEqualityWithConfidence(squareMeter, result);
    }

    [TestMethod]
    public void Convert_OneFoot_ToMeters()
    {
        // Arrange
        var oneFoot = new Measurement
        (
            quantity: 1.0,
            units: [Unit.OfType(USCustomary.Units.Feet)]
        );

        var meters = new Measurement([SIBase.Meter()], 0.3048);

        // Act
        var convertedFoot = oneFoot.ConvertTo(meters);

        // Assert
        AssertEqualityWithConfidence(meters, convertedFoot);
    }

    [TestMethod]
    public void Convert_02330InchesPerMicrosecond_To5918MetersPerSecond()
    {
        // Arrange
        var _02330InchesPerMicroSecond = new Measurement
        (
            quantity: 0.2330,
            units: 
            [
                Unit.OfType(USCustomary.Units.Inch),

                Unit.OfType(SIBase.Units.Second, 
                    prefix: SIPrefix.Prefixes.Micro, 
                    exponent: -1
                )
            ] 
        );

        var expected = new Measurement
        (
            quantity: 5918.2,
            units: [
                SIBase.Meter(),
                SIBase.Second(-1)
            ]
        );

        // Act
        var convertedUnit = _02330InchesPerMicroSecond.ConvertTo(expected);

        // Assert
        AssertEqualityWithConfidence(expected, convertedUnit);
    }

    [TestMethod]
    public void Convert_OneTon_To907Kilograms()
    {
        var oneTon = new Measurement
        (
            quantity: 1.0,
            units:[Unit.OfType(USCustomary.Units.Ton)]
        );

        var expected = new Measurement
        (
            quantity: 907.18474,
            units: [SIBase.Kilogram()]
        );

        // Act
        var actual = oneTon.ConvertTo(expected);

        // Assert
        AssertEqualityWithConfidence(expected, actual);
    }

    [TestMethod]
    public void Convert_OneTon_ToBaseConversion()
    {
        var oneTon = new Measurement
        (
            quantity: 1.0,
            units: [Unit.OfType(USCustomary.Units.Ton)]
        );

        var inGrams = oneTon.Units.AggregateToBaseConversionFactors();

        Assert.AreEqual(907.18474e3, inGrams);
    }

    [TestMethod]
    public void Convert_OneKilogram_To2p2Pounds()
    {
        var kilogram = new Measurement(SIBase.Kilogram(), 1);

        var pounds = new Measurement
        (
            quantity: 2.20462262,
            units: [Unit.OfType(USCustomary.Units.Pound)]
        );

        // Act
        var actualValue = kilogram.ConvertTo(pounds);

        // Assert
        AssertEqualityWithConfidence(pounds, actualValue, delta:1e-8);
    }

    [TestMethod]
    public void Convert_2p2Pounds_ToKilograms()
    {
        // Arrange
        var pounds = new Measurement
        (
            quantity: 2.20462262,
            units: [Unit.OfType(USCustomary.Units.Pound)]
        );

        var oneKilogram = new Measurement
        (
            quantity: 1.0,
            units: [SIBase.Kilogram()]
        );

        // Act
        var actualValue = pounds.ConvertTo(oneKilogram);

        // Assert
        AssertEqualityWithConfidence(oneKilogram, actualValue, delta:1e-9);
    }

    [TestMethod]
    public void Convert_3600Seconds_ToOneHour()
    {
        var _3600Seconds = new Measurement
        (
            quantity: 3600.0,
            units: [SIBase.Second()]
        );

        var oneHour = new Measurement
        (
            quantity: 1.0,
            units: [Unit.OfType(Miscellaneous.Units.Hour)]
        );

        // Act
        var actualValue = _3600Seconds.ConvertTo(oneHour);

        // Assert
        AssertEqualityWithConfidence(oneHour, actualValue);
    }

    [TestMethod]
    public void Convert_OneHour_To3600Seconds()
    {
        // Arrange
        var oneHour = new Measurement
        (
            quantity: 1,
            units:[Unit.OfType(Miscellaneous.Units.Hour)]
        );

        var seconds = new Measurement(units:[SIBase.Second()], quantity: 3600);

        // Act
        var hoursInSeconds = oneHour.ConvertTo(seconds);

        // Assert
        Assert.AreEqual(seconds, hoursInSeconds);
    }

    [TestMethod]
    public void Convert_MilesPerHour_ToMetersPerSecond()
    {
        // Arrange
        var SixtyMilesPerHour = new Measurement
        (
            quantity: 60,
            units: 
            [ 
                Unit.OfType(USCustomary.Units.Miles), 
                Unit.OfType(Miscellaneous.Units.Hour, exponent:-1)
            ]
        );

        var metersPerSecond = new Measurement
        ( 
            quantity: 26.8224,
            units: [ 
                SIBase.Meter(), 
                SIBase.Second(-1) 
            ] 
        );

        // Act
        var result = SixtyMilesPerHour.ConvertTo( metersPerSecond );

        // Assert
        Assert.AreEqual(metersPerSecond, result);
    }

    [TestMethod]
    public void Convert_20MetersPerSecond_ToKilometersPerHour()
    {
        // Arrange
        var twentyMetersPerSecond = new Measurement
        (
            quantity: 20.0,
            units:
            [
                Unit.OfType(SIBase.Units.Meter),
                Unit.OfType(SIBase.Units.Second, exponent:-1)
            ]
        );

        var kilometerPerHour = new Measurement
        (
            quantity: 72.0,
            units:
            [
                Unit.OfType(SIBase.Units.Meter, SIPrefix.Prefixes.Kilo),
                Unit.OfType(Miscellaneous.Units.Hour, exponent: -1)
            ]
        );

        // Act
        var result = twentyMetersPerSecond.ConvertTo(kilometerPerHour);

        // Assert
        AssertEqualityWithConfidence(kilometerPerHour, result);
    }

    [TestMethod]
    public void Miscellaneous_FromBase_SecondsToHoursConvertsCorrectly()
    {
        // Arrange
        var _3600Seconds = new Measurement([SIBase.Second()], 3600.00);
        var _1Hour = new Measurement
        (
            quantity: 1.0,
            units: [Unit.OfType(Miscellaneous.Units.Hour)]
        );

        // Act
        var secondToHourConversionFactor = _3600Seconds.QuantityConversionFactor(_1Hour);
        var hourToSecondConversionFactor = _1Hour.QuantityConversionFactor(_3600Seconds);

        // Assert
        Assert.AreEqual(hourToSecondConversionFactor, 1/secondToHourConversionFactor);
    }

    [TestMethod]
    public void USCustomary_FromBaseAndToBase_DifferentFactorsGenerated()
    {
        // Arrange
        var oneMile = new Measurement([Unit.OfType(USCustomary.Units.Miles)], 1);
        var oneMeter = new Measurement([SIBase.Meter()], 1);

        // Act
        var mileToMeterConversionFactor = oneMile.QuantityConversionFactor(oneMeter);
        var meterToMileConversionFactor = oneMeter.QuantityConversionFactor(oneMile);

        // Assert
        Assert.AreNotEqual(mileToMeterConversionFactor, meterToMileConversionFactor);
    }

    [TestMethod]
    public void UnitAggregation_ProducesTheCorrectOutput()
    {
        List<Unit> units =
        [
            Unit.OfType(USCustomary.Units.Feet),
            SIBase.Second(-1),
        ];

        var aggregateConversionFactors = units.AggregateToBaseConversionFactors();

        Assert.AreEqual(0.3048, aggregateConversionFactors);
    }

    public static void AssertEqualityWithConfidence(Measurement expected, Measurement actual, double? delta = null) => Assert.AreEqual
    (
        expected: expected.Quantity,
        actual: actual.Quantity,
        delta: delta ?? 1e-12,
        $"\nactual: {actual}\nexpected: {expected}\ndelta: {expected.Quantity - actual.Quantity})"
    );
}