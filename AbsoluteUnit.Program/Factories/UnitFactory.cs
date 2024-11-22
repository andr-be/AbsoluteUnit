using AbsoluteUnit.Program.Parsers.ParserGroups;
using AbsoluteUnit.Program.Structures;
using AbsoluteUnit.Program.UnitTypes;

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

        return ProcessUnitGroups(unitGroups)
            .Select(CreateUnit)
            .ToList();
    }

    public List<Unit> BuildUnits(UnitGroup? unitGroup = null)
    {
        if (unitGroup is null) return [];

        return BuildUnits([unitGroup]);
    }

    public static Unit BuildUnit
    (
        IUnitType unitType, 
        int exponent = 1, 
        SIPrefix.Prefixes prefix = SIPrefix.Prefixes._None
   
    ) => new(unitType, exponent, new(prefix));


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
                {
                    reachedDivisionSign = true;
                }

                return reachedDivisionSign
                    ? current with { Exponent = current.Exponent * -1 }
                    : current;
            }
        ).ToList();
    }

    private static List<UnitGroup> ComplexPropagation(List<UnitGroup> input) =>
        input.Select(current => current.Operation == UnitGroup.UnitOperation.Divide
            ? current with { Exponent = current.Exponent * -1 }
            : current).ToList();

    private void ValidateSymbols(List<UnitGroup> groups)
    {
        foreach (var unit in groups)
            if (!IsInUnitDictionaries(unit))
                throw new CommandError(ErrorCode.UnrecognisedUnit, $"{unit.UnitSymbol} is not a recognised unit.");
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
}