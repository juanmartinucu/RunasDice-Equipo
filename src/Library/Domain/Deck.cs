using System;
using System.Collections.Generic;

namespace Ucu.Poo.RunasDices.Domain
{
    /// <summary>Administra el mazo ordenado de un jugador.</summary>
    public class Deck
    {
        private readonly List<ICard> cards = new List<ICard>();

        public IReadOnlyList<ICard> Cards => this.cards.AsReadOnly();

        public void Add(ICard card)
        {
            ArgumentNullException.ThrowIfNull(card);
            this.cards.Add(card);
        }

        public ICard Draw()
        {
            if (this.cards.Count == 0)
            {
                return null;
            }

            ICard card = this.cards[0];
            this.cards.RemoveAt(0);
            return card;
        }

        public void Shuffle()
        {
            Random random = new Random();
            for (int i = this.cards.Count - 1; i > 0; i--)
            {
                int position = random.Next(i + 1);
                ICard card = this.cards[i];
                this.cards[i] = this.cards[position];
                this.cards[position] = card;
            }
        }
    }
}
