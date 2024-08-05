using AbsoluteUnit.Program.Structures;
using AbsoluteUnit.Program;
using System.Text.RegularExpressions;

namespace AbsoluteUnit
{

    public partial class MeasurementParser(IUnitGroupParser unitGroupParser, IUnitFactory unitFactory) : IMeasurementParser
    {
        public MeasurementGroup? MeasurementGroup { get; set; }
        public string? MeasurementString { get; set; }

        private const string MeasurementRegexString =
            @"^(-?\d+[.,\d]*(?:\.|,)?\d*)?(?:e(-?\d+))? *([A-Za-zµ°Ω]+[\w\d.*^\-\/]*)*$";

        [GeneratedRegex(MeasurementRegexString)]
        private static partial Regex Regex();

        public MeasurementGroup GenerateMeasurementGroup(string measurementString, bool unitOnly = false)
        {
            MeasurementString = measurementString;

            var match = Regex().Match(MeasurementString);

            if (match.Success)
            {
                MeasurementGroup = unitOnly
                    ? CreateUnitOnlyGroup(match)
                    : CreateFullGroup(match);

                return MeasurementGroup;
            }

            else throw new ParseErrorException($"invalid measurementString: [{MeasurementString}]: invalid format");
        }

        public AbsMeasurement ProcessMeasurement()
        {
            var units = unitFactory.BuildUnits(MeasurementGroup?.Units ?? []);
            var quantity = MeasurementGroup?.Quantity ?? 0;
            var exponent = MeasurementGroup?.Exponent ?? 0;
            
            return new(units, quantity, exponent);
        }

        public AbsMeasurement ProcessMeasurement(string measurementString, bool unitOnly = false)
        {
            GenerateMeasurementGroup(measurementString, unitOnly);
            return ProcessMeasurement();
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
            catch (ParseErrorException e)
            {
                throw new ParseErrorException($"unable to parse {MeasurementString} as MeasurementGroup", inner: e);
            }
            catch (ArgumentException a)
            {
                throw new ParseErrorException($"unable to parse {match.Groups[3].Value} as UnitGroup", inner: a);
            }
        }

        private MeasurementGroup CreateUnitOnlyGroup(Match match)
        {
            if (!match.Groups[3].Success || match.Groups[3].Value.Contains('e'))
                throw new ParseErrorException($"invalid measurementString [{MeasurementString}]: no units provided");

            try
            {
                return new(0.0, 0, GetUnitGroups(match.Groups[3].Value));
            }
            catch (Exception e)
            {
                throw new ParseErrorException($"Unable to parse {MeasurementString} as a UnitOnly measurement group", inner: e);
            }
        }

        private void ValidateFullGroup(Match match)
        {
            if (match.Groups[0].Length == 0)
                throw new ArgumentNullException(nameof(match), message:"blank measurement string; invalid format");

            else if (!match.Groups[1].Success)
                throw new ArgumentException($"invalid measurementString [{MeasurementString}]: no quantity provided");

            else if (!match.Groups[3].Success || match.Groups[3].Value.Contains('e'))
                throw new ArgumentException($"invalid measurementString [{MeasurementString}]: no units provided");
        }

        private List<UnitGroup> GetUnitGroups(string unitString) => 
            unitGroupParser.ParseUnitGroups(unitString);
        
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
                throw new ParseErrorException($"unable to parse quantity: {quantityString}");
        }
    }

    public class ParseErrorException : Exception
    {
        public ParseErrorException() { }
        public ParseErrorException(string message) : base(message) { }
        public ParseErrorException(string message, Exception inner) : base(message, inner) { }
    }
}
