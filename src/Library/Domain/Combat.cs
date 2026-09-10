using System;

namespace Ucu.Poo.RunasDices.Domain
{
    /// <summary>Resuelve un enfrentamiento entre dos criaturas.</summary>
    public class Combat
    {
        public Combat(Creature attacker, Creature defender)
        {
            ArgumentNullException.ThrowIfNull(attacker);
            ArgumentNullException.ThrowIfNull(defender);
            this.Attacker = attacker;
            this.Defender = defender;
        }

        public Creature Attacker { get; }

        public Creature Defender { get; }

        public bool Resolve(Dice attackDice, Dice defenseDice)
        {
            ArgumentNullException.ThrowIfNull(attackDice);
            ArgumentNullException.ThrowIfNull(defenseDice);
            int attack = this.Attacker.Attack + attackDice.Roll().Value;
            int defense = this.Defender.Defense + defenseDice.Roll().Value;
            if (attack >= defense)
            {
                this.Defender.Destroy();
                return true;
            }

            return false;
        }
    }
}
