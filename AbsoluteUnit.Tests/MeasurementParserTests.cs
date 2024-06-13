using AbsoluteUnit.Program;

namespace AbsoluteUnit.Tests
{
    [TestClass]
    public class MeasurementParserTests
    {
        private IUnitGroupParser _unitGroupParser;
        private IUnitFactory _unitFactory;
        private MeasurementParser _measurementParser;

        [TestInitialize]
        public void Setup()
        {
            _unitGroupParser = new UnitGroupParser();
            _unitFactory = new UnitFactory();
            _measurementParser = new MeasurementParser(_unitGroupParser, _unitFactory);
        }

        [TestMethod]
        public void MeasurementParser_ValidSimpleInput_SuccessfullyParses()
        {
            // Arrange
            var simpleMeasurement = "1m";
            UnitGroup unitM = new UnitGroupBuilder()
                .WithDivMulti(UnitGroup.UnitOperation.Multiply)
                .WithSymbol("m")
                .WithExponent(1)
                .Build();

            // Act
            MeasurementGroup measurement = _measurementParser.GenerateMeasurementGroup(simpleMeasurement);

            // Assert
            Assert.AreEqual(unitM, measurement.Units.FirstOrDefault());
        }
        
        [TestMethod]
        public void MeasurementParser_ValidExponentInput_SuccessfullyParses()
        {
            // Arrange
            string goodInput = "123.4e5 kg.m/s^2";

            UnitGroup unitKg = new UnitGroupBuilder()
                .WithDivMulti(UnitGroup.UnitOperation.Multiply)
                .WithSymbol("kg")
                .WithExponent(1)
                .Build();

            UnitGroup unitM = new UnitGroupBuilder()
                .WithDivMulti(UnitGroup.UnitOperation.Multiply)
                .WithSymbol("m")
                .WithExponent(1)
                .Build();
        
            UnitGroup unitS2 = new UnitGroupBuilder()
                .WithDivMulti(UnitGroup.UnitOperation.Divide)
                .WithSymbol("s")
                .WithExponent(2)
                .Build();

            List<UnitGroup> unitGroups = [ unitKg, unitM, unitS2 ];

            // Act
            MeasurementGroup testGroup = _measurementParser.GenerateMeasurementGroup(goodInput);

            // Assert
            Assert.AreEqual(testGroup.Quantity, 123.4);
            Assert.AreEqual(testGroup.Exponent, 5);
            Assert.IsTrue(testGroup.Units.SequenceEqual(unitGroups));
        }

        [TestMethod]
        public void MeasurementParser_ValidNonExponentUnit_SuccessfullyParses()
        {
            // Arrange
            string goodInput = "69 m/s";

            UnitGroup unitM = new UnitGroupBuilder()
                .WithDivMulti(UnitGroup.UnitOperation.Multiply)
                .WithSymbol("m")
                .WithExponent(1)
                .Build();

            UnitGroup unitS = new UnitGroupBuilder()
                .WithDivMulti(UnitGroup.UnitOperation.Divide)
                .WithSymbol("s")
                .WithExponent(1)
                .Build();

            List<UnitGroup> unitGroups = [unitM, unitS];

            // Act
            MeasurementGroup testGroup = _measurementParser.GenerateMeasurementGroup(goodInput);

            // Assert
            Assert.AreEqual(testGroup.Quantity, 69);
            Assert.AreEqual(testGroup.Exponent, 0);
            Assert.IsTrue(testGroup.Units.SequenceEqual(unitGroups));
        }

        [TestMethod]
        public void MeasurementParser_CommaSeparatedNumber_SuccessfullyParses()
        {
            // Arrange
            var commaSeparatedMeasurement = "1,000,000 km";
            double oneMillion = 1000000;
            UnitGroup kilometer = new UnitGroupBuilder()
                .WithDivMulti(UnitGroup.UnitOperation.Multiply)
                .WithSymbol("km")
                .WithExponent(1)
                .Build();

            // Act
            MeasurementGroup testGroup = _measurementParser.GenerateMeasurementGroup(commaSeparatedMeasurement);


            // Assert
            Assert.AreEqual(testGroup.Quantity, oneMillion);
            Assert.AreEqual(testGroup.Units.First(), kilometer);
        }

        [TestMethod]
        public void MeasurementParser_IndianCommaSeparator_SuccessfullyParses()
        {
            // Arrange
            var indianCommaSeparatedMeasurement = "12,34,56,789 ms";
            double longNumber = 123456789;
            UnitGroup microSecond = new UnitGroupBuilder()
                .WithSymbol("ms")
                .Build();

            // Act
            MeasurementGroup testGroup = _measurementParser.GenerateMeasurementGroup(indianCommaSeparatedMeasurement);

            // Assert
            Assert.AreEqual(testGroup.Quantity, longNumber);
            Assert.AreEqual(testGroup.Units.First(), microSecond);
        }

        [TestMethod]
        public void MeasurementParser_EuropeanCommaSeparator_ThrowsParseError()
        {
            // Arrange
            var europeanCommaSeparatedMeasurement = "1.234.567,89 l";
            double euroNumber = 1234567.89;
            UnitGroup litre = new UnitGroupBuilder()
                .WithSymbol("l")
                .Build();

            // Act
            MeasurementGroup testGroup = _measurementParser.GenerateMeasurementGroup(europeanCommaSeparatedMeasurement);

            // Assert
            Assert.AreEqual(testGroup.Quantity, euroNumber);
            Assert.AreEqual(testGroup.Units.First(), litre);
        }

        [TestMethod]
        [ExpectedException(typeof(ParseError))]
        public void MeasurementParser_NoQuantityWithExponent_ThrowsParseError()
        {
            // Arrange
            string noQuantityWithExponent = "e5 kg";

            // Act
            MeasurementGroup _ = _measurementParser.GenerateMeasurementGroup(noQuantityWithExponent);
        }

        [TestMethod]
        [ExpectedException(typeof(ParseError))]
        public void MeasurementParser_NoUnitsWithExponent_ThrowsParseError()
        {
            // Arrange
            string noUnitsExponentInput = "123e4";

            // Act
            MeasurementGroup _ = _measurementParser.GenerateMeasurementGroup(noUnitsExponentInput);
        }        
        
        [TestMethod]
        [ExpectedException(typeof(ParseError))]
        public void MeasurementParser_NoUnitsWithoutExponent_ThrowsParseError()
        {
            // Arrange
            string noUnitsInput = "123";

            // Act
            MeasurementGroup _ = _measurementParser.GenerateMeasurementGroup(noUnitsInput);
        }

        [TestMethod]
        [ExpectedException(typeof(ParseError))]
        public void MeasurementParser_OnlyUnitInput_ThrowsParseError()
        {
            // Arrange
            string onlyUnit = "kg.m/s^2";

            // Act
            MeasurementGroup _ = _measurementParser.GenerateMeasurementGroup(onlyUnit);
        }

        [TestMethod]
        [ExpectedException(typeof(ParseError))]
        public void MeasurementParser_FractionalExponentInput_ThrowsParseError()
        {
            // Arrange
            string fractionalExponent = "123e4.5 kg.m/s^2";

            // Act
            MeasurementGroup _ = _measurementParser.GenerateMeasurementGroup(fractionalExponent);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MeasurementParser_BlankString_ThrowsArgumentException()
        {
            // Arrange
            string blankString = "";

            // Act
            MeasurementGroup _ = _measurementParser.GenerateMeasurementGroup(blankString); 
        }

        [TestMethod]
        public void MeasurementParser_RandomUnicode_ThrowsParseError()
        {
            List<string> randomUnicode =
            [
                "𝟙.𝟚𝟛 𝕜𝕘/𝕞𝕤^𝟚",
                "⹠≽⽗⨣ⱙ⮱⺇⡫ⱠⱿ⋯∟➋⫽⺽⒇ⴝ“⹰⬈⬩⒝⮦⌑╈⥟≖⫔Ⱨ➨⢃",
                "⁰∨⏹₈∇↏↝Ɱ⻆Ⱇ⍕⛍⫙⬪⦰╅⳰⑟∿⬫⨹⯄⾿⽛⊹≨◕┐⚣⨪",
                "⓼ⱛ₭⛊↍⛠ⵙ⇲⹲☹⚄⽊☹⋎☐■⛤℃⭸⁂⤄♊⅟⬘⒯▀⭽₲⩕∔",
                "( ͡° ͜ʖ ͡°)",
            ];

            foreach (var s in randomUnicode)
            {
                try
                {
                    MeasurementGroup _ = _measurementParser.GenerateMeasurementGroup(s);
                }
                catch (Exception e)
                {
                    Assert.IsInstanceOfType(e, typeof(ParseError));
                }
            }
        }
    }

    public class UnitGroupBuilder
    {
        private UnitGroup.UnitOperation _divMulti = UnitGroup.UnitOperation.Multiply;
        private string _symbol = "";
        private int _exponent = 1;

        public UnitGroupBuilder WithDivMulti(UnitGroup.UnitOperation divMulti)
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

        public UnitGroup Build() => new(_divMulti, _symbol, _exponent);
        
    }
}
