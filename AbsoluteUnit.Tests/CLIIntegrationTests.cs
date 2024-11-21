using AbsoluteUnit.Program.Factories;

namespace AbsoluteUnit.Tests
{
    [TestClass]
    public class CLIIntegrationTests
    {
        private StringWriter? consoleOutput;

        [TestInitialize]
        public void TestInitialize()
        {
            consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);
        }

        [TestCleanup]
        public void TestCleanup() 
            => consoleOutput.Dispose();

        #region Convert Command Tests
        [TestMethod]
        public void Convert_WithDecimalPrecision_FormatsCorrectly()
        {
            string[] args = ["--convert", "0.2330 in/µs", "m/s", "-dec", "2"];

            AbsoluteUnitCLI.Main(args);

            var output = consoleOutput.ToString().Trim();
            var result = output.Split('\n').Last();
            Assert.AreEqual("Result:\t\t5918.20 m.s^-1", result);
        }

        [TestMethod]
        public void Convert_WithVerboseFlag_ShowsConversionFactor()
        {
            string[] args = ["--convert", "100 mi/h", "m/s", "-ver"];

            AbsoluteUnitCLI.Main(args);

            var output = consoleOutput.ToString().Trim().Split('\n').Last(); ;
            Assert.AreEqual("Result:\t\t44.70 m.s^-1 (100.0 x0.44704)", output);
        }

        [TestMethod]
        public void Convert_WithStandardFlag_RemovesDecimalPlaces()
        {
            string[] args = ["--convert", "0.2330 in/µs", "m/s", "-std"];

            AbsoluteUnitCLI.Main(args);

            var output = consoleOutput.ToString().Trim().Split('\n').Last(); ;
            Assert.AreEqual("Result:\t\t5918 m.s^-1", output);
        }
        #endregion

        #region Error Handling Tests
        [TestMethod]
        [ExpectedException(typeof(CommandNotRecognised))]
        public void InvalidCommand_ThrowsCommandNotRecognised()
        {
            string[] args = ["--invalid", "1 m"];
            AbsoluteUnitCLI.Main(args);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Convert_WithInvalidUnit_ThrowsArgumentException()
        {
            string[] args = ["--convert", "1 invalidunit", "m"];
            AbsoluteUnitCLI.Main(args);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Convert_WithMissingTarget_ThrowsArgumentException()
        {
            string[] args = ["--convert", "1 m"];
            AbsoluteUnitCLI.Main(args);
        }
        #endregion

        #region Edge Cases
        [TestMethod]
        public void Convert_ZeroValue_HandlesCorrectly()
        {
            string[] args = ["--convert", "0 m", "km"];

            AbsoluteUnitCLI.Main(args);

            var output = consoleOutput.ToString().Trim().Split('\n').Last();
            Assert.AreEqual("Result:\t\t0 km", output);
        }

        [TestMethod]
        public void Convert_VeryLargeNumber_UsesScientificNotation()
        {
            string[] args = ["--convert", "1e15 mm", "km", "-std"];

            AbsoluteUnitCLI.Main(args);

            var output = consoleOutput.ToString().Trim();
            Assert.AreEqual("Result:\t\t1.000e9 km", output);
        }
        #endregion

        #region Formatting Tests
        [TestMethod]
        public void Output_WithMultipleFlags_FormatsCorrectly()
        {
            string[] args = ["--convert", "100 mi/h", "m/s", "-ver", "-dec", "4"];

            AbsoluteUnitCLI.Main(args);

            var output = consoleOutput.ToString().Trim().Split('\n').Last(); ;
            Assert.AreEqual("Result:\t\t44.7040 m.s^-1 (100.0 x0.44704)", output);
        }

        [TestMethod]
        public void Output_TabAlignment_IsConsistent()
        {
            string[] args = ["--convert", "1 km", "m", "-ver"];

            AbsoluteUnitCLI.Main(args);

            var output = consoleOutput.ToString().Trim().Split('\n').Last(); ;
            Assert.AreEqual("Result:\t\t1000 m (1.000 x1000)", output);
        }
        #endregion

        [TestMethod]
        public void CLI_Convert_InchesPerMicrosecondIntoMetersPerSecond_Correctly()
        {
            // Arrange
            string[] args = ["--convert", "0.2330 in/µs", "m/s", "-dec", "2"];

            // Act
            AbsoluteUnitCLI.Main(args);

            // Assert
            string output = consoleOutput.ToString();
            Assert.IsTrue(output.Contains("5918.20"), "Expected output to contain '5918.20 m/s'");
        }

        [TestMethod]
        public void CLI_Convert_KgsIntoShortTons_Correctly()
        {
            // Arrange
            string[] args = ["--convert", "1ton", "kg"];

            // Act
            AbsoluteUnitCLI.Main(args);
            string output = consoleOutput.ToString();

            // Assert
            Assert.IsTrue(output.Contains("907.2"), "Expected output to contain 907.2 kg");
        }

        [TestMethod]
        public void CLI_Convert_YearsIntoMonths()
        {
            // Arrange
            string[] args = ["--convert", "1yr", "month", "-dec", "0"];

            // Act
            AbsoluteUnitCLI.Main(args);

            // Assert
            string output = consoleOutput.ToString();
            Assert.IsTrue(output.Contains("12"), "Expected output to be 12 months!");
        }

        [TestMethod]
        public void CLI_Convert_MilesPerHourIntoMetersPerSecond()
        {
            // Arrange
            string[] args = ["--convert", "100mi/h", "m/s"];

            // Act
            AbsoluteUnitCLI.Main(args);

            // Assert
            string output = consoleOutput.ToString();
            Assert.IsTrue(output.Contains("44.7"), "Expected output to be 44.7 m/s!");
        }

        [TestMethod]
        public void CLI_ConvertTenYearsIntoDays()
        {
            // Arrange
            string[] args = ["--convert", "10 yr", "day"];

            // Act
            AbsoluteUnitCLI.Main(args);

            // Assert
            string output = consoleOutput.ToString();
            Assert.IsTrue(output.Contains("3650"), "Expected output to be 3650 days!");
        }

        [TestMethod]
        public void CLI_ExpressesProperly()
        {
            // Arrange
            string[] args = ["--express", "1 km/h", "-dec", "2"];

            // Act
            AbsoluteUnitCLI.Main(args);

            // Assert
            string output = consoleOutput.ToString();
            Assert.IsTrue(output.Contains("0.28 m.s^-1"), "Expected output to contain '0.28 m/s'");
        }

        [TestMethod]
        public void CLI_SimplifiesProperly()
        {
            // Arrange
            string[] args = ["--simplify", "1000 kg*m/s^2", "-ver", "-dec", "0"];

            // Act
            AbsoluteUnitCLI.Main(args);

            // Assert
            string output = consoleOutput.ToString();
            Assert.IsTrue(output.Contains("1 kN"), "Expected output to contain '1 kN'");
        }

        [TestMethod]
        [ExpectedException(typeof(CommandNotRecognised))]
        public void CLI_HandlesInvalidCommandProperly()
        {
            // Arrange
            string[] args = ["--invalid", "1 m"];

            // Act
            AbsoluteUnitCLI.Main(args);

            // Assert is handled by ExpectedException
        }

        [TestMethod]
        public void CLI_HandlesVerboseFlagProperly()
        {
            // Arrange
            string[] args = ["--convert", "1 ft", "m", "--verbose"];

            // Act
            AbsoluteUnitCLI.Main(args);

            // Assert
            string output = consoleOutput.ToString();
            Assert.IsTrue(output.Contains("x0.3048"), "Expected verbose output");
        }
    }
}
