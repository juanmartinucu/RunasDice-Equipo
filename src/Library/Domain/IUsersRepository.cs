//------------------------------------------------------------------------------
// <copyright file="IUsersRepository.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;

namespace Ucu.Poo.RunasDices.Domain
{
    /// <summary>
    /// Un tipo de repositorio de usuarios que se implementa con
    /// <see cref="Ucu.Poo.RunasDices.Domain.UsersRepository"/> en producción
    /// y por un mock en las pruebas.
    /// </summary>
    public interface IUsersRepository
    {
        /// <summary>
        /// Obtiene una colección con todos los usuarios <see cref="User"/>.
        /// </summary>
        IReadOnlyCollection<User> AllUsers { get; }

        /// <summary>
        /// Busca un usuario <see cref="User"/> en el repositorio que tenga
        /// el nombre de usuario de Discord provisto..
        /// </summary>
        /// <param name="userName">El nombre de usuario de Discord a
        /// buscar.</param>
        /// <returns>El usuario encontrado, si lo hubiera;<c>null</c> en caso
        /// contrario.</returns>
        User Find(string userName);

        /// <summary>
        /// Agrega un nuevo usuario <see cref="User"/> al repositorio.
        /// </summary>
        /// <param name="userName">El nombre de usuario de Discord del nuevo
        /// usuario.</param>
        /// <returns>El usuario agregado.</returns>
        User Add(string userName);
    }
}