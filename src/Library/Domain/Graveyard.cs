using System;
using System.Collections.Generic;

namespace Ucu.Poo.RunasDices.Domain
{
    /// <summary>Conserva las cartas descartadas, usadas o destruidas.</summary>
    public class Graveyard
    {
        private readonly List<ICard> cards = new List<ICard>();

        public IReadOnlyList<ICard> Cards => this.cards.AsReadOnly();

        public void Add(ICard card)
        {
            ArgumentNullException.ThrowIfNull(card);
            this.cards.Add(card);
        }
    }
}
