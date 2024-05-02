namespace AbsoluteUnit.Tests
{
    [TestClass]
    public class CommandParserTests
    {
        [TestMethod]
        public void ParseCommand_ValidCommand_ReturnsCorrectCommandType()
        {
            string[] args = ["--convert", "123.4e5 m", "ft"];

            CommandParser commandParser = new(args);

            Assert.AreEqual(CommandParser.Command.Convert, commandParser.CommandType);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand_FlagArgumentsReturnValidFlags()
        {
            string[] args = ["--Convert", "1m", "ft", "-dec", "2", "--verbose"];
            List<CommandParser.Flag> flags = [CommandParser.Flag.DecimalPlaces, CommandParser.Flag.VerboseCalculation];

            CommandParser commandParser = new(args);

            for(int i = 0; i < flags.Count; i++)
            {
                Assert.AreEqual(commandParser.Flags[i], flags[i]);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(CommandNotRecognised))]
        public void ParseCommand_InvalidCommand_ThrowsCommandNotRecognisedException()
        {
            string[] args = ["--dinosaur", "1m", "ft"];

            CommandParser _ = new(args);
        }

        [TestMethod]
        [ExpectedException(typeof(FlagNotRecognised))]
        public void ParseCommand_InvalidFlag_ThrowsFlagNotRecognisedException()
        {
            string[] args = ["--convert", "1m", "ft", "-flag"];

            CommandParser _ = new(args);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ParseCommand_InvalidArgumentCount_ThrowsArgumentException()
        {
            string[] args = ["--convert", "1m", "ft", "badarg"];

            CommandParser _ = new(args);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ParseCommand_InvalidFlagArgument_ThrowsArgumentException()
        {
            string[] args = ["--convert", "1m", "ft", "-dec", "phat"];

            CommandParser _ = new(args);
        }
    }
}