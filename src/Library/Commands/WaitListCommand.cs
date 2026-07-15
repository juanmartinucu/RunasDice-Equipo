//------------------------------------------------------------------------------
// <copyright file="WaitListCommand.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using Ucu.Poo.RunasDices.Discord;
using Ucu.Poo.RunasDices.Domain;

namespace Ucu.Poo.RunasDices.Commands
{
    /// <summary>
    /// Esta clase implementa el comando 'waitlist' del bot. Este comando
    /// muestra los usuarios que están esperando por un oponente para jugar.
    /// </summary>
    public class WaitListCommand : CommandBase
    {
        /// <summary>
        /// Implementa el comando 'waitlist'.
        /// </summary>
        /// <returns>Una tarea asíncrona para representar la ejecución del
        /// comando.</returns>
        [Command("waitlist")]
        [Summary("Muestra la lista de usuarios esperando por un oponente.")]
        public async Task ExecuteAsync()
        {
            IReadOnlyList<string> usersWaiting = Facade.Instance.GetUsersWaitingForOpponent();

            if (usersWaiting.Count == 0)
            {
                await this.ReplyAsync(
                    WaitListCommandMessages.NoPlayersWaitingToPlay)
                    .ConfigureAwait(false);
            }
            else
            {
                string users = string.Join(", ", usersWaiting.Select(user => $"'{user}'"));
                await this.ReplyAsync(
                    WaitListCommandMessages.PlayersWaiting(users))
                    .ConfigureAwait(false);
            }
        }
    }
}
