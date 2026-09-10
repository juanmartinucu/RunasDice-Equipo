using System;

namespace Ucu.Poo.RunasDices.Domain
{
    /// <summary>Representa un efecto básico aplicable a un jugador.</summary>
    public class Effect
    {
        public Effect(EffectType type, int amount)
        {
            this.Type = type;
            this.Amount = amount;
        }

        public EffectType Type { get; }

        public int Amount { get; }

        public void Apply(Player player)
        {
            ArgumentNullException.ThrowIfNull(player);
            if (this.Type == EffectType.Damage)
            {
                player.ReceiveDamage(this.Amount);
            }
            else if (this.Type == EffectType.Heal)
            {
                player.Heal(this.Amount);
            }
            else if (this.Type == EffectType.AddEther)
            {
                player.AddEther(this.Amount);
            }
        }
    }

    public enum EffectType
    {
        Damage,
        Heal,
        AddEther,
        DrawCard,
        ModifyStats,
    }
}
