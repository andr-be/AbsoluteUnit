using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AbsoluteUnit;

namespace AbsoluteUnit.Tests
{
    [TestClass]
    public class CLIIntegrationTests
    {
        private StringWriter consoleOutput;

        [TestInitialize]
        public void TestInitialize()
        {
            consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            consoleOutput.Dispose();
        }

        [TestMethod]
        public void CLI_Convert_InchesPerMicrosecondIntoMetersPerSecond_0p2330__5918p2()
        {
            // Arrange
            string[] args = ["--convert", "0.2330 in/µs", "m/s"];

            // Act
            AbsoluteUnitCLI.Main(args);

            // Assert
            string output = consoleOutput.ToString();
            Assert.IsTrue(output.Contains("5918.2"), "Expected output to contain '5918.2 m/s'");
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
            Assert.IsTrue(output.Contains("0.28 m/s"), "Expected output to contain '0.28 m/s'");
        }

        [TestMethod]
        public void CLI_SimplifiesProperly()
        {
            // Arrange
            string[] args = ["--simplify", "1000 kg*m/s^2", "-ver"];

            // Act
            AbsoluteUnitCLI.Main(args);

            // Assert
            string output = consoleOutput.ToString();
            Assert.IsTrue(output.Contains("1 kN"), "Expected output to contain '1 kN'");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
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
            Assert.IsTrue(output.Contains("Conversion steps:"), "Expected verbose output");
        }
    }
}
