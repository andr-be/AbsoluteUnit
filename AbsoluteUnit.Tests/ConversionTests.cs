using AbsoluteUnit.Program;

namespace AbsoluteUnit.Tests
{
    [TestClass]
    public class ConversionTests
    {
        [TestMethod]
        public void Equals_OneMeterEqualsOneMeter()
        {
            var a = new AbsUnit(new SIBase(SIBase.Units.Meter));
            var b = new AbsUnit(new SIBase(SIBase.Units.Meter));

            Assert.AreEqual(a, b);
        }

        [TestMethod]
        public void Convert_ValidConversion_IsConsideredValid()
        {
            List<AbsUnit> metersPerSecond =
            [
                new AbsUnit(new SIBase(SIBase.Units.Meter)),
                new AbsUnit(new SIBase(SIBase.Units.Second), exponent: -1)
            ];

            List<AbsUnit> inchesPerMicrosecond =
            [
                new AbsUnit(new USCustomary(USCustomary.Units.Inch)),
                new AbsUnit(new SIBase(SIBase.Units.Second), exponent: -1)
            ];

            AbsMeasurement metersPerSecondMeasurement = new(metersPerSecond);
            AbsMeasurement inchesPerMicrosecondMeasurement = new(inchesPerMicrosecond);

            // Act
            var msToBase = MeasurementConverter.ToBaseMeasurement(metersPerSecondMeasurement);
            var inmsToBase = MeasurementConverter.ToBaseMeasurement(inchesPerMicrosecondMeasurement);

            // Assert
            Assert.IsTrue(MeasurementConverter.ValidConversion(msToBase, inmsToBase));
        }

        [TestMethod]
        public void Convert_InvalidConversion_IsNotConsideredValid()
        {
            List<AbsUnit> metersPerSecond =
            [
                new AbsUnit(new SIBase(SIBase.Units.Meter)),
                new AbsUnit(new SIBase(SIBase.Units.Second), exponent: -1)
            ];

            List<AbsUnit> poundsPerSecondSquared =
            [
                new AbsUnit(new USCustomary(USCustomary.Units.Pound)),
                new AbsUnit(new SIBase(SIBase.Units.Second), exponent: -2)
            ];

            AbsMeasurement metersPerSecondMeasurement = new(metersPerSecond);
            AbsMeasurement poundsPerSecondSquaredMeasurement = new(poundsPerSecondSquared);

            // Act
            var msToBase = MeasurementConverter.ToBaseMeasurement(metersPerSecondMeasurement);
            var lbs2ToBase = MeasurementConverter.ToBaseMeasurement(poundsPerSecondSquaredMeasurement);

            // Assert
            Assert.IsFalse(MeasurementConverter.ValidConversion(msToBase, lbs2ToBase));
        }

        [TestMethod]
        public void Convert_ConvertsOneFootToMeters_Correctly()
        {
            // Arrange
            var foot = new AbsUnit(new USCustomary(USCustomary.Units.Feet));
            var oneFoot = new AbsMeasurement(foot, 1);

            var meter = new AbsUnit(new SIBase(SIBase.Units.Meter));
            var expectedResult = new AbsMeasurement(meter, 0.3048);

            // Act
            var convertedFoot = MeasurementConverter.ToBaseMeasurement(oneFoot);

            // Assert
            Assert.AreEqual(expectedResult, convertedFoot);
        }
    }
}