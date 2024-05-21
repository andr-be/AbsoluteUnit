using AbsoluteUnit.Program;

namespace AbsoluteUnit.Tests
{
    [TestClass]
    internal class UnitParserTests
    {
        [TestMethod]
        public void UnitConstructor_GivenMetersSquared_ReturnsSIBaseUnit()
        {
            // Arrange
            UnitGroup metersSquared = new UnitGroupBuilder()
                .WithDivMulti(UnitGroup.UnitOperation.Multiply)
                .WithSymbol("m")
                .WithExponent(2)
                .Build();

            // Act
            Unit parsedm2 = new(metersSquared);

            // Assert
            Assert.AreEqual<object>(parsedm2.Base, SI_Base.Unit.Meter);
        }
    }
}
