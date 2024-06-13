using AbsoluteUnit.Program;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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
        public void Convert_Converts1fToMeters_Correctly()
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