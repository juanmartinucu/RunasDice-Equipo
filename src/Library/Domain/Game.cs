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
            this.ActivePlayer = player1;
            this.DefenderPlayer = player2;
            this.State = GameState.Preparing;
            this.Phase = GamePhase.Start;
        }

        /// <summary>
        /// Obtiene el primer jugador de la partida.
        /// </summary>
        public Player Player1 { get; private set; }

        /// <summary>
        /// Obtiene el segundo jugador de la partida.
        /// </summary>
        public Player Player2 { get; private set; }

        /// <summary>Obtiene el jugador cuyo turno está activo.</summary>
        public Player ActivePlayer { get; private set; }

        /// <summary>Obtiene el jugador que defiende durante el turno.</summary>
        public Player DefenderPlayer { get; private set; }

        /// <summary>Obtiene el estado actual de la partida.</summary>
        public GameState State { get; private set; }

        /// <summary>Obtiene la fase actual.</summary>
        public GamePhase Phase { get; private set; }

        /// <summary>Obtiene el ganador cuando la partida termina.</summary>
        public Player Winner { get; private set; }

        /// <summary>Inicia la partida.</summary>
        public void Start()
        {
            this.State = GameState.InProgress;
            this.Phase = GamePhase.Start;
        }

        /// <summary>Cambia la fase actual.</summary>
        public void ChangePhase(GamePhase phase)
        {
            this.Phase = phase;
        }

        /// <summary>Cambia los roles al finalizar el turno.</summary>
        public void ChangeTurn()
        {
            Player previousActivePlayer = this.ActivePlayer;
            this.ActivePlayer = this.DefenderPlayer;
            this.DefenderPlayer = previousActivePlayer;
            this.Phase = GamePhase.Start;
        }

        /// <summary>Finaliza la partida y registra el ganador.</summary>
        public void Finish(Player winner)
        {
            ArgumentNullException.ThrowIfNull(winner);
            this.Winner = winner;
            this.State = GameState.Finished;
        }

        /// <summary>Obtiene el oponente del jugador indicado.</summary>
        public Player GetOpponent(Player player)
        {
            ArgumentNullException.ThrowIfNull(player);
            return player == this.Player1 ? this.Player2 : this.Player1;
        }
    }

    /// <summary>Estados posibles de una partida.</summary>
    public enum GameState
    {
        Preparing,
        InProgress,
        Finished,
    }

    /// <summary>Fases posibles de un turno.</summary>
    public enum GamePhase
    {
        Start,
        Draw,
        Main,
        Combat,
        End,
    }
}
