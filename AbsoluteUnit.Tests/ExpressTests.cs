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
            [
                Unit.OfType(SIBase.Units.Gram, SIPrefix.Prefixes.Kilo),
                Unit.OfType(SIBase.Units.Meter),
                Unit.OfType(SIBase.Units.Second, exponent:-2)
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
}