using System.Text.RegularExpressions;

namespace AbsoluteUnit
{

    public partial class MeasurementParser
    {
        public double Quantity { get; }
        public int Exponent { get; }
        public UnitGroupParser Units { get; }

        private const string MeasurementRegexString = @"^(-?\d+[,\d]*\.?\d*)+(?:e(-?\d+))? *([A-Za-zµ°Ω]+[\w\d.*^\-\/]*)+$";
        [GeneratedRegex(MeasurementRegexString)]
        private static partial Regex Regex();

        public MeasurementParser(string measurementString)
        {
            var match = Regex().Match(measurementString);
            if (!match.Success)
            {
                if (measurementString
                    .AsEnumerable()
                    .Any(a => char.IsNumber(a) || a == 'e' || a == 'E')
                )
                    throw new ParseError($"invalid measurement string: {measurementString} (no units provided)");
                else
                    throw new ParseError($"invalid measurement string: {measurementString} (invalid format)");
            }

            try
            {
                Quantity = ParseQuantity(match.Groups[1].Value);
                Exponent = ParseExponent(match.Groups[2].Value);
                Units = new UnitGroupParser(match.Groups[3].Value);
            }
            catch (Exception e)
            {
                throw new ParseError($"unable to parse {measurementString} as MeasurementGroup", inner: e);
            }
        }

        private static int ParseExponent(string exponentString)
        {
            if (int.TryParse(exponentString, null, out var exponent))
                return exponent;
            else
                return 0;
        }

        private static double ParseQuantity(string quantityString)
        {
            if (double.TryParse(quantityString, null, out var quantity))
                return quantity;
            else
                throw new ParseError($"unable to parse quantity: {quantityString}");
        }
    }

    public partial class UnitGroupParser
    {
        private const string UnitGroupRegexString = @"([ ./])?([A-Za-zµ°Ω]+)+(?:(?:\^)?|(?:\*\*))?((?:\+|-)?\d+)?";
        [GeneratedRegex(UnitGroupRegexString)]
        private static partial Regex Regex();

        public List<UnitGroup> Groups { get; set; } = [];

        public UnitGroupParser(string unitString)
        {
            if (string.IsNullOrWhiteSpace(unitString))
                throw new ArgumentException("no/null unitString provided");

            Groups = ParseUnitGroups(unitString);
        }

        private static List<UnitGroup> ParseUnitGroups(string unitString)
        {
            List<UnitGroup> groups = [];
            MatchCollection matches = Regex().Matches(unitString);
            if (matches.Count == 0)
                throw new ParseError($"parsing error: {unitString} produced no matches");

            foreach (Match match in matches.Cast<Match>())
            {
                var unitGroup = ParseGroupMatch(match);
                groups.Add(unitGroup);
            }
            return groups;
        }

        private static UnitGroup ParseGroupMatch(Match match)
        {
            UnitGroup.DivMulti divMulti = match.Groups[1].Success
                ? UnitGroup.GetDivMulti(match.Groups[1].Value.FirstOrDefault())
                : UnitGroup.DivMulti.Multiply;

            int exponent = match.Groups[3].Success
                ? int.Parse(match.Groups[3].Value)
                : 1;

            string unitSymbol = match.Groups[2].Value;

            return new(divMulti, unitSymbol, exponent);
        }
    }

    public class ParseError : Exception
    {
        public ParseError() { }
        public ParseError(string message) : base(message) { }
        public ParseError(string message, Exception inner) : base(message, inner) { }
    }


    public record UnitGroup(UnitGroup.DivMulti _DivMulti, string UnitSymbol, int Exponent) 
    {
        public static DivMulti GetDivMulti(char c)
        {
            return c switch
            {
                '/' => DivMulti.Divide,
                '.' => DivMulti.Multiply,
                _ => throw new ParseError("invalid DivMulti symbol")
            };
        }
        public enum DivMulti
        {
            Divide,
            Multiply,
        }
    }
}
