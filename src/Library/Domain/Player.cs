//------------------------------------------------------------------------------
// <copyright file="Player.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Ucu.Poo.RunasDices.Domain
{
    /// <summary>
    /// Esta clase representa a un jugador para un usuario en el juego.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Player"/>.
        /// </summary>
        /// <param name="user">El usuario correspondiente a este jugador.</param>
        public Player(User user)
        {
            ArgumentNullException.ThrowIfNull(user);

            this.User = user;
            this.Life = 20;
            this.Deck = new Deck();
            this.Hand = new Hand();
            this.Board = new Board();
            this.Graveyard = new Graveyard();
        }

        /// <summary>
        /// Obtiene el usuario correspondiente a este jugador.
        /// </summary>
        public User User { get; private set; }

        /// <summary>Obtiene la vida actual del jugador.</summary>
        public int Life { get; private set; }

        /// <summary>Obtiene el Ether disponible del jugador.</summary>
        public int Ether { get; private set; }

        /// <summary>Obtiene el mazo del jugador.</summary>
        public Deck Deck { get; }

        /// <summary>Obtiene la mano del jugador.</summary>
        public Hand Hand { get; }

        /// <summary>Obtiene el tablero del jugador.</summary>
        public Board Board { get; }

        /// <summary>Obtiene el cementerio del jugador.</summary>
        public Graveyard Graveyard { get; }

        /// <summary>Recibe daño y reduce la vida del jugador.</summary>
        public void ReceiveDamage(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("El daño no puede ser negativo.", nameof(amount));
            }

            this.Life -= amount;
        }

        /// <summary>Recupera vida.</summary>
        public void Heal(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("La curación no puede ser negativa.", nameof(amount));
            }

            this.Life += amount;
        }

        /// <summary>Establece el Ether disponible al inicio del turno.</summary>
        public void ResetEther()
        {
            this.Ether = 3;
        }

        /// <summary>Agrega Ether temporal al jugador.</summary>
        public void AddEther(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("El Ether no puede ser negativo.", nameof(amount));
            }

            this.Ether += amount;
        }

        /// <summary>Paga un costo usando el Ether disponible.</summary>
        public void SpendEther(int amount)
        {
            if (amount < 0 || amount > this.Ether)
            {
                throw new InvalidOperationException("El jugador no tiene Ether suficiente.");
            }

            this.Ether -= amount;
        }

        /// <summary>Roba una carta y la agrega a la mano.</summary>
        public ICard DrawCard()
        {
            ICard card = this.Deck.Draw();
            if (card != null)
            {
                this.Hand.Add(card);
            }

            return card;
        }

        /// <summary>Mueve una carta de la mano al cementerio.</summary>
        public void Discard(ICard card)
        {
            if (this.Hand.Remove(card))
            {
                this.Graveyard.Add(card);
            }
        }
    }
}
