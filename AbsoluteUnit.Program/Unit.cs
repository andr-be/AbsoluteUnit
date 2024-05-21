using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public double Convert(double value, AbsUnit target)
        {
            double baseValue = ToBase(value);
            return target.FromBase(baseValue);
        }
    }

    public abstract class AbsPrefix
    {
        public abstract double Factor { get; }
        public double Convert(double value, AbsPrefix target) => value * Factor / target.Factor;
    }

    public class SIUnit : AbsUnit
    {
        public enum BaseUnit
        {
            Meter,
            Gram,
            Second,
            Ampere,
            Kelvin,
            Mole,
            Candela
        }

        private readonly BaseUnit _baseUnit;

        public SIUnit(BaseUnit baseUnit)
        {
            _baseUnit = baseUnit;
        }

        public override double ToBase(double value)
        {
            return value;   // SI Base units are already in base form
        }

        public override double FromBase(double value)
        {
            return value;   // SI Base units are already in base form
        }

        public BaseUnit? FromString(string input) => StringUnitPairs[input];

        public Dictionary<string, BaseUnit> StringUnitPairs = new()
        {
            { "m", BaseUnit.Meter },
            { "g", BaseUnit.Gram },
            { "s", BaseUnit.Second },
            { "A", BaseUnit.Ampere },
            { "K", BaseUnit.Kelvin },
            { "mole", BaseUnit.Mole },
            { "cd", BaseUnit.Candela },
        };
    }

    public class USCustomaryUnit : AbsUnit { }

    public class MiscUnit : AbsUnit { }

    public class SIPrefix : AbsPrefix { }
}
