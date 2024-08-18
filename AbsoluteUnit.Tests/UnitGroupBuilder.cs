using AbsoluteUnit.Program.Structures;

namespace AbsoluteUnit.Tests;

public class UnitGroupBuilder
{
    private UnitGroup.UnitOperation _divMulti = UnitGroup.UnitOperation.Multiply;
    private string _symbol = "";
    private int _exponent = 1;

    public UnitGroupBuilder WithDivMulti(UnitGroup.UnitOperation divMulti)
    {
        _divMulti = divMulti;
        return this;
    }

    public UnitGroupBuilder WithSymbol(string symbol)
    {
        _symbol = symbol;
        return this;
    }

    public UnitGroupBuilder WithExponent(int exponent)
    {
        _exponent = exponent;
        return this;
    }

    public UnitGroup Build() => new(_divMulti, _symbol, _exponent);
    
}
