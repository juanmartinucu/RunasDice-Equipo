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
        }

        /// <summary>
        /// Obtiene el usuario correspondiente a este jugador.
        /// </summary>
        public User User { get; private set; }
    }
}
