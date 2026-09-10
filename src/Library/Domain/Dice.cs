using System;

namespace Ucu.Poo.RunasDices.Domain
{
    /// <summary>Representa un dado configurable del juego.</summary>
    public class Dice
    {
        private readonly int[] faces;

        public Dice(DiceType type, params int[] faces)
        {
            ArgumentNullException.ThrowIfNull(faces);
            if (faces.Length == 0)
            {
                throw new ArgumentException("El dado debe tener caras.", nameof(faces));
            }

            this.Type = type;
            this.faces = faces;
        }

        public DiceType Type { get; }

        public DiceResult Roll()
        {
            Random random = new Random();
            return new DiceResult(this.faces[random.Next(this.faces.Length)]);
        }
    }

    public enum DiceType
    {
        StandardNumeric,
        Power,
        Risk,
        Healing,
        Rune,
    }
}
