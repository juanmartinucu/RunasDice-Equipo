using System;

namespace Ucu.Poo.RunasDices.Domain
{
    /// <summary>Representa una mejora que se equipa a un objetivo.</summary>
    public class Upgrade : ICard
    {
        public Upgrade(string name, int cost, string description, Effect effect)
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

        public bool IsEquipped { get; private set; }

        public void Equip()
        {
            this.IsEquipped = true;
        }
    }
}
