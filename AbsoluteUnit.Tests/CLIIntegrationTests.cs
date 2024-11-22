using AbsoluteUnit.Program;

namespace AbsoluteUnit.Tests;

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
        => consoleOutput?.Dispose();

    static string GetResultLine(StringWriter output) =>
        output.ToString().Trim().Split('\n').Last();

    #region Conversion Tests
    [TestMethod]
    [DataRow("0.2330 in/µs", "m/s", "-dec 2", "5918.20 m.s^-1", DisplayName = "Inches per microsecond to meters per second")]
    [DataRow("1ton", "kg", "", "907.2", DisplayName = "Tons to kilograms")]
    [DataRow("1yr", "month", "-dec 0", "12", DisplayName = "Years to months")]
    [DataRow("100mi/h", "m/s", "", "44.7", DisplayName = "Miles per hour to meters per second")]
    [DataRow("10 yr", "day", "", "3650", DisplayName = "Years to days")]
    public void CLI_ConvertsUnitsCorrectly(string from, string to, string flags, string expectedValue)
    {
        // Arrange
        var flagArray = flags.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var args = new[] { "--convert", from, to }.Concat(flagArray).ToArray();

        // Act
        AbsoluteUnitCLI.Main(args);
        var output = consoleOutput.ToString();

        // Assert
        Assert.IsTrue(output.Contains(expectedValue), $"Expected output to contain '{expectedValue}'");
    }
    #endregion

    #region Flag Tests
    [TestMethod]
    [DataRow("0.2330 in/µs", "m/s", "-std", "5.918e3 m.s^-1", DisplayName = "Standard form flag")]
    [DataRow("100 mi/h", "m/s", "-ver", "x0.44704", DisplayName = "Verbose flag")]
    [DataRow("100 mi/h", "m/s", "-ver -dec 4", "44.7040", DisplayName = "Multiple flags")]
    [DataRow("1 km", "m", "-ver", "1000 m (1.000 x1000)", DisplayName = "Tab alignment")]
    public void CLI_HandlesFormatFlagsCorrectly(string from, string to, string flags, string expectedOutput)
    {
        // Arrange
        var flagArray = flags.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var args = new[] { "--convert", from, to }.Concat(flagArray).ToArray();

        // Act
        AbsoluteUnitCLI.Main(args);
        var result = GetResultLine(consoleOutput);

        // Assert
        Assert.IsTrue(result.Contains(expectedOutput),
            $"Expected '{expectedOutput}' in '{result}'");
    }
    #endregion

    #region Edge Cases
    [TestMethod]
    [DataRow("0 m", "km", "0 km", DisplayName = "Zero value")]
    [DataRow("1e15 mm", "km", "1.000e9 km", DisplayName = "Very large number")]
    public void CLI_HandlesEdgeCasesCorrectly(string from, string to, string expectedOutput)
    {
        // Arrange
        string[] args = ["--convert", from, to, "-std"];

        // Act
        AbsoluteUnitCLI.Main(args);
        var result = GetResultLine(consoleOutput);

        // Assert
        Assert.AreEqual($"Result:\t\t{expectedOutput}", result);
    }
    #endregion

    #region Error Handling
    [TestMethod]
    [DataRow("--invalid", "1 m", "", ErrorCode.InvalidCommand, DisplayName = "Invalid command")]
    [DataRow("--convert", "1 invalidunit", "m", ErrorCode.UnrecognisedUnit, DisplayName = "Invalid unit")]
    [DataRow("--convert", "1 m", "", ErrorCode.BadArgumentCount, DisplayName = "Missing target unit")]
    public void CLI_HandlesErrorsCorrectly(string command, string measurement, string target, ErrorCode expectedError)
    {
        // Arrange
        string[] args = string.IsNullOrEmpty(target)
            ? [command, measurement]
            : [command, measurement, target];

        // Act
        AbsoluteUnitCLI.Main(args);
        var result = GetResultLine(consoleOutput);

        // Assert
        Assert.IsTrue(result.Contains(expectedError.ToDisplayString()),
            $"Expected error code '{expectedError.ToDisplayString()}' in '{result}'");
    }
    #endregion

    #region Express and Simplify Commands
    [TestMethod]
    [DataRow("--express", "1 km/h", "-dec 2", "0.28 m.s^-1", DisplayName = "Express km/h in base units")]
    [DataRow("--simplify", "1000 kg*m/s^2", "-ver -dec 0", "1 kN", DisplayName = "Simplify to kilonewtons")]
    public void CLI_HandlesExpressAndSimplifyCorrectly(string command, string value, string flags, string expectedOutput)
    {
        // Arrange
        var flagArray = flags.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var args = new[] { command, value }.Concat(flagArray).ToArray();

        // Act
        AbsoluteUnitCLI.Main(args);
        var output = consoleOutput.ToString();

        // Assert
        Assert.IsTrue(output.Contains(expectedOutput),
            $"Expected output to contain '{expectedOutput}'");
    }
    #endregion
}