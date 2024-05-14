namespace AbsoluteUnit.Tests
{
    [TestClass]
    public class MeasurementParserTests
    {
        [TestMethod]
        public void MeasurementParser_ValidSimpleInput_SuccessfullyParses()
        {
            // Arrange
            var simpleMeasurement = "1m";
            UnitGroup unitM = new UnitGroupBuilder()
                .WithDivMulti(UnitGroup.DivMulti.Multiply)
                .WithSymbol("m")
                .WithExponent(1)
                .Build();

            // Act
            MeasurementParser measurement = new(simpleMeasurement);

            // Assert
            Assert.AreEqual(unitM, measurement.Units.Groups.FirstOrDefault());
        }
        
        [TestMethod]
        public void MeasurementParser_ValidExponentInput_SuccessfullyParses()
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
        public void MeasurementParser_ValidNonExponentUnit_SuccessfullyParses()
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
        public void MeasurementParser_NoQuantityWithExponent_ThrowsParseError()
        {
            // Arrange
            string noQuantityWithExponent = "e5 kg";

            // Act
            MeasurementParser _ = new(noQuantityWithExponent);
        }

        [TestMethod]
        [ExpectedException(typeof(ParseError))]
        public void MeasurementParser_NoUnitsWithExponent_ThrowsParseError()
        {
            // Arrange
            string noUnitsExponentInput = "123e4";

            // Act
            MeasurementParser _ = new(noUnitsExponentInput);
        }        
        
        [TestMethod]
        [ExpectedException(typeof(ParseError))]
        public void MeasurementParser_NoUnitsWithoutExponent_ThrowsParseError()
        {
            // Arrange
            string noUnitsInput = "123";

            // Act
            MeasurementParser _ = new(noUnitsInput);
        }

        [TestMethod]
        [ExpectedException(typeof(ParseError))]
        public void MeasurementParser_OnlyUnitInput_ThrowsParseError()
        {
            // Arrange
            string onlyUnit = "kg.m/s^2";

            // Act
            MeasurementParser _ = new(onlyUnit);
        }

        [TestMethod]
        [ExpectedException(typeof(ParseError))]
        public void MeasurementParser_FractionalExponentInput_ThrowsParseError()
        {
            // Arrange
            string fractionalExponent = "123e4.5 kg.m/s^2";

            // Act
        MeasurementParser _ = new(fractionalExponent);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MeasurementParser_BlankString_ThrowsParseError()
        {
            // Arrange
            string blankString = "";

            // Act
            MeasurementParser _ = new(blankString); 
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
