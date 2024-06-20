namespace AbsoluteUnit.Program.Units;

public class SIPrefix(SIPrefix.Prefixes prefix = SIPrefix.Prefixes._None)
{
    public readonly Prefixes Prefix = prefix;

    public override string ToString() => Prefix switch
    {
        Prefixes.Quetta => "Q",
        Prefixes.Ronna => "R",
        Prefixes.Yotta => "Y",
        Prefixes.Zetta => "Z",
        Prefixes.Exa => "E",
        Prefixes.Peta => "P",
        Prefixes.Tera => "T",
        Prefixes.Giga => "G",
        Prefixes.Mega => "M",
        Prefixes.Kilo => "k",
        Prefixes.Hecto => "h",
        Prefixes.Deka => "da",

        Prefixes._None => "",

        Prefixes.Deci => "d",
        Prefixes.Centi => "c",
        Prefixes.Milli => "m",
        Prefixes.Micro => "μ",
        Prefixes.Nano => "n",
        Prefixes.Pico => "p",
        Prefixes.Femto => "f",
        Prefixes.Atto => "a",
        Prefixes.Zepto => "z",
        Prefixes.Yocto => "y",
        Prefixes.Ronto => "r",
        Prefixes.Quecto => "q",
        _ => throw new NotImplementedException("Prefix enum unrecognised?")
    };

    public enum Prefixes
    {
        Quetta = 30,
        Ronna = 27,
        Yotta = 24,
        Zetta = 21,
        Exa = 18,
        Peta = 15,
        Tera = 12,
        Giga = 9,
        Mega = 6,
        Kilo = 3,
        Hecto = 2,
        Deka = 1,

        _None = 0,

        Deci = -1,
        Centi = -2,
        Milli = -3,
        Micro = -6,
        Nano = -9,
        Pico = -12,
        Femto = -15,
        Atto = -18,
        Zepto = -21,
        Yocto = -24,
        Ronto = -27,
        Quecto = -30
    }

    public static readonly Dictionary<string, SIPrefix> ValidPrefixStrings = new()
    {
        { "Q", new SIPrefix(Prefixes.Quetta) },
        { "R", new SIPrefix(Prefixes.Ronna) },
        { "Y", new SIPrefix(Prefixes.Yotta) },
        { "Z", new SIPrefix(Prefixes.Zetta) },
        { "E", new SIPrefix(Prefixes.Exa) },
        { "P", new SIPrefix(Prefixes.Peta) },
        { "T", new SIPrefix(Prefixes.Tera) },
        { "G", new SIPrefix(Prefixes.Giga) },
        { "M", new SIPrefix(Prefixes.Mega) },
        { "k", new SIPrefix(Prefixes.Kilo) },
        { "h", new SIPrefix(Prefixes.Hecto) },
        { "da", new SIPrefix(Prefixes.Deka) },
        { "d", new SIPrefix(Prefixes.Deci) },
        { "c", new SIPrefix(Prefixes.Centi) },
        { "m", new SIPrefix(Prefixes.Milli) },
        { "µ", new SIPrefix(Prefixes.Micro) },
        { "n", new SIPrefix(Prefixes.Nano) },
        { "p", new SIPrefix(Prefixes.Pico) },
        { "f", new SIPrefix(Prefixes.Femto) },
        { "a", new SIPrefix(Prefixes.Atto) },
        { "z", new SIPrefix(Prefixes.Zepto) },
        { "y", new SIPrefix(Prefixes.Yocto) },
        { "r", new SIPrefix(Prefixes.Ronto) },
        { "q", new SIPrefix(Prefixes.Quecto) },
    };

    public override bool Equals(object? obj) =>
        obj is SIPrefix other &&
        Prefix.Equals(other.Prefix);
}

public static class PrefixExtensions
{
    public static double Factor(this SIPrefix.Prefixes prefix) => (int)prefix;
}
