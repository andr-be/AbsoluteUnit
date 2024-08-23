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

        private static CommandGroup CreateSimplifyCommandGroup(string startMeasurement, Dictionary<Flag, int>? flags = null)
            => new(Command.Simplify, flags ?? [], [startMeasurement]);
    }
}
