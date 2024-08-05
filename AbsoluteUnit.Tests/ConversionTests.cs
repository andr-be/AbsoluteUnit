using AbsoluteUnit.Program.Units;
using AbsoluteUnit.Program.Factories;
using AbsoluteUnit.Program.Commands;
using AbsoluteUnit.Program.Structures;

namespace AbsoluteUnit.Tests
{
    [TestClass]
    public class ConversionTests
    {
        [TestMethod]
        public void Equals_OneMeterEqualsOneMeter()
        {
            var a = UnitFactory.Meter();
            var b = UnitFactory.Meter();

            Assert.AreEqual(a, b);
        }

        [TestMethod]
        public void Convert_MetersPerSecondToInchesPerMicrosecond_IsValidConversion()
        {
            var metersPerSecond = CreateMeasurement
            (
                quantity: 5918.2,
                units: [
                    UnitFactory.Meter(),
                    UnitFactory.Second(-1)
                ]
            );

            var inchesPerMicroSecond = CreateMeasurement
            (
                quantity: 0.2330,
                units: [
                    new TestUnitBuilder()
                        .WithUnit(USCustomary.Inch())
                        .Build(),
                    
                    new TestUnitBuilder()
                        .WithUnit(new SIBase(SIBase.Units.Second))
                        .WithPrefix(SIPrefix.Prefixes.Micro)
                        .WithExponent(-1)
                        .Build(),
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
            (
                [
                    UnitFactory.Meter(),
                    UnitFactory.Second(-1)
                ]
            );

            var poundsPerSecondSquared = CreateMeasurement
            (
                [
                    new TestUnitBuilder()
                        .WithUnit(new USCustomary(USCustomary.Units.Pound))
                        .Build(),

                    UnitFactory.Second(-2)
                ]
            );

            // Act
            var isValid = MeasurementConverter.IsValidConversion(metersPerSecond, poundsPerSecondSquared);

            // Assert
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void Convert_ConvertsOneFootToMeters_Correctly()
        {
            // Arrange
            var oneFoot = CreateMeasurement
            (
                quantity: 1,
                units: [
                    new TestUnitBuilder()
                        .WithUnit(USCustomary.Feet())
                        .Build()
                ]
            );

            var expectedResult = CreateMeasurement([UnitFactory.Meter()], 0.3048);

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
                quantity: 0.2330,
                units: [
                    new TestUnitBuilder()
                        .WithUnit(USCustomary.Inch())
                        .Build(),

                    new TestUnitBuilder()
                        .WithUnit(new SIBase("s"))
                        .WithExponent(-1)
                        .WithPrefix(SIPrefix.Prefixes.Micro)
                        .Build()
                ]
                    
            );

            var expected = CreateMeasurement
            (
                quantity: 5918.2,
                units: [
                    UnitFactory.Meter(),
                    UnitFactory.Second(-1)
                ]
            );

            // Act
            var convertedUnit = MeasurementConverter.ExpressInBaseUnits(_02330InchesPerMicroSecond);

            // Assert
            Assert.AreEqual
            (
                expected:   expected.Quantity, 
                actual:     convertedUnit.Quantity, 
                delta:      1e-12, 
                $"actual: {convertedUnit} .. expected: {expected} .. (delta {expected.Quantity - convertedUnit.Quantity})"
            );
        }

        [TestMethod]
        public void Convert_OneTonIntoKilograms_Correctly()
        {
            var expected = CreateMeasurement([UnitFactory.Kilogram()], 907.18474);

            var oneTon = CreateMeasurement
            (
                [
                    new TestUnitBuilder()
                        .WithUnit(new USCustomary(USCustomary.Units.Ton))
                        .Build()
                ]
            );

            // Act
            var actual = MeasurementConverter.ConvertMeasurement(oneTon, expected);

            // Assert
            AssertWithConfidence(expected, actual);
        }

        [TestMethod]
        public void Convert_OneKilogramInto2pt2Pounds_Correctly()
        {
            var kilogram = new Measurement(UnitFactory.Kilogram(), 1);

            var expectedValue = CreateMeasurement
            (
                quantity: 2.20462262,
                units: [
                    new TestUnitBuilder()
                        .WithUnit(USCustomary.Pound())
                        .Build()
                ]
            );

            // At
            var actualValue = MeasurementConverter.ConvertMeasurement(kilogram, expectedValue);

            // Assert
            AssertWithConfidence(expectedValue, actualValue);
        }

        private static Measurement CreateMeasurement(
            List<Unit> units,
            double quantity = 1.0,
            int exponent = 1
            ) => 
                new(units, quantity, exponent);

        private static void AssertWithConfidence(Measurement expected, Measurement actual) => Assert.AreEqual
        (
            expected: expected.Quantity,
            actual: actual.Quantity,
            delta: 1e-12,
            $"actual: {actual} .. expected: {expected} .. (delta {expected.Quantity - actual.Quantity})"
        );
    }
}