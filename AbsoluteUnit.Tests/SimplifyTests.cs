using AbsoluteUnit.Program;
using AbsoluteUnit.Program.Commands;
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

        private static CommandGroup CreateSimplifyCommandGroup(string startMeasurement, Dictionary<Flag, int>? flags = null)
            => new(Command.Simplify, flags ?? [], [startMeasurement]);
    }
}
