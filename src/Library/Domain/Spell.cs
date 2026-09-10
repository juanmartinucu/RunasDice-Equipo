using System;

namespace Ucu.Poo.RunasDices.Domain
{
    /// <summary>Representa una carta que se resuelve inmediatamente.</summary>
    public class Spell : ICard
    {
        public Spell(string name, int cost, string description, Effect effect)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentException.ThrowIfNullOrWhiteSpace(description);
            ArgumentNullException.ThrowIfNull(effect);
            this.Name = name;
            this.Cost = cost;
            this.Description = description;
            this.Effect = effect;
        }

        public string Name { get; }

        public int Cost { get; }

        public string Description { get; }

        public Effect Effect { get; }
    }
}
