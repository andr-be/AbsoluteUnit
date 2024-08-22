using AbsoluteUnit.Program;
using AbsoluteUnit.Program.Commands;
using AbsoluteUnit.Program.Structures;
using AbsoluteUnit.Program.Units;

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
            Assert.AreEqual(SIBase.Units.Meter, result.Units.First().UnitType.Unit, "should have simplified to a meter");
            
        }

        [TestMethod]
        [TestCategory("SimpleTests")]
        public void Simplify_OneNewton_ToOneKilogramMeterPerSecondSquare()
        {

        }

        private static CommandGroup CreateSimplifyCommandGroup(string startMeasurement, Dictionary<Flag, int>? flags = null)
            => new(Command.Simplify, flags ?? [], [startMeasurement]);
    }
}
