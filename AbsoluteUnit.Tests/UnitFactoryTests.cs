using AbsoluteUnit.Program.Factories;
using AbsoluteUnit.Program.Parsers.ParserGroups;
using AbsoluteUnit.Program.UnitTypes;

namespace AbsoluteUnit.Tests
{
    [TestClass]
    public class UnitFactoryTests
    {
        [TestMethod]
        public void UnitFactory_MetersSquared_CorrectlyBuilds()
        {
            // Arrange
            UnitGroup metersSquared = new UnitGroupBuilder()
                .WithDivMulti(UnitGroup.UnitOperation.Multiply)
                .WithSymbol("m")
                .WithExponent(2)
                .Build();

            // Act
            var parsedM2 = new UnitFactory().BuildUnits(metersSquared).First();

            // Assert
            Assert.AreEqual<object>(parsedM2.UnitType.Symbol, "m");
            Assert.AreEqual(parsedM2.Exponent, 2);
        }

        [TestMethod]
        public void UnitFactory_KilogramMetersPerSecondSquared_CorrectlyBuilds()
        {
            // Arrange
            UnitGroup kg = new UnitGroupBuilder()
                .WithSymbol("kg")
                .Build();

            UnitGroup m = new UnitGroupBuilder()
                .WithSymbol("m")
                .Build();

            UnitGroup s2 = new UnitGroupBuilder()
                .WithSymbol("s")
                .WithExponent(2)
                .WithDivMulti(UnitGroup.UnitOperation.Divide)
                .Build();

            List<UnitGroup> unitGroups = [kg, m, s2];

            var grams = new SIBase(SIBase.Units.Gram);
            var meters = new SIBase(SIBase.Units.Meter);
            var seconds = new SIBase(SIBase.Units.Second);

            // Act
            var result = new UnitFactory().BuildUnits(unitGroups);

            // Assert
            Assert.AreEqual(result[0].UnitType.UnitType, grams.UnitType, "kg should parse as Kilo && Gram");
            Assert.AreEqual(result[0].Prefix.Prefix, SIPrefix.Prefixes.Kilo, "kg should parse as Kilo && Gram");

            Assert.AreEqual(result[1].UnitType.UnitType, meters.UnitType, "m should parse as Meter");
            
            Assert.AreEqual(result[2].UnitType.UnitType, seconds.UnitType, "s should parse as Second");
            Assert.AreEqual(result[2].Exponent, -2, "divide + exp:2 should build as -2 exponent");
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
            var resultExponent = new UnitFactory()
                .BuildUnits(unitGroups)
                .Where(g => (SIBase.Units)g.UnitType.UnitType == SIBase.Units.Second)
                .First()
                .Exponent;

            // Assert
            Assert.AreEqual(resultExponent, -2, "s-1 x2 exponents should collate to -2");
        }

        [TestMethod]
        public void UnitFactory_CorrectlyBuildsInches()
        {
            UnitGroup oneInch = new UnitGroupBuilder()
                .WithSymbol("in")
                .Build();

            var result = new UnitFactory()
                .BuildUnits(oneInch)
                .First();

            Assert.AreEqual(result.UnitType.UnitType, USCustomary.Units.Inch, "in should parse as Inch");
        }

        [TestMethod]
        public void UnitFactory_CorrectlyBuildsKilometers()
        {
            UnitGroup oneKm = new UnitGroupBuilder()
                .WithSymbol("km")
                .Build();

            var result = new UnitFactory()
                .BuildUnits(oneKm)
                .First();

            Assert.AreEqual(result.UnitType.UnitType, SIBase.Units.Meter, "km should parse as Kilo && Meter");
            Assert.AreEqual(result.Prefix.Prefix, SIPrefix.Prefixes.Kilo, "km should parse as Kilo && Meter");
        }

        [TestMethod]
        public void UnitFactory_CorrectlyBuildsKilometersPerSecondSquared()
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
            var result = new UnitFactory()
                .BuildUnits(unitGroups);

            // Assert
            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(result[i].UnitType.UnitType, correctUnits[i], 
                    $"{result[i].UnitType.UnitType} should equal {correctUnits[i]}");

                Assert.AreEqual(result[i].Exponent, correctExponents[i], 
                    $"{result[i].Exponent} should equal {correctExponents[i]}");

                Assert.AreEqual(result[i].Prefix.Prefix, correctPrefixes[i], 
                    $"{result[i].Prefix.Prefix} should equal {correctPrefixes[i]}");
            }
        }

        [TestMethod]
        public void UnitFactory_CorrectlyBuilds1mm()
        {
            UnitGroup oneMm = new UnitGroupBuilder().WithSymbol("mm").Build();

            var result = new UnitFactory().BuildUnits([oneMm]).First();

            Assert.AreEqual(result.UnitType.UnitType, SIBase.Units.Meter);
            Assert.AreEqual(result.Prefix.Prefix, SIPrefix.Prefixes.Milli);
        }
    }
}
