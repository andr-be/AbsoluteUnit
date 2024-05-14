namespace AbsoluteUnit.Tests
{
    [TestClass]
    public class UnitGroupTests
    {
        [TestMethod]
        public void MeasurementGroup_GivenGoodInput_SuccessfullyParses()
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
            MeasurementGroup testGroup = new(goodInput);

            // Assert
            Assert.AreEqual(testGroup.Quantity, 123.4);
            Assert.AreEqual(testGroup.Exponent, 5);
            Assert.IsTrue(testGroup.Units.Groups.SequenceEqual(unitGroups));
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
