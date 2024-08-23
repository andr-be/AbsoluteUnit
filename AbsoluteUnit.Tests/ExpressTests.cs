using AbsoluteUnit.Program.Structures;
using AbsoluteUnit.Program.UnitTypes;

namespace AbsoluteUnit.Tests;


[TestClass]
public class ExpressTests
{
    [TestMethod]
    public void Express_OneNewton_IsOneKilogramMeterPerSecondSquared()
    {
        Measurement oneNewton = new
        (
            [Unit.OfType(SIDerived.Units.Newton)]
        );

        Measurement expected = new
        (
            units: [
                Unit.OfType(SIBase.Units.Gram, SIPrefix.Prefixes.Kilo),
                Unit.OfType(SIBase.Units.Meter),
                Unit.OfType(SIBase.Units.Second, exponent:-2)
            ]
        );

        Measurement result = oneNewton.ExpressInBaseUnits();

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Express_OneNewtonSquared_IsOneKilogramSquaredMeterSquaredPerSecondToTheFour()
    {
        Measurement oneNewton = new
        (
            [Unit.OfType(SIDerived.Units.Newton, exponent:2)]
        );

        Measurement expected = new
        (
            units: [
                Unit.OfType(SIBase.Units.Gram, SIPrefix.Prefixes.Kilo, exponent:2),
                Unit.OfType(SIBase.Units.Meter, exponent:2),
                Unit.OfType(SIBase.Units.Second, exponent:-4)
            ]
        );

        Measurement result = oneNewton.ExpressInBaseUnits();

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Express_30MilesPerHour_IsThirteenPointFourOneOneTwoMetersPerSecond()
    {
        Measurement thirtyMilesPerHour = new
        (
            quantity: 30.0,
            units: [
                Unit.OfType(USCustomary.Units.Miles),
                Unit.OfType(Miscellaneous.Units.Hour, exponent:-1)
            ]
        );

        Measurement expected = new
        (
            quantity: 13.4112,
            units: [
                Unit.OfType(SIBase.Units.Meter),
                Unit.OfType(SIBase.Units.Second, exponent:-1)
            ]
        );

        Measurement result = thirtyMilesPerHour.ExpressInBaseUnits();

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Express_OneHz_IsOneSecondPowerMinusOne()
    {
        Measurement oneHz = new
        (
            quantity: 1.0,
            units: [Unit.OfType(SIDerived.Units.Hertz)]
        );

        Measurement expected = new
        (
            quantity: 1.0,
            units: [Unit.OfType(SIBase.Units.Second, exponent:-1)]
        );

        Measurement result = oneHz.ExpressInBaseUnits();

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Express_OneKilogramMeterPerSecondSquared_IsOneKilogramMeterPerSecondSquared()
    {
        Measurement oneKilogramMeterPerSecondSquared = new
        (
            quantity: 1.0,
            units: [
                SIBase.Kilogram(),
                SIBase.Meter(),
                SIBase.Second(-2),
            ]
        );

        Measurement expected = oneKilogramMeterPerSecondSquared;

        Measurement result = oneKilogramMeterPerSecondSquared.ExpressInBaseUnits();

        Assert.AreEqual(expected, result);
    }
}