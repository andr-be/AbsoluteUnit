namespace AbsoluteUnit.Program.Structures;

public record UnitGroup(UnitGroup.UnitOperation Operation, string UnitSymbol, int Exponent, bool HasPrefix = false)
{
    public static UnitOperation GetUnitOperation(char c) => c switch
    {
        '/' => UnitOperation.Divide,
        '.' => UnitOperation.Multiply,
        _ => throw new ParseError("invalid DivMulti symbol")
    };

    public enum UnitOperation
    {
        Divide,
        Multiply,
    }

    public override string ToString()
    {
        string operation = Operation == UnitOperation.Divide
                ? "-"
                : "";

        string exponent = Exponent is < 0 or > 1
            ? $"^{operation}{Exponent}"
            : "";

        return $"{UnitSymbol}{exponent}";
    }
}
