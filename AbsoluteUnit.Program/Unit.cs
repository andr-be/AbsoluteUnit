namespace AbsoluteUnit.Program
{
    public class Unit
    {
        public AbsUnit Base;
        public AbsPrefix? Prefix;
        public int Exponent;
    }

    public abstract class AbsUnit
    {
        public abstract double ToBase(double value);
        public abstract double FromBase(double value);
        public double Convert(double value, AbsUnit target) => target.FromBase(ToBase(value));
        protected abstract Dictionary<string, object> UnitStrings { get; }
    }

    public abstract class AbsPrefix
    {
        public abstract double Factor { get; }
        public double Convert(double value, AbsPrefix target) => value * Factor / target.Factor;
    }

    public class SI_Base(SI_Base.Unit baseUnit) : AbsUnit
    {
        public enum Unit
        {
            Meter,
            Gram,
            Second,
            Ampere,
            Kelvin,
            Mole,
            Candela
        }

        public override double ToBase(double value) => value; // SI Base units are already in base form

        public override double FromBase(double value) => value; // SI Base units are already in base form

        public Unit? FromString(string input) => 
            (UnitStrings.TryGetValue(input, out var unit) && unit is Unit unitValue)
                ? unitValue
                : null;

        public override string ToString() => UnitStrings
            .Where(a => a.Value.Equals(baseUnit))
            .FirstOrDefault()
            .Key; 

        private static readonly Dictionary<string, object> unitStrings = new()
        {
            { "m", Unit.Meter },
            { "g", Unit.Gram },
            { "s", Unit.Second },
            { "A", Unit.Ampere },
            { "K", Unit.Kelvin },
            { "mole", Unit.Mole },
            { "cd", Unit.Candela },
        };

        protected override Dictionary<string, object> UnitStrings => unitStrings;
    }

    public class SI_Derived(SI_Derived.Unit baseUnit) : AbsUnit
    {
        public enum Unit
        {
            Hertz,
            Radian,
            Steradian,
            Newton,
            Pascal,
            Joule,
            Watt,
            Coulomb,
            Volt,
            Farad,
            Ohm,
            Siemens,
            Weber,
            Tesla,
            Henry,
            Celsius,
            Lumen,
            Lux,
            Becquerel,
            Gray,
            Sievert,
            Katal,
        }
        public override double FromBase(double value)
        {
            throw new NotImplementedException();
        }

        public override double ToBase(double value)
        {
            throw new NotImplementedException();
        }
    }

    public class USCustomary(USCustomary.Unit baseUnit) : AbsUnit
    {
        public enum Unit
        {
            // Length
            Mil,
            Inch,
            Feet,
            Yards,
            Miles,
            // Weight
            Ounce,
            Pound,
            Ton,
            // Volume
            FluidOunce,
            Pint,
            Gallon,
            // Temperature
            Fahrenheit
        }

        public override double FromBase(double value)
        {
            throw new NotImplementedException();
        }

        public override double ToBase(double value)
        {
            throw new NotImplementedException();
        }
    }

    public class MiscUnit : AbsUnit
    {
        public override double FromBase(double value)
        {
            throw new NotImplementedException();
        }

        public override double ToBase(double value)
        {
            throw new NotImplementedException();
        }
    }

    public class SIPrefix(SIPrefix.Prefix prefix) : AbsPrefix
    {
        public enum Prefix
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
            Deca = 1,
            // Base case is 0!
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

        public double Convert(double value, Prefix target) => value * Math.Pow(10, Factor) * Math.Pow(10, target.Factor());

        public Prefix? FromString(string input) => PrefixStrings.TryGetValue(input, out var prefix) ? prefix : null;

        public override string ToString() => PrefixStrings.FirstOrDefault(p => p.Value == prefix).Key;

        private static readonly Dictionary<string, Prefix> PrefixStrings = new()
        {
            { "Q", Prefix.Quetta },
            { "R", Prefix.Ronna },
            { "Y", Prefix.Yotta },
            { "Z", Prefix.Zetta },
            { "E", Prefix.Exa },
            { "P", Prefix.Peta },
            { "T", Prefix.Tera },
            { "G", Prefix.Giga },
            { "M", Prefix.Mega },
            { "k", Prefix.Kilo },
            { "h", Prefix.Hecto },
            { "da", Prefix.Deca },
            { "d", Prefix.Deci },
            { "c", Prefix.Centi },
            { "m", Prefix.Milli },
            { "µ", Prefix.Micro },
            { "n", Prefix.Nano },
            { "p", Prefix.Pico },
            { "f", Prefix.Femto },
            { "a", Prefix.Atto },
            { "z", Prefix.Zepto },
            { "y", Prefix.Yocto },
            { "r", Prefix.Ronto },
            { "q", Prefix.Quecto },
        };

        public override double Factor => prefix.Factor();
    }

    public static class PrefixExtensions
    {
        public static double Factor(this SIPrefix.Prefix prefix) => (int)prefix;
    }
}
