//------------------------------------------------------------------------------
// <copyright file="UsersRepository.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;

using Ucu.Poo.RunasDices.Discord;

namespace Ucu.Poo.RunasDices.Domain
{
    /// <summary>
    /// Esta clase implementa el repositorio de usuarios <see cref="User"/>.
    /// </summary>
    public class UsersRepository : IUsersRepository
    {
        private List<User> users = new List<User>();

        /// <inheritdoc/>
        public IReadOnlyCollection<User> AllUsers
        {
            get { return this.users.AsReadOnly(); }
        }

        /// <inheritdoc/>
        public User Find(string userName)
        {
            return this.users.Find(u => u.UserName == userName);
        }

        /// <inheritdoc/>
        public User Add(string userName)
        {
            User newUser = new User(userName);
            this.users.Add(newUser);
            return newUser;
        }
    }
}
