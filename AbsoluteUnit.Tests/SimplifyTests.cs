using AbsoluteUnit.Program;
using AbsoluteUnit.Program.Commands;
using AbsoluteUnit.Program.Parsers.ParserGroups;
using AbsoluteUnit.Program.Structures;
using AbsoluteUnit.Program.UnitTypes;

namespace AbsoluteUnit.Tests
{
    [TestClass]
    public class SimplifyTests
    {
        
        [TestMethod]
        [TestCategory("SimpleTests")]
        public void Simplify_OneMeter_ToOneMeter()
        {
            // Arrange
            var calculator = new Calculator(CreateSimplifyCommandGroup("1 m"))
                .ParseCommandArguments(ParserFactory.CreateMeasurementParser());
            
            // Act
            var result = calculator.Calculate();

            // Assert
            Assert.AreEqual(SIBase.Units.Meter, result[0].Units.First().UnitType.UnitType, "should have simplified to a meter");
            
        }

        [TestMethod]
        [TestCategory("SimpleTests")]
        public void Simplify_OneKilogramMeterPerSecondSquare_ToOneNewton()
        {
            var calculator = new Calculator(CreateSimplifyCommandGroup("1 kg.m.s^-2"))
                .ParseCommandArguments(ParserFactory.CreateMeasurementParser());

            var expectedResult = new Measurement([Unit.OfType(SIDerived.Units.Newton)], quantity: 1);

            var result = calculator.Calculate();

            Assert.AreEqual<Measurement>(expectedResult, result[0]);
        }

        [TestMethod]
        [TestCategory("ComplexTests")]
        public void Simplify_OneThousandKilogramMetersPerSecondSquare_ToOneKilonewton()
        {
            var calculator = new Calculator(CreateSimplifyCommandGroup("1000 kg.m.s^-2"))
                .ParseCommandArguments(ParserFactory.CreateMeasurementParser());

            var expected = new Measurement
            (
                units: [Unit.OfType(SIDerived.Units.Newton, SIPrefix.Prefixes.Kilo)], 
                quantity: 1
            );

            var result = calculator.Calculate();

            Assert.AreEqual(expected, result.First(), message: "1000kgm/s^2 should be 1kN!");
        }

        [TestMethod]
        [TestCategory("ComplexTests")]
        public void Simplify_1eMinus3Meters_ToOneMillimeter()
        {
            var calculator = new Calculator(CreateSimplifyCommandGroup("1e-3 m"))
                .ParseCommandArguments(ParserFactory.CreateMeasurementParser());

            var expected = new Measurement
            (
                units: [Unit.OfType(SIBase.Units.Meter, SIPrefix.Prefixes.Milli)],
                quantity: 1
            );

            var result = calculator.Calculate();

            Assert.AreEqual(expected, result.First(), message: "1e-3m should be 1mm!");
        }

        [TestMethod]
        [TestCategory("DerivedUnits")]
        public void Simplify_KilogramMeterSquaredPerSecondCubed_ToOneWatt()
        {
            var calculator = new Calculator(CreateSimplifyCommandGroup("1 kg.m^2.s^-3"))
                .ParseCommandArguments(ParserFactory.CreateMeasurementParser());

            var expected = new Measurement(
                units: [Unit.OfType(SIDerived.Units.Watt)],
                quantity: 1
            );

            var result = calculator.Calculate();

            Assert.AreEqual(expected, result.First(), "kg⋅m²⋅s⁻³ should simplify to W");
        }

        [TestMethod]
        [TestCategory("DerivedUnits")]
        public void Simplify_NewtonPerMeterSquared_ToPascal()
        {
            var calculator = new Calculator(CreateSimplifyCommandGroup("1 N.m^-2"))
                .ParseCommandArguments(ParserFactory.CreateMeasurementParser());

            var expected = new Measurement(
                units: [Unit.OfType(SIDerived.Units.Pascal)],
                quantity: 1
            );

            var result = calculator.Calculate();

            Assert.AreEqual(expected, result.First(), "N/m² should simplify to Pa");
        }

        [TestMethod]
        [TestCategory("UnitCancellation")]
        public void Simplify_MeterPerMeter_ToOne()
        {
            var calculator = new Calculator(CreateSimplifyCommandGroup("1 m.m^-1"))
                .ParseCommandArguments(ParserFactory.CreateMeasurementParser());

            var expected = new Measurement(
                units: [],
                quantity: 1
            );

            var result = calculator.Calculate();

            Assert.AreEqual(expected, result.First(), "m/m should cancel to dimensionless");
        }

        [TestMethod]
        [TestCategory("ComplexPrefixes")]
        public void Simplify_MicroNewtonKiloMeter_ToMilliJoule()
        {
            var calculator = new Calculator(CreateSimplifyCommandGroup("1 µN.km"))
                .ParseCommandArguments(ParserFactory.CreateMeasurementParser());

            var expected = new Measurement(
                units: [Unit.OfType(SIDerived.Units.Joule, SIPrefix.Prefixes.Milli)],
                quantity: 1
            );

            var result = calculator.Calculate();

            Assert.AreEqual(expected, result.First(), "µN⋅km should simplify to mJ");
        }

        [TestMethod]
        [TestCategory("ElectricalUnits")]
        public void Simplify_VoltAmpere_ToWatt()
        {
            var calculator = new Calculator(CreateSimplifyCommandGroup("1 V.A"))
                .ParseCommandArguments(ParserFactory.CreateMeasurementParser());

            var expected = new Measurement(
                units: [Unit.OfType(SIDerived.Units.Watt)],
                quantity: 1
            );

            var result = calculator.Calculate();

            Assert.AreEqual(expected, result.First(), "V⋅A should simplify to W");
        }

        [TestMethod]
        [TestCategory("ComplexCancellation")]
        public void Simplify_NewtonMeterPerJoule_ToOne()
        {
            var calculator = new Calculator(CreateSimplifyCommandGroup("1 N.m.J^-1"))
                .ParseCommandArguments(ParserFactory.CreateMeasurementParser());

            var expected = new Measurement(
                units: [],
                quantity: 1
            );

            var result = calculator.Calculate();

            Assert.AreEqual(expected, result.First(), "N⋅m/J should cancel to dimensionless");
        }

        [TestMethod]
        [TestCategory("ExponentHandling")]
        public void Simplify_MeterToFourth_StaysMeterToFourth()
        {
            var calculator = new Calculator(CreateSimplifyCommandGroup("1 m^4"))
                .ParseCommandArguments(ParserFactory.CreateMeasurementParser());

            var expected = new Measurement(
                units: [Unit.OfType(SIBase.Units.Meter, exponent: 4)],
                quantity: 1
            );

            var result = calculator.Calculate();

            Assert.AreEqual(expected, result.First(), "m⁴ should stay as m⁴");
        }

        [TestMethod]
        [TestCategory("QuantityHandling")]
        public void Simplify_VeryLargeNumber_UsesCorrectPrefix()
        {
            var calculator = new Calculator(CreateSimplifyCommandGroup("1e9 N"))
                .ParseCommandArguments(ParserFactory.CreateMeasurementParser());

            var expected = new Measurement(
                units: [Unit.OfType(SIDerived.Units.Newton, SIPrefix.Prefixes.Giga)],
                quantity: 1
            );

            var result = calculator.Calculate();

            Assert.AreEqual(expected, result.First(), "1e9 N should be 1 GN");
        }

        [TestMethod]
        [TestCategory("EdgeCases")]
        public void Simplify_ZeroQuantity_PreservesUnits()
        {
            var calculator = new Calculator(CreateSimplifyCommandGroup("0 kg.m.s^-2"))
                .ParseCommandArguments(ParserFactory.CreateMeasurementParser());

            var expected = new Measurement(
                units: [Unit.OfType(SIDerived.Units.Newton)],
                quantity: 0
            );

            var result = calculator.Calculate();

            Assert.AreEqual(expected, result.First(), "0 kg⋅m⋅s⁻² should be 0 N");
        }

        private static CommandGroup CreateSimplifyCommandGroup(string startMeasurement, Dictionary<Flag, int>? flags = null)
            => new(Command.Simplify, flags ?? [], [startMeasurement]);
    }
}
