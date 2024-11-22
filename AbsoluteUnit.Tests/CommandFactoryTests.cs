using AbsoluteUnit.Program;
using AbsoluteUnit.Program.Factories;

namespace AbsoluteUnit.Tests
{
    [TestClass]
    public class CommandFactoryTests
    {
        [TestMethod]
        public void ParseCommand_ValidConvertCommand_ReturnsCorrectCommandType()
        {
            // Arrange
            string[] args = ["--convert", "123.4e5 m", "ft"];
            var correctCommand = Command.Convert;

            // Act
            CommandFactory commandFactory = new(args);

            // Assert
            Assert.AreEqual(correctCommand, commandFactory.ParseArguments().CommandType);
        }

        [TestMethod]
        public void ParseCommand_ValidExpressCommand_ReturnsCorrectCommandType()
        {
            // Arrange
            string[] args = ["--express", "123.4e5 ft"];
            var correctCommand = Command.Express;

            // Act
            CommandFactory commandFactory = new(args);

            // Assert
            Assert.AreEqual(correctCommand, commandFactory.ParseArguments().CommandType);
        }

        [TestMethod]
        public void ParseCommand_ValidSimplifyCommand_ReturnsCorrectCommandType()
        {
            // Arrange
            string[] args = ["--simplify", "123.4e5 ft"];
            var correctCommand = Command.Simplify;

            // Act
            CommandFactory commandFactory = new(args);

            // Assert
            Assert.AreEqual(correctCommand, commandFactory.ParseArguments().CommandType);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand_FlagArgumentsReturnValidFlags()
        {
            // Arrange
            string[] args = 
            [
                "--Convert", "1m", "ft", 
                "-dec", "2", 
                "--verbose"
            ];
            Dictionary<Flag, int> flags = new()
            {
                { Flag.DecimalPlaces, 2 },
                { Flag.VerboseCalculation, 0 }
            };

            // Act
            CommandFactory commandFactory = new(args);

            // Assert
            Assert.IsTrue(DictionaryEqual(commandFactory.ParseArguments().Flags, flags));
        }

        [TestMethod]
        public void ParseCommand_AllFlags_AllowsAllFlags()
        {
            // Arrange
            string[] args = 
            [
                "--convert", "1m", "ft", 
                "-dec", "2", 
                "-sig", "3", 
                "--verbose", 
                "--standard", 
                "--engineering"
            ];
            Dictionary<Flag, int> flags = new()
            {
                {Flag.DecimalPlaces, 2},
                {Flag.SignificantFigures, 3},
                {Flag.VerboseCalculation, 0},
                {Flag.StandardForm, 0 },
                {Flag.Engineering, 0},
            };

            // Act
            CommandFactory commandFactory = new(args);

            // Assert
            Assert.IsTrue(DictionaryEqual(commandFactory.ParseArguments().Flags, flags));
        }

        [TestMethod]
        [ExpectedException(typeof(CommandError))]
        public void ParseCommand_InvalidCommand_ThrowsCommandNotRecognisedException()
        {
            string[] args = ["--dinosaur", "1m", "ft"];

            CommandFactory commandFactory = new(args);
            commandFactory.ParseArguments();
        }

        [TestMethod]
        [ExpectedException(typeof(CommandError))]
        public void ParseCommand_InvalidFlag_ThrowsFlagNotRecognisedException()
        {
            string[] args = ["--convert", "1m", "ft", "-flag"];

            CommandFactory commandFactory = new(args);
            commandFactory.ParseArguments();
        }

        [TestMethod]
        [ExpectedException(typeof(CommandError))]
        public void ParseCommand_InvalidArgumentCount_ThrowsArgumentException()
        {
            string[] args = ["--convert", "1m", "ft", "badarg"];

            CommandFactory commandFactory = new(args);
            commandFactory.ParseArguments();
        }

        [TestMethod]
        [ExpectedException(typeof(CommandError))]
        public void ParseCommand_InvalidFlagArgument_ThrowsArgumentException()
        {
            string[] args = ["--convert", "1m", "ft", "-dec", "phat"];

            CommandFactory commandFactory = new(args);
            commandFactory.ParseArguments();
        }

        private static bool DictionaryEqual<T>(Dictionary<T, int> d1, Dictionary<T, int> d2) => 
            d1.Count == d2.Count && 
            !d1.Except(d2).Any();
    }
}