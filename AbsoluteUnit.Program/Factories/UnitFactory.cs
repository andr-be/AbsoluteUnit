using AbsoluteUnit.Program.Structures;
using AbsoluteUnit.Program.Units;

namespace AbsoluteUnit.Program.Factories;

public class UnitFactory : IUnitFactory
{
    public List<UnitGroup> UnitGroups { get; set; }

    public UnitFactory() => UnitGroups = [];

    public UnitFactory(UnitGroup unitGroup) => UnitGroups = [unitGroup];

    public UnitFactory(List<UnitGroup> unitGroups) => UnitGroups = unitGroups;

    private readonly List<Dictionary<string, object>> ValidSymbols =
    [
        SIBase.ValidUnitStrings,
        SIDerived.ValidUnitStrings,
        USCustomary.ValidUnitStrings,
        Miscellaneous.ValidUnitStrings,
    ];

    public List<AbsUnit> BuildUnits(List<UnitGroup>? unitGroups = null)
    {
        UnitGroups = unitGroups is null ? UnitGroups : unitGroups;

        PropagateExponents();
        ValidateSymbols();
        EvaluatePrefixes();

        return UnitGroups
            .Select(CreateUnit)
            .ToList();
    }

    private void PropagateExponents()
    {
        GroupLikeSymbols();
        UnitGroups = UnitGroups
            .Where(ug => ug.Operation == UnitGroup.UnitOperation.Divide)
            .Count() switch
            {
                0 => UnitGroups,
                1 => SimplePropagation(UnitGroups),
                _ => ComplexPropagation(UnitGroups),
            };
    }

    private static List<UnitGroup> SimplePropagation(List<UnitGroup> input)
    {
        bool reachedDivisionSign = false;

        return input
            .Select(current =>
            {
                if (current.Operation == UnitGroup.UnitOperation.Divide)
                    reachedDivisionSign = true;

                return reachedDivisionSign
                    ? current with { Exponent = current.Exponent * -1 }
                    : current;
            })
            .ToList();
    }

    private static List<UnitGroup> ComplexPropagation(List<UnitGroup> input) =>
        input.Select(current =>
        {
            return current.Operation == UnitGroup.UnitOperation.Divide
                ? current = current with { Exponent = current.Exponent * -1 }
                : current;
        })
        .ToList();

    private void GroupLikeSymbols() =>
        UnitGroups = UnitGroups
            .GroupBy(unitGroup => unitGroup.UnitSymbol)
            .Select(group =>
            {
                var operation = group.First().Operation;
                var symbol = group.Key;
                var exponent = group.Sum(ug => ug.Exponent);
                return new UnitGroup(operation, symbol, exponent);
            })
            .ToList();

    private void ValidateSymbols()
    {
        foreach (var unit in UnitGroups)
            if (!IsInUnitDictionaries(unit))
                throw new KeyNotFoundException($"{unit.UnitSymbol} is not a supported unit!");
    }

    private bool IsInUnitDictionaries(UnitGroup? unit) =>
        CheckUnitDictionaries(unit?.UnitSymbol) || CheckUnitDictionaries(unit?.UnitSymbol.Remove(0, 1));

    private bool CheckUnitDictionaries(string? current)
    {
        if (string.IsNullOrEmpty(current))
            return false;

        foreach (var dict in ValidSymbols)
            if (dict.ContainsKey(current))
                return true;

        return false;
    }

    private void EvaluatePrefixes()
    {
        for (int i = 0; i < UnitGroups.Count; i++)
            if (StartsWithValidPrefix(UnitGroups[i]))
                UnitGroups[i] = UnitGroups[i] with { HasPrefix = true };
    }

    private static SIPrefix? GetPrefix(char firstChar) =>
        SIPrefix.ValidPrefixStrings.TryGetValue($"{firstChar}", out var value)
            ? value
            : null;

    private bool StartsWithValidPrefix(UnitGroup group)
    {
        var unit = group.UnitSymbol;

        return unit.Length > 1
            && !CheckUnitDictionaries(group.UnitSymbol)
            && GetPrefix(unit[0]) != null;
    }

    private AbsUnit CreateUnit(UnitGroup group)
    {
        SIPrefix? prefix;
        if (group.HasPrefix)
        {
            prefix = GetPrefix(group.UnitSymbol[0]);
            group = group with { UnitSymbol = group.UnitSymbol.Remove(0, 1) };
        }
        else prefix = new SIPrefix(SIPrefix.Prefixes._None);

        foreach (var stringDict in ValidSymbols)
        {
            stringDict.TryGetValue(group.UnitSymbol, out var unit);
            if (unit is not null)
                return new AbsUnit((IUnit)unit, group.Exponent, prefix!);
        }
        throw new KeyNotFoundException($"{group.UnitSymbol} not found in key database...");
    }
}
