using System.Text.RegularExpressions;

namespace AbsoluteUnit.Program;

public interface IUnitGroupParser
{
    List<UnitGroup> ParseUnitGroups(string unitString);
}

public record UnitGroup(UnitGroup.UnitOperation Operation, string UnitSymbol, int Exponent, bool HasPrefix = false)
{
    public static UnitOperation GetUnitOperation(char c) => c switch
    {
        '/' => UnitOperation.Divide,
        '.' => UnitOperation.Multiply,
        _ => throw new ParseError("invalid DivMulti symbol")
    };

    public enum UnitOperation
    {
        Divide,
        Multiply,
    }

    public override string ToString()
    {
        string operation = (Operation == UnitOperation.Divide)
                ? "-"
                : "";

        string exponent = (Exponent is < 0 or > 1)
            ? $"^{operation}{Exponent}"
            : "";

        return $"{UnitSymbol}{exponent}";
    }
}

public partial class UnitGroupParser : IUnitGroupParser
{
    public string UnitString { get; set; } = "";

    private const string UnitGroupRegexString =
        @"([ ./])?([A-Za-zµ°Ω]+)+(?:(?:\^)?|(?:\*\*))?((?:\+|-)?\d+)?";

    [GeneratedRegex(UnitGroupRegexString)]
    private static partial Regex Regex();

    public List<UnitGroup> ParseUnitGroups(string unitString)
    {
        if (string.IsNullOrWhiteSpace(unitString))
            throw new ArgumentException("no/null unitString provided");

        MatchCollection matches = Regex().Matches(unitString);
        if (matches.Count == 0)
            throw new ParseError($"unable to parse {unitString} as UnitGroup");

        return matches
            .Select(ParseGroupMatch)
            .ToList();
    }

    private static UnitGroup ParseGroupMatch(Match match)
    {
        string unitSymbol;

        if (string.IsNullOrWhiteSpace(match.Groups[2].Value))
            throw new ParseError("no unit symbol provided");
        else
            unitSymbol = match.Groups[2].Value;

        UnitGroup.UnitOperation operation = match.Groups[1].Success
            ? UnitGroup.GetUnitOperation(match.Groups[1].Value.FirstOrDefault())
            : UnitGroup.UnitOperation.Multiply;

        int exponent = match.Groups[3].Success
            ? int.Parse(match.Groups[3].Value)
            : 1;

        return new(operation, unitSymbol, exponent);
    }
}
