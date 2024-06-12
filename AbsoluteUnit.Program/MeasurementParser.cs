using AbsoluteUnit.Program;
using System.Text.RegularExpressions;

namespace AbsoluteUnit
{
    public record MeasurementGroup(double Quantity, int Exponent, List<UnitGroup> Units)
    {
        public override string ToString() =>
            $"{Quantity}{(Exponent != 0 ? "e" + Exponent : "")} {string.Join("", Units)}";
    }

    public partial class MeasurementParser
    {
        public MeasurementGroup MeasurementGroup { get; }

        private const string MeasurementRegexString =
            @"^(-?\d+[.,\d]*(?:\.|,)?\d*)?(?:e(-?\d+))? *([A-Za-zµ°Ω]+[\w\d.*^\-\/]*)*$";

        [GeneratedRegex(MeasurementRegexString)]
        private static partial Regex Regex();

        private string MeasurementString { get; }
        private IUnitGroupParser UnitGroupParser { get; }
        private IUnitFactory UnitFactory { get; }

        public MeasurementParser(IUnitGroupParser unitGroupParser, IUnitFactory unitFactory, string measurementString, bool unitOnly = false)
        {
            MeasurementString = measurementString;
            UnitGroupParser = unitGroupParser;
            UnitFactory = unitFactory;

            var match = Regex().Match(MeasurementString);

            if (match.Success) MeasurementGroup = unitOnly
                ? CreateUnitOnlyGroup(match)
                : CreateFullGroup(match);

            else throw new ParseError($"invalid measurementString: [{MeasurementString}]: invalid format");
        }

        private MeasurementGroup CreateFullGroup(Match match)
        {
            ValidateFullGroup(match);
            try
            {
                var quantity = ParseQuantity(match.Groups[1].Value);
                var exponent = ParseExponent(match.Groups[2].Value);
                var units = GetUnitGroups(match.Groups[3].Value);

                return new(quantity, exponent, units);
            }
            catch (ParseError e)
            {
                throw new ParseError($"unable to parse {MeasurementString} as MeasurementGroup", inner: e);
            }
            catch (ArgumentException a)
            {
                throw new ParseError($"unable to parse {match.Groups[3].Value} as UnitGroup", inner: a);
            }
        }

        private MeasurementGroup CreateUnitOnlyGroup(Match match)
        {
            if (!match.Groups[3].Success || match.Groups[3].Value.Contains('e'))
                throw new ParseError($"invalid measurementString [{MeasurementString}]: no units provided");

            try
            {
                return new(0.0, 0, GetUnitGroups(match.Groups[3].Value));
            }
            catch (Exception e)
            {
                throw new ParseError($"Unable to parse {MeasurementString} as a UnitOnly measurement group", inner: e);
            }
        }

        private void ValidateFullGroup(Match match)
        {
            if (match.Groups[0].Length == 0)
                throw new ArgumentException("blank measurement string; invalid format");

            else if (!match.Groups[1].Success)
                throw new ParseError($"invalid measurementString [{MeasurementString}]: no quantity provided");

            else if (!match.Groups[3].Success || match.Groups[3].Value.Contains('e'))
                throw new ParseError($"invalid measurementString [{MeasurementString}]: no units provided");
        }

        private List<UnitGroup> GetUnitGroups(string unitString) => 
            UnitGroupParser.ParseUnitGroups(unitString);
        
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
                UnitFactory.BuildUnits(MeasurementGroup.Units),
                MeasurementGroup.Quantity,
                MeasurementGroup.Exponent
                );
        }
    }

    public class ParseError : Exception
    {
        public ParseError() { }
        public ParseError(string message) : base(message) { }
        public ParseError(string message, Exception inner) : base(message, inner) { }
    }
}
