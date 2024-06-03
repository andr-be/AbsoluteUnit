using AbsoluteUnit.Program;

namespace AbsoluteUnit.Tests
{
    [TestClass]
    public class UnitParserTests
    {
        [TestMethod]
        public void UnitFactory_MetersSquared_CorrectlyParses()
        {
            // Arrange
            UnitGroup metersSquared = new UnitGroupBuilder()
                .WithDivMulti(UnitGroup.UnitOperation.Multiply)
                .WithSymbol("m")
                .WithExponent(2)
                .Build();

            // Act
            var parsedm2 = new UnitFactory(metersSquared).BuildUnits().First();

            // Assert
            Assert.AreEqual<object>(parsedm2.Unit.Symbol, "m");
            Assert.AreEqual(parsedm2.Exponent, 2);
        }

        [TestMethod]
        public void UnitFactory_MetersPerSecondSquared_CorrectlyCollatesExponents()
        {
            // Arrange
            UnitGroup meters = new UnitGroupBuilder()
                .WithSymbol("m")
                .Build();

            UnitGroup perSecond = new UnitGroupBuilder()
                .WithDivMulti(UnitGroup.UnitOperation.Divide)
                .WithSymbol("s")
                .Build();

            List<UnitGroup> unitGroups = [meters, perSecond, perSecond];

            // Act
            var resultExponent = new UnitFactory(unitGroups)
                .BuildUnits()
                .Where(g => g.Unit.Symbol == "s")
                .First()
                .Exponent;

            // Assert
            Assert.AreEqual(resultExponent, -2);
        }

        [TestMethod]
        public void UnitFactory_CorrectlyParsesVariousUSCustomaryUnits()
        {
            UnitGroup oneInch = new UnitGroupBuilder()
                .WithSymbol("in")
                .Build();

            var result = new UnitFactory(oneInch)
                .BuildUnits()
                .First();

            Assert.AreEqual<USCustomary.Units>((USCustomary.Units)result.Unit.Unit, USCustomary.Units.Inch);
        }
    }
}
