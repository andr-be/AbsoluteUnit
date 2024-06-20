namespace AbsoluteUnit.Program.Structures
{
    public record MeasurementGroup(double Quantity, int Exponent, List<UnitGroup> Units)
    {
        public override string ToString() =>
            $"{Quantity}{(Exponent != 0 ? "e" + Exponent : "")} {string.Join(".", Units)}";
    }
}
