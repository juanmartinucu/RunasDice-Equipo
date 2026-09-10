using System;
using System.Collections.Generic;

namespace Ucu.Poo.RunasDices.Domain
{
    /// <summary>Administra las cartas que un jugador tiene en su mano.</summary>
    public class Hand
    {
        private readonly List<ICard> cards = new List<ICard>();

        public IReadOnlyList<ICard> Cards => this.cards.AsReadOnly();

        public void Add(ICard card)
        {
            ArgumentNullException.ThrowIfNull(card);
            this.cards.Add(card);
        }

        public bool Remove(ICard card)
        {
            return this.cards.Remove(card);
        }
    }
}
