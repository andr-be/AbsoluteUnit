namespace AbsoluteUnit.Program
{
    public class Unit
    {
        public IUnit Base;
        public AbsPrefix? Prefix;
        public int Exponent;

        public Unit(UnitGroup group)
        {
            Base = AbsUnit.ParseUnit(group.UnitSymbol);

            var remaining = group.UnitSymbol.Replace(Base.ToString() ?? null, "");

            Prefix = AbsPrefix.ParsePrefix(remaining) as AbsPrefix;

            Exponent = group.Exponent;
        }
    }

    public interface IUnit
    {
        string Symbol { get; }
        double ToBase(double value);
        double FromBase(double value);
    }

    public class UnitFactory(List<UnitGroup> unitGroups)
    {
        public List<UnitGroup> UnitGroups = unitGroups;

        public List<Unit> BuildUnits()
        {
            PropagateExponents();
            ValidateSymbols();
            EvaluatePrefixes();

            List<Unit> units = [];
            foreach (var group in UnitGroups)
                units.Add(new Unit(group));

            return units;
        }
        private void PropagateExponents()
        {
            GroupLikeSymbols();
            UnitGroups = UnitGroups.Where(ug => ug.Operation == UnitGroup.UnitOperation.Divide).Count() switch
            {
                0 => UnitGroups,
                1 => SimplePropagation(UnitGroups),
                _ => ComplexPropagation(UnitGroups),
            };
        }

        private void GroupLikeSymbols() { }
        private void ValidateSymbols() { }
        private void EvaluatePrefixes() { }

        private List<UnitGroup> SimplePropagation(List<UnitGroup> input)
        {

            return input;
        }

        private List<UnitGroup> ComplexPropagation(List<UnitGroup> input)
        {

            return input;
        }
    }

    public abstract class AbsUnit
    {
        public abstract double ToBase(double value);
        public abstract double FromBase(double value);
        public double Convert(double value, AbsUnit target) => target.FromBase(ToBase(value));
        protected abstract Dictionary<string, object> UnitStrings { get; }
        public static object ParseUnit(string unitString)
        {
            var derivedTypes = typeof(AbsUnit).Assembly.GetTypes()
                .Where(type => type.IsSubclassOf(typeof(AbsUnit)));

            foreach (var derivedType in derivedTypes)
            {
                var unitStringsProperty = derivedType
                    .GetProperty("UnitStrings", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                if (unitStringsProperty != null)
                {
                    if (unitStringsProperty.GetValue(Activator.CreateInstance(derivedType)) is Dictionary<string, object> unitStrings && 
                        unitStrings.TryGetValue(unitString, out var enumValue))
                        return enumValue;
                }
            }
            return null;
        }
    }

    public abstract class AbsPrefix
    {
        public abstract double Factor { get; }
        public double Convert(double value, AbsPrefix target) => value * Factor / target.Factor;

        public static object ParsePrefix(string prefixString)
        {
            var derivedTypes = typeof(AbsPrefix).Assembly.GetTypes()
                .Where(type => type.IsSubclassOf(typeof(AbsPrefix)));

            foreach (var derivedType in derivedTypes)
            {
                var prefixStringsProperty = derivedType
                    .GetProperty("PrefixStrings", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                if (prefixStringsProperty != null)
                {
                    if (prefixStringsProperty.GetValue(Activator.CreateInstance(derivedType)) is Dictionary<string, object> prefixStrings &&
                        prefixStrings.TryGetValue(prefixString, out var enumValue))
                        return enumValue;
                }
            }
            return null;
        }
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

        public SI_Base() : this(Unit.Meter) { }

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
        protected override Dictionary<string, object> UnitStrings => throw new NotImplementedException();

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
        protected override Dictionary<string, object> UnitStrings => throw new NotImplementedException();

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
        protected override Dictionary<string, object> UnitStrings => throw new NotImplementedException();

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
