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

// cSpell:ignore waitlist

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
        [Command("waitlist")]
        [Summary("Muestra la lista de usuarios esperando por un oponente.")]
        public async Task ExecuteAsync()
        {
            var result = Facade.Instance.GetUsersWaitingForOpponent();

            if (result.IsFailure)
            {
                await ReplyAsync($"**Error**: {result.Errors}")
                    .ConfigureAwait(false);

                return;
            }

            IReadOnlyList<string> usersWaiting = result.Value;

            if (usersWaiting.Count == 0)
            {
                await ReplyAsync(
                    "**Error**: No hay jugadores esperando por oponente. Usa `!play` para inscribirte.")
                    .ConfigureAwait(false);
            }
            else
            {
                string users = string.Join(", ", usersWaiting.Select(user => $"'{user}'"));
                await ReplyAsync(
                    $"**Okay**: esperan por oponente: {users}. Usa !play <usuario> para unirte.")
                    .ConfigureAwait(false);
            }
        }
    }
}
