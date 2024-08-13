using AbsoluteUnit.Program.Structures;

namespace AbsoluteUnit.Program.Units;

public class Miscellaneous(Miscellaneous.Units unit) : IUnitType
{
    public enum Units
    {
        // Time
        Year,
        Month,
        Day,
        Hour,
        Minute,
    }

    public string Symbol => Unit switch
    {
        Units.Year => "yr",
        Units.Month => "mth",
        Units.Day => "day",
        Units.Hour => "h",
        Units.Minute => "min",
        _ => throw new NotImplementedException($"{Unit} has no Symbol implementation!")
    };

    public object Unit { get; init; } = unit;

    public double FromBase(double value) => value / Conversion[(Units)Unit];

    public double ToBase(double value) => value * Conversion[(Units)Unit];

    public List<Unit> ExpressInBaseUnits() => (Units)Unit switch
    {
        Units.Year or
        Units.Month or
        Units.Day or
        Units.Hour or
        Units.Minute => [SIBase.Second()],

        _ => throw new NotImplementedException($"No base unit conversion for {Unit}")
    };

    public static readonly Dictionary<string, object> ValidUnitStrings = new()
    {
        { "yr", new Miscellaneous(Units.Year) },
        { "yrs", new Miscellaneous(Units.Year) },
        { "year", new Miscellaneous(Units.Year) },
        { "years", new Miscellaneous(Units.Year) },
        
        { "mth", new Miscellaneous(Units.Month) },
        { "mths", new Miscellaneous(Units.Month) },
        { "month", new Miscellaneous(Units.Month) },
        { "months", new Miscellaneous(Units.Month) },
        
        { "day", new Miscellaneous(Units.Day) },
        { "days", new Miscellaneous(Units.Day) },
        
        { "h", new Miscellaneous(Units.Hour) },
        { "hr", new Miscellaneous(Units.Hour) },
        { "hrs", new Miscellaneous(Units.Hour) },
        { "hour", new Miscellaneous(Units.Hour) },
        { "hours", new Miscellaneous(Units.Hour) },
        
        { "min", new Miscellaneous(Units.Minute) },
        { "mins", new Miscellaneous(Units.Minute) },
    };

    static readonly Dictionary<Units, double> Conversion = new()
    {
        { Units.Year,  31_536_000 },
        { Units.Month,  2_628_000 },
        { Units.Day,       86_400 },
        { Units.Hour,       3_600 },
        { Units.Minute,        60 },
    };
}