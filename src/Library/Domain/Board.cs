using System;
using System.Collections.Generic;

namespace Ucu.Poo.RunasDices.Domain
{
    /// <summary>Administra las cartas permanentes que están en juego.</summary>
    public class Board
    {
        private readonly List<Creature> creatures = new List<Creature>();
        private readonly List<Upgrade> upgrades = new List<Upgrade>();

        public IReadOnlyList<Creature> Creatures => this.creatures.AsReadOnly();

        public IReadOnlyList<Upgrade> Upgrades => this.upgrades.AsReadOnly();

        public void AddCreature(Creature creature)
        {
            ArgumentNullException.ThrowIfNull(creature);
            this.creatures.Add(creature);
        }

        public void AddUpgrade(Upgrade upgrade)
        {
            ArgumentNullException.ThrowIfNull(upgrade);
            this.upgrades.Add(upgrade);
            upgrade.Equip();
        }

        public bool RemoveCreature(Creature creature)
        {
            return this.creatures.Remove(creature);
        }
    }
}
