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

            Assert.AreEqual(result.Unit.Unit, USCustomary.Units.Inch);
        }

        [TestMethod]
        public void UnitFactory_CorrectlyParsesKilometers()
        {
            UnitGroup oneKm = new UnitGroupBuilder()
                .WithSymbol("km")
                .Build();

            var result = new UnitFactory(oneKm)
                .BuildUnits()
                .First();

            Assert.AreEqual(result.Unit.Unit, SIBase.Units.Meter);
            Assert.AreEqual(result.Prefix.Prefix, SIPrefix.Prefixes.Kilo);
        }

        [TestMethod]
        public void UnitFactory_CorrectlyParsesKilometersPerSecondSquared()
        {
            // Arrange
            UnitGroup oneKm = new UnitGroupBuilder()
                .WithSymbol("km")
                .Build();

            UnitGroup perSecond = new UnitGroupBuilder()
                .WithSymbol("s")
                .WithExponent(1)
                .WithDivMulti(UnitGroup.UnitOperation.Divide)
                .Build();

            List<UnitGroup> unitGroups = [oneKm, perSecond, perSecond];

            List<SIBase.Units> correctUnits = [SIBase.Units.Meter, SIBase.Units.Second];
            List<int> correctExponents = [1, -2];
            List<SIPrefix.Prefixes> correctPrefixes = [SIPrefix.Prefixes.Kilo, SIPrefix.Prefixes._None];

            // Act
            var result = new UnitFactory(unitGroups)
                .BuildUnits();

            // Assert
            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(result[i].Unit.Unit, correctUnits[i]);
                Assert.AreEqual(result[i].Exponent, correctExponents[i]);
                Assert.AreEqual(result[i].Prefix.Prefix, correctPrefixes[i]);
            }
        }

        [TestMethod]
        public void UnitFactory_CorrectlyParses1mm()
        {
            UnitGroup oneMm = new UnitGroupBuilder().WithSymbol("mm").Build();

            var result = new UnitFactory(oneMm).BuildUnits().First();

            Assert.AreEqual(result.Unit.Unit, SIBase.Units.Meter);
            Assert.AreEqual(result.Prefix.Prefix, SIPrefix.Prefixes.Milli);
        }
    }
}
