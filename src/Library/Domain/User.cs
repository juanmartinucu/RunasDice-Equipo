//------------------------------------------------------------------------------
// <copyright file="User.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Ucu.Poo.RunasDices.Domain
{
    /// <summary>
    /// Esta clase representa un usuario de la aplicación.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Obtiene el nombre del usuario.
        /// </summary>
        public string UserName { get; }

        /// <summary>
        /// Crea una nueva instancia de la clase <see cref="User"/>.
        /// </summary>
        /// <param name="userName">El nombre de usuario de este usuario.</param>
        /// <exception cref="ArgumentException">Cuando el nombre de usuario
        /// está en blanco o es <c>null</c>.</exception>
        public User(string userName)
        {
            ArgumentException.ThrowIfNullOrEmpty(userName);

            this.UserName = userName;
        }
    }
}
