namespace Ucu.Poo.RunasDices.Domain
{
    /// <summary>Representa el resultado producido por un dado.</summary>
    public class DiceResult
    {
        public DiceResult(int value)
        {
            this.Value = value;
        }

        public int Value { get; }
    }
}
