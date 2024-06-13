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
        public void Convert_MetersPerSecondToInchesPerMicrosecond_IsValidConversion()
        {
            var metersPerSecond = CreateMeasurement
            (
                new AbsUnit(new SIBase(SIBase.Units.Meter)),
                new AbsUnit(new SIBase(SIBase.Units.Second), exponent: -1)
            );

            var inchesPerMicroSecond = CreateMeasurement
            (
                new AbsUnit(new USCustomary(USCustomary.Units.Inch)),
                new AbsUnit(new SIBase(SIBase.Units.Second), exponent: -1)
            );

            // Act
            var msToBase = MeasurementConverter.ToBaseMeasurement(metersPerSecond);
            var inµsToBase = MeasurementConverter.ToBaseMeasurement(inchesPerMicroSecond);

            // Assert
            Assert.IsTrue(MeasurementConverter.ValidConversion(msToBase, inµsToBase));
        }

        [TestMethod]
        public void Convert_MetersPerSecondToPoundsPerSecondSquared_IsNotValidConversion()
        {
            // Arrange
            var metersPerSecond = CreateMeasurement
            (
                new AbsUnit(new SIBase(SIBase.Units.Meter)),
                new AbsUnit(new SIBase(SIBase.Units.Second), exponent: -1)
            );

            var poundsPerSecondSquared = CreateMeasurement
            (
                new AbsUnit(new USCustomary(USCustomary.Units.Pound)),
                new AbsUnit(new SIBase(SIBase.Units.Second), exponent: -2)
            );

            // Act
            var msToBase = MeasurementConverter.ToBaseMeasurement(metersPerSecond);
            var lbs2ToBase = MeasurementConverter.ToBaseMeasurement(poundsPerSecondSquared);

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

        private static AbsMeasurement CreateMeasurement(params AbsUnit[] units) => new([.. units]);
    }
}