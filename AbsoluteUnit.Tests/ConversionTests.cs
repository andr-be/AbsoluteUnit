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
            var a = SIBase.Meter();
            var b = SIBase.Meter();

            Assert.AreEqual(a, b);
        }

        [TestMethod]
        public void Convert_MetersPerSecondToInchesPerMicrosecond_IsValidConversion()
        {
            var metersPerSecond = CreateMeasurement
            (
                quantity: 5918.2,
                units: [
                    SIBase.Meter(),
                    SIBase.Second(-1)
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
            var isValidForwards = metersPerSecond.IsValidConversion(target: inchesPerMicroSecond);
            var isValidBackwards = inchesPerMicroSecond.IsValidConversion(target: metersPerSecond);

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
                    SIBase.Meter(),
                    SIBase.Second(-1)
                ]
            );

            var poundsPerSecondSquared = CreateMeasurement
            (
                [
                    new TestUnitBuilder()
                        .WithUnit(new USCustomary(USCustomary.Units.Pound))
                        .Build(),

                    SIBase.Second(-2)
                ]
            );

            // Act
            var isValid = metersPerSecond.IsValidConversion(target: poundsPerSecondSquared);

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

            var expectedResult = CreateMeasurement([SIBase.Meter()], 0.3048);

            // Act
            var convertedFoot = oneFoot.ExpressInBaseUnits();

            // Assert
            AssertEqualityWithConfidence(expectedResult, convertedFoot);
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
                    SIBase.Meter(),
                    SIBase.Second(-1)
                ]
            );

            // Act
            var convertedUnit = _02330InchesPerMicroSecond.ExpressInBaseUnits();

            // Assert
            AssertEqualityWithConfidence(expected, convertedUnit);
        }

        [TestMethod]
        public void Convert_OneTonIntoKilograms_Correctly()
        {
            var expected = CreateMeasurement([SIBase.Kilogram()], 907.18474);

            var oneTon = CreateMeasurement
            ([
                new TestUnitBuilder()
                    .WithUnit(new USCustomary(USCustomary.Units.Ton))
                    .Build()
            ]);

            // Act
            var actual = oneTon.ConvertTo(expected);

            // Assert
            AssertEqualityWithConfidence(expected, actual);
        }

        [TestMethod]
        public void Convert_OneKilogramInto2pt2Pounds_Correctly()
        {
            var kilogram = new Measurement(SIBase.Kilogram(), 1);

            var expectedValue = CreateMeasurement
            (
                quantity: 2.20462262,
                units: [
                    new TestUnitBuilder()
                        .WithUnit(USCustomary.Pound())
                        .Build()
                ]
            );

            // Act
            var actualValue = kilogram.ConvertTo(expectedValue);

            // Assert
            AssertEqualityWithConfidence(expectedValue, actualValue);
        }

        [TestMethod]
        public void Convert_OneHourInto3600Seconds_Correctly()
        {
            // Arrange
            var oneHour = CreateMeasurement
            (
                quantity: 1.0,
                units: [
                    new TestUnitBuilder()
                    .WithUnit(new Miscellaneous(Miscellaneous.Units.Hour))
                    .Build()
                ]
            );

            var expectedValue = CreateMeasurement
            (
                quantity: 3600.0,
                units: [ SIBase.Second() ]
            );

            // Act
            var actualValue = oneHour.ConvertTo(expectedValue);

            // Assert
            AssertEqualityWithConfidence(expectedValue, actualValue);
        }

        [TestMethod]
        public void Convert3600SecondsIntoOneHour_Correctly()
        {
            var _3600Seconds = CreateMeasurement
            (
                quantity: 3600.0,
                units: [SIBase.Second()]
            );

            var expectedValue = CreateMeasurement
            (
                quantity: 1.0,
                units: [
                    new TestUnitBuilder()
                    .WithUnit(new Miscellaneous(Miscellaneous.Units.Hour))
                    .Build()
                ]
            );

            // Act
            var actualValue = _3600Seconds.ConvertTo(expectedValue);

            // Assert
            AssertEqualityWithConfidence(expectedValue, actualValue);
        }

        private static Measurement CreateMeasurement(
            List<Unit> units,
            double quantity = 1.0,
            int exponent = 1
            ) => 
                new(units, quantity, exponent);

        private static void AssertEqualityWithConfidence(Measurement expected, Measurement actual) => Assert.AreEqual
        (
            expected: expected.Quantity,
            actual: actual.Quantity,
            delta: 1e-12,
            $"actual: {actual} .. expected: {expected} .. (delta {expected.Quantity - actual.Quantity})"
        );
    }
}