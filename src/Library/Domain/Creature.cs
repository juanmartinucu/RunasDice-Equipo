using System;
using System.Collections.Generic;

namespace Ucu.Poo.RunasDices.Domain
{
    /// <summary>Representa una criatura que puede estar en el tablero.</summary>
    public class Creature : ICard
    {
        public Creature(string name, int cost, string description, int attack, int defense)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentException.ThrowIfNullOrWhiteSpace(description);
            this.Name = name;
            this.Cost = cost;
            this.Description = description;
            this.Attack = attack;
            this.Defense = defense;
            this.Upgrades = new List<Upgrade>();
        }

        public string Name { get; }

        public int Cost { get; }

        public string Description { get; }

        public int Attack { get; private set; }

        public int Defense { get; private set; }

        public bool IsDestroyed { get; private set; }

        public List<Upgrade> Upgrades { get; }

        public void AddUpgrade(Upgrade upgrade)
        {
            ArgumentNullException.ThrowIfNull(upgrade);
            this.Upgrades.Add(upgrade);
        }

        public void ModifyStats(int attackChange, int defenseChange)
        {
            this.Attack += attackChange;
            this.Defense += defenseChange;
        }

        public void Destroy()
        {
            this.IsDestroyed = true;
        }
    }
}
