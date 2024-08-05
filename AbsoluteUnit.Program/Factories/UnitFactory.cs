using AbsoluteUnit.Program.Structures;
using AbsoluteUnit.Program.Units;

namespace AbsoluteUnit.Program.Factories;

public class UnitFactory : IUnitFactory
{
    private readonly List<Dictionary<string, object>> ValidSymbols =
    [
        SIBase.ValidUnitStrings,
        SIDerived.ValidUnitStrings,
        USCustomary.ValidUnitStrings,
        Miscellaneous.ValidUnitStrings,
    ];

    public List<Unit> BuildUnits(List<UnitGroup>? unitGroups = null)
    {
        if (unitGroups is null || unitGroups.Count == 0)
        {
            return [];
        }

        var processedGroups = ProcessUnitGroups(unitGroups);
        return processedGroups.Select(CreateUnit).ToList();
    }

    public static Unit BuildUnit(IUnitType unitType, int exponent = 1, SIPrefix.Prefixes prefix = SIPrefix.Prefixes._None)
    {
        return new Unit(unitType, exponent, new SIPrefix(prefix));
    }

    private List<UnitGroup> ProcessUnitGroups(List<UnitGroup> unitGroups)
    {
        var processed = PropagateExponents(unitGroups);
        ValidateSymbols(processed);
        return EvaluatePrefixes(processed);
    }

    private static List<UnitGroup> PropagateExponents(List<UnitGroup> groups)
    {
        var groupedSymbols = GroupLikeSymbols(groups);
        return groupedSymbols.Where(ug => ug.Operation == UnitGroup.UnitOperation.Divide).Count() switch
        {
            0 => groupedSymbols,
            1 => SimplePropagation(groupedSymbols),
            _ => ComplexPropagation(groupedSymbols),
        };
    }

    private static List<UnitGroup> GroupLikeSymbols(List<UnitGroup> groups) =>
        groups
            .GroupBy(unitGroup => unitGroup.UnitSymbol)
            .Select(group => new UnitGroup(
                group.First().Operation,
                group.Key,
                group.Sum(ug => ug.Exponent)))
            .ToList();

    private static List<UnitGroup> SimplePropagation(List<UnitGroup> input)
    {
        bool reachedDivisionSign = false;
        return input.Select(current =>
        {
            if (current.Operation == UnitGroup.UnitOperation.Divide)
                reachedDivisionSign = true;
            return reachedDivisionSign
                ? current with { Exponent = current.Exponent * -1 }
                : current;
        }).ToList();
    }

    private static List<UnitGroup> ComplexPropagation(List<UnitGroup> input) =>
        input.Select(current => current.Operation == UnitGroup.UnitOperation.Divide
            ? current with { Exponent = current.Exponent * -1 }
            : current).ToList();

    private void ValidateSymbols(List<UnitGroup> groups)
    {
        foreach (var unit in groups)
        {
            if (!IsInUnitDictionaries(unit))
                throw new KeyNotFoundException($"{unit.UnitSymbol} is not a supported unit!");
        }
    }

    private bool IsInUnitDictionaries(UnitGroup? unit) =>
        CheckUnitDictionaries(unit?.UnitSymbol) ||
        (unit?.UnitSymbol.Length > 1 && CheckUnitDictionaries(unit.UnitSymbol[1..]));

    private bool CheckUnitDictionaries(string? symbol) =>
        !string.IsNullOrEmpty(symbol) && ValidSymbols.Any(dict => dict.ContainsKey(symbol));

    private List<UnitGroup> EvaluatePrefixes(List<UnitGroup> groups) =>
        groups.Select(group => StartsWithValidPrefix(group)
            ? group with { HasPrefix = true }
            : group).ToList();

    private bool StartsWithValidPrefix(UnitGroup group) =>
        group.UnitSymbol.Length > 1 &&
        !CheckUnitDictionaries(group.UnitSymbol) &&
        SIPrefix.ValidPrefixStrings.ContainsKey(group.UnitSymbol[0].ToString());

    private Unit CreateUnit(UnitGroup group)
    {
        SIPrefix prefix = new(SIPrefix.Prefixes._None);
        string symbol = group.UnitSymbol;

        if (group.HasPrefix)
        {
            prefix = SIPrefix.ValidPrefixStrings[symbol[0].ToString()];
            symbol = symbol[1..];
        }

        foreach (var dict in ValidSymbols)
        {
            if (dict.TryGetValue(symbol, out var unitType))
            {
                return new Unit((IUnitType)unitType, group.Exponent, prefix);
            }
        }

        throw new KeyNotFoundException($"{symbol} not found in unit database.");
    }

    // Static methods for common units
    public static Unit Kilogram(int exponent = 1) => UnitFactory.BuildUnit(new SIBase("g"), exponent, SIPrefix.Prefixes.Kilo);
    public static Unit Meter(int exponent = 1) => UnitFactory.BuildUnit(new SIBase("m"), exponent);
    public static Unit Second(int exponent = 1) => UnitFactory.BuildUnit(new SIBase("s"), exponent);
    public static Unit Ampere(int exponent = 1) => UnitFactory.BuildUnit(new SIBase("A"), exponent);
    public static Unit Kelvin(int exponent = 1) => UnitFactory.BuildUnit(new SIBase("K"), exponent);
    public static Unit Candela(int exponent = 1) => UnitFactory.BuildUnit(new SIBase("cd"), exponent);
    public static Unit Mole(int exponent = 1) => UnitFactory.BuildUnit(new SIBase("mol"), exponent);
}