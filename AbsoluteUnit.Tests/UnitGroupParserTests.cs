namespace AbsoluteUnit.Tests
{
    [TestClass]
    public class UnitGroupParserTests
    {
        [TestMethod]
        public void MeasurementGroup_GoodInput_SuccessfullyParsesCompoundUnit_Case1()
        {
            // Arrange
            string goodInput = "123.4e5 kg.m/s^2";

            UnitGroup unitKg = new UnitGroupBuilder()
                .WithDivMulti(UnitGroup.DivMulti.Multiply)
                .WithSymbol("kg")
                .WithExponent(1)
                .Build();

            UnitGroup unitM = new UnitGroupBuilder()
                .WithDivMulti(UnitGroup.DivMulti.Multiply)
                .WithSymbol("m")
                .WithExponent(1)
                .Build();
        
            UnitGroup unitS2 = new UnitGroupBuilder()
                .WithDivMulti(UnitGroup.DivMulti.Divide)
                .WithSymbol("s")
                .WithExponent(2)
                .Build();

            List<UnitGroup> unitGroups = [ unitKg, unitM, unitS2 ];

            // Act
            MeasurementParser testGroup = new(goodInput);

            // Assert
            Assert.AreEqual(testGroup.Quantity, 123.4);
            Assert.AreEqual(testGroup.Exponent, 5);
            Assert.IsTrue(testGroup.Units.Groups.SequenceEqual(unitGroups));
        }

        [TestMethod]
        public void MeasurementGroup_GoodInput_SuccessfullyParsesCompoundUnit_Case2()
        {
            // Arrange
            string goodInput = "69 m/s";

            UnitGroup unitM = new UnitGroupBuilder()
                .WithDivMulti(UnitGroup.DivMulti.Multiply)
                .WithSymbol("m")
                .WithExponent(1)
                .Build();

            UnitGroup unitS = new UnitGroupBuilder()
                .WithDivMulti(UnitGroup.DivMulti.Divide)
                .WithSymbol("s")
                .WithExponent(1)
                .Build();

            List<UnitGroup> unitGroups = [unitM, unitS];

            // Act
            MeasurementParser testGroup = new(goodInput);

            // Assert
            Assert.AreEqual(testGroup.Quantity, 69);
            Assert.AreEqual(testGroup.Exponent, 0);
            Assert.IsTrue(testGroup.Units.Groups.SequenceEqual(unitGroups));
        }

        [TestMethod]
        [ExpectedException(typeof(ParseError))]
        public void MeasurementGroup_NoUnitsInput_ThrowsParsingException()
        {
            // Arrange
            string noUnitsInput = "123";

            // Act
            MeasurementParser _ = new(noUnitsInput);
        }

        [TestMethod]
        [ExpectedException(typeof(ParseError))]
        public void MeasurementGroup_OnlyUnitInput_ThrowsParsingException()
        {
            // Arrange
            string onlyUnit = "kg.m/s^2";

            // Act
            MeasurementParser _ = new(onlyUnit);
        }

        [TestMethod]
        [ExpectedException(typeof(ParseError))]
        public void MeasurementGroup_FractionalExponentInput_ThrowsParsingException()
        {
            // Arrange
            string fractionalExponent = "123e4.5 kg.m/s^2";

            // Act
            MeasurementParser _ = new(fractionalExponent);
        }
    }

    public class UnitGroupBuilder
    {
        private UnitGroup.DivMulti _divMulti;
        private string _symbol;
        private int _exponent;

        public UnitGroupBuilder WithDivMulti(UnitGroup.DivMulti divMulti)
        {
            _divMulti = divMulti;
            return this;
        }

        public UnitGroupBuilder WithSymbol(string symbol)
        {
            _symbol = symbol;
            return this;
        }

        public UnitGroupBuilder WithExponent(int exponent)
        {
            _exponent = exponent;
            return this;
        }

        public UnitGroup Build()
        {
            return new UnitGroup(_divMulti, _symbol, _exponent);
        }
    }
}
