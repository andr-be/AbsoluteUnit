using System.Text.RegularExpressions;
using AbsoluteUnit.Program;

namespace AbsoluteUnit
{
    public record MeasurementGroup(double Quantity, int Exponent, List<UnitGroup> Units)
    {
        public override string ToString()
        {
            string measurementGroupString = $"{Quantity}{(Exponent != 0 ? "e" + Exponent : "")} ";

            foreach (var unit in Units)
                measurementGroupString += unit.ToString();

            return measurementGroupString;
        }
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

    public partial class MeasurementParser
    {
        public MeasurementGroup MeasurementGroup { get; }

        private const string MeasurementRegexString =
            @"^(-?\d+[.,\d]*(?:\.|,)?\d*)?(?:e(-?\d+))? *([A-Za-zµ°Ω]+[\w\d.*^\-\/]*)*$";

        [GeneratedRegex(MeasurementRegexString)]
        private static partial Regex Regex();

        public MeasurementParser(string measurementString)
        {
            var match = Regex().Match(measurementString);

            if (!match.Success)
                throw new ParseError($"invalid measurementString: [{measurementString}]: invalid format");

            else if (match.Groups[0].Length == 0)
                throw new ArgumentException("blank measurement string; invalid format");
            
            else if (!match.Groups[1].Success)
                throw new ParseError($"invalid measurementString [{measurementString}]: no quantity provided");

            else if (!match.Groups[3].Success || match.Groups[3].Value.Contains('e'))
                throw new ParseError($"invalid measurementString [{measurementString}]: no units provided");

            try
            {
                var quantity = ParseQuantity(match.Groups[1].Value);
                var exponent = ParseExponent(match.Groups[2].Value);
                var units = new UnitGroupParser(match.Groups[3].Value).ParseUnitGroups();

                MeasurementGroup = new(quantity, exponent, units);
            }
            catch (ParseError e)
            {
                throw new ParseError($"unable to parse {measurementString} as MeasurementGroup", inner: e);
            }
            catch (ArgumentException a)
            {
                throw new ParseError($"unable to parse {match.Groups[3].Value} as UnitGroup", inner: a);
            }
        }

        private static int ParseExponent(string exponentString) => 
            int.TryParse(exponentString, null, out var exponent) ? exponent : 0;

        private static string ToEuroString(string s) => 
            s.Replace(',', '#').Replace('.', ',').Replace('#', '.');

        private static double ParseQuantity(string quantityString)
        {
            quantityString = quantityString.Trim();

            if (double.TryParse(quantityString, null, out var quantity))
                return quantity;

            // ================== HERESY CONTAINED WITHIN ==================
            var euroString = ToEuroString(quantityString);
            if (double.TryParse(euroString, null, out var euroQuantity))
                return euroQuantity;
            // ================== DON'T TRY THIS AT HOME  ================== 

            else
                throw new ParseError($"unable to parse quantity: {quantityString}");
        }

        public AbsMeasurement ProcessMeasurement()
        {
            return new(
                new UnitFactory(MeasurementGroup.Units).BuildUnits(),
                MeasurementGroup.Quantity,
                MeasurementGroup.Exponent
                );
        }
    }

    public partial class UnitGroupParser
    {
        private const string UnitGroupRegexString = 
            @"([ ./])?([A-Za-zµ°Ω]+)+(?:(?:\^)?|(?:\*\*))?((?:\+|-)?\d+)?";
        [GeneratedRegex(UnitGroupRegexString)]
        private static partial Regex Regex();
        private string UnitString { get; }

        public UnitGroupParser(string unitString)
        {
            if (string.IsNullOrWhiteSpace(unitString))
                throw new ArgumentException("no/null unitString provided");

            UnitString = unitString;
        }

        public List<UnitGroup> ParseUnitGroups()
        {
            MatchCollection matches = Regex().Matches(UnitString);
            if (matches.Count == 0)
                throw new ParseError($"unable to parse {UnitString} as UnitGroup");

            return matches
                .Select(ParseGroupMatch)
                .ToList();
        }

        private static UnitGroup ParseGroupMatch(Match match)
        {
            string unitSymbol;

            if (!string.IsNullOrWhiteSpace(match.Groups[2].Value))
                unitSymbol = match.Groups[2].Value;
            else
                throw new ParseError("no unit symbol provided");

            UnitGroup.UnitOperation divMulti = match.Groups[1].Success
                ? UnitGroup.GetUnitOperation(match.Groups[1].Value.FirstOrDefault())
                : UnitGroup.UnitOperation.Multiply;

            int exponent = match.Groups[3].Success
                ? int.Parse(match.Groups[3].Value)
                : 1;

            return new(divMulti, unitSymbol, exponent);
        }
    }

    public class ParseError : Exception
    {
        public ParseError() { }
        public ParseError(string message) : base(message) { }
        public ParseError(string message, Exception inner) : base(message, inner) { }
    }
}
