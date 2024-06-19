using AbsoluteUnit.Program;

namespace AbsoluteUnit.Tests
{
    [TestClass]
    public class ConversionTests
    {
        [TestMethod]
        public void Equals_OneMeterEqualsOneMeter()
        {
            var a = new AbsUnit(new SIBase("m"));
            var b = new AbsUnit(new SIBase("m"));

            Assert.AreEqual(a, b);
        }

        [TestMethod]
        public void Convert_MetersPerSecondToInchesPerMicrosecond_IsValidConversion()
        {
            var metersPerSecond = CreateMeasurement
            ([
                new AbsUnit(new SIBase("m")),
                new AbsUnit(new SIBase("s"), exponent: -1)
            ]);

            var inchesPerMicroSecond = CreateMeasurement
            ([
                new AbsUnit(new USCustomary(USCustomary.Units.Inch)),
                new AbsUnit(new SIBase("s"), exponent: -1)
            ]);

            // Act
            var isValid = MeasurementConverter.ValidConversion(metersPerSecond, inchesPerMicroSecond);

            // Assert
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void Convert_MetersPerSecondToPoundsPerSecondSquared_IsNotValidConversion()
        {
            // Arrange
            var metersPerSecond = CreateMeasurement
            ([
                new AbsUnit(new SIBase("m")),
                new AbsUnit(new SIBase("s"), exponent: -1)
            ]);

            var poundsPerSecondSquared = CreateMeasurement
            ([
                new AbsUnit(new USCustomary(USCustomary.Units.Pound)),
                new AbsUnit(new SIBase("s"), exponent: -2)
            ]);

            // Act
            var isValid = MeasurementConverter.ValidConversion(metersPerSecond, poundsPerSecondSquared);

            // Assert
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void Convert_ConvertsOneFootToMeters_Correctly()
        {
            // Arrange
            var foot = new AbsUnit(new USCustomary(USCustomary.Units.Feet));
            var oneFoot = new AbsMeasurement(foot, 1);

            var meter = new AbsUnit(new SIBase("m"));
            var expectedResult = new AbsMeasurement(meter, 0.3048);

            // Act
            var convertedFoot = MeasurementConverter.ToBaseMeasurement(oneFoot);

            // Assert
            Assert.AreEqual(expectedResult, convertedFoot);
        }

        [TestMethod]
        public void Convert_02330InchesPerMicrosecondTo5918MetersPerSecond_Correctly()
        {
            // Arrange
            var _02330InchesPerMicroSecond = CreateMeasurement
            (
                [   
                    new AbsUnit(new USCustomary(USCustomary.Units.Inch)),
                    new AbsUnit(new SIBase("s"), 
                                exponent: -1, 
                                prefix: new(SIPrefix.Prefixes.Micro)) 
                ],
                quantity: 0.2330
            );

            var _5918MetersPerSecond = CreateMeasurement
            (
                [
                    new AbsUnit(new SIBase("m")),
                    new AbsUnit(new SIBase("s"), 
                                exponent: -1)
                ], 
                quantity: 5918.2
            );

            // Act
            var convertedUnit = MeasurementConverter.ToBaseMeasurement(_02330InchesPerMicroSecond);

            // Assert
            Assert.AreEqual
            (
                expected:   _5918MetersPerSecond.Quantity, 
                actual:     convertedUnit.Quantity, 
                delta:      1e-12, 
                $"{convertedUnit} != {_5918MetersPerSecond}"
            );
        }

        private static AbsMeasurement CreateMeasurement(
            List<AbsUnit> units,
            double quantity = 1.0,
            int exponent = 1
            ) => 
                new(units, quantity, exponent);
    }
}