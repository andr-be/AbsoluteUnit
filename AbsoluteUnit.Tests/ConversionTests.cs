using AbsoluteUnit.Program;
using AbsoluteUnit.Program.Units;
using AbsoluteUnit.Program.Factories;
using AbsoluteUnit.Program.Commands;

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
            (
                quantity: 5918.2,
                units: [
                    new AbsUnit(new SIBase("m")),
                    new AbsUnit(new SIBase("s"), exponent: -1)
                ]
            );

            var inchesPerMicroSecond = CreateMeasurement
            (
                quantity: 0.2330,
                units: [
                    new AbsUnit(new USCustomary(USCustomary.Units.Inch)),
                    new AbsUnit(new SIBase("s"), exponent: -1)
                ]
            );

            // Act
            var isValidForwards = MeasurementConverter.IsValidConversion(metersPerSecond, inchesPerMicroSecond);
            var isValidBackwards = MeasurementConverter.IsValidConversion(inchesPerMicroSecond, metersPerSecond);

            // Assert
            Assert.IsTrue(isValidForwards && isValidBackwards);
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
            var isValid = MeasurementConverter.IsValidConversion(metersPerSecond, poundsPerSecondSquared);

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
            var convertedFoot = MeasurementConverter.ExpressInBaseUnits(oneFoot);

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
                    new AbsUnitBuilder()
                        .WithUnit(new USCustomary(USCustomary.Units.Inch))
                        .Build(),

                    new AbsUnitBuilder()
                        .WithUnit(new SIBase("s"))
                        .WithExponent(-1)
                        .WithPrefix(new SIPrefix(SIPrefix.Prefixes.Micro))
                        .Build()
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
            var convertedUnit = MeasurementConverter.ExpressInBaseUnits(_02330InchesPerMicroSecond);

            // Assert
            Assert.AreEqual
            (
                expected:   _5918MetersPerSecond.Quantity, 
                actual:     convertedUnit.Quantity, 
                delta:      1e-12, 
                $"actual: {convertedUnit} .. expected: {_5918MetersPerSecond} .. (delta {_5918MetersPerSecond.Quantity - convertedUnit.Quantity})"
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