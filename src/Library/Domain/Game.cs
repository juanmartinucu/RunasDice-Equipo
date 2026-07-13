//------------------------------------------------------------------------------
// <copyright file="Game.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Ucu.Poo.RunasDices.Domain
{
    /// <summary>
    /// Esta clase representa una partida entre dos jugadores.
    /// </summary>
    public class Game
    {
        /// <summary>
        /// El primer jugador de la partida.
        /// </summary>
        public Player Player1 { get; private set; }

        /// <summary>
        /// El segundo jugador de la partida.
        /// </summary>
        public Player Player2 { get; private set; }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Game"/>.
        /// </summary>
        /// <param name="player1">El primer jugador de la partida.</param>
        /// <param name="player2">El segundo jugador de la partida.</param>
        public Game(Player player1, Player player2)
        {
            ArgumentNullException.ThrowIfNull(player1);
            ArgumentNullException.ThrowIfNull(player2);

            this.Player1 = player1;
            this.Player2 = player2;
        }
    }
}
