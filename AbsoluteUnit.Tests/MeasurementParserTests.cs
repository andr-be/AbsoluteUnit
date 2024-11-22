using AbsoluteUnit.Program;
using AbsoluteUnit.Program.Factories;
using AbsoluteUnit.Program.Parsers.ParserGroups;
using AbsoluteUnit.Program.Structures;
using AbsoluteUnit.Program.UnitTypes;

namespace AbsoluteUnit.Tests
{
    [TestClass]
    public class MeasurementParserTests
    {
        private IUnitGroupParser? _unitGroupParser;
        private IUnitFactory? _unitFactory;
        private MeasurementParser? _measurementParser;

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
            MeasurementGroup measurement = _measurementParser!.GenerateMeasurementGroup(simpleMeasurement);

            // Assert
            Assert.IsTrue(measurement.Units.Count == 1);
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
            MeasurementGroup testGroup = _measurementParser!.GenerateMeasurementGroup(goodInput);

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
            MeasurementGroup testGroup = _measurementParser!.GenerateMeasurementGroup(goodInput);

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
            MeasurementGroup testGroup = _measurementParser!.GenerateMeasurementGroup(commaSeparatedMeasurement);


            // Assert
            Assert.AreEqual(testGroup.Quantity, oneMillion);
            Assert.AreEqual(testGroup.Units.First(), kilometer);
        }

        [TestMethod]
        public void MeasurementParser_IndianCommaSeparator_SuccessfullyParses()
        {
            // Arrange
            var IndianCommaSeparatedMeasurement = "12,34,56,789 ms";
            double longNumber = 123456789;
            UnitGroup microSecond = new UnitGroupBuilder()
                .WithSymbol("ms")
                .Build();

            // Act
            MeasurementGroup testGroup = _measurementParser!.GenerateMeasurementGroup(IndianCommaSeparatedMeasurement);

            // Assert
            Assert.AreEqual(testGroup.Quantity, longNumber);
            Assert.AreEqual(testGroup.Units.First(), microSecond);
        }

        [TestMethod]
        [ExpectedException(typeof(CommandError))]
        public void MeasurementParser_NoQuantityWithExponent_ThrowsCommandError()
        {
            // Arrange
            string noQuantityWithExponent = "e5 kg";

            // Act
            MeasurementGroup _ = _measurementParser!.GenerateMeasurementGroup(noQuantityWithExponent);
        }

        [TestMethod]
        [ExpectedException(typeof(CommandError))]
        public void MeasurementParser_NoUnitsWithExponent_ThrowsCommandError()
        {
            // Arrange
            string noUnitsExponentInput = "123e4";

            // Act
            MeasurementGroup _ = _measurementParser!.GenerateMeasurementGroup(noUnitsExponentInput);
        }        
        
        [TestMethod]
        [ExpectedException(typeof(CommandError))]
        public void MeasurementParser_NoUnitsWithoutExponent_ThrowsArgumentException()
        {
            // Arrange
            string noUnitsInput = "123";

            // Act
            MeasurementGroup _ = _measurementParser!.GenerateMeasurementGroup(noUnitsInput);
        }

        [TestMethod]
        [ExpectedException(typeof(CommandError))]
        public void MeasurementParser_OnlyUnitInput_ThrowsArgumentException()
        {
            // Arrange
            string onlyUnit = "kg.m/s^2";

            // Act
            MeasurementGroup _ = _measurementParser!.GenerateMeasurementGroup(onlyUnit);
        }

        [TestMethod]
        [ExpectedException(typeof(CommandError))]
        public void MeasurementParser_FractionalExponentInput_ThrowsCommandError()
        {
            // Arrange
            string fractionalExponent = "123e4.5 kg.m/s^2";

            // Act
            MeasurementGroup _ = _measurementParser!.GenerateMeasurementGroup(fractionalExponent);
        }

        [TestMethod]
        public void MeasurementParser_BlankString_ThrowsCommandError()
        {
            try
            {
                string blankString = "";
                MeasurementGroup _ = _measurementParser!.GenerateMeasurementGroup(blankString); 
            }
            catch (CommandError e)
            {
                Assert.AreEqual(ErrorCode.InvalidMeasurement, e.Code);
            }

        }

        [TestMethod]
        public void MeasurementParser_RandomUnicode_ThrowsCommandError()
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
                    MeasurementGroup _ = _measurementParser!.GenerateMeasurementGroup(s);
                }
                catch (CommandError e)
                {
                    Assert.AreEqual(ErrorCode.InvalidMeasurement, e.Code);
                }
            }
        }

        [TestMethod]
        public void MeasurementParser_MinutesParseCorrectly()
        {
            Unit minute = new TestUnitBuilder()
                .WithUnit(new Miscellaneous(Miscellaneous.Units.Minute))
                .Build();

            Measurement expected = new(units: [minute], quantity:1, exponent:0);

            string minutes = "1 min";
            var parsed = _measurementParser?.ProcessMeasurement(minutes) ?? null;

            Assert.AreEqual(expected, parsed);
        }
    }
}
