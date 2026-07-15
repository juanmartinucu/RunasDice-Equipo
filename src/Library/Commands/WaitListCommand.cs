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
            IReadOnlyList<string> usersWaiting = Facade.Instance.GetUsersWaitingForOpponent();

            if (usersWaiting.Count == 0)
            {
                await ReplyAsync(
                    WaitListCommandMessages.NoPlayersWaitingToPlay)
                    .ConfigureAwait(false);
            }
            else
            {
                string users = string.Join(", ", usersWaiting.Select(user => $"'{user}'"));
                await ReplyAsync(
                    WaitListCommandMessages.PlayersWaiting(users))
                    .ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// Esta clase contiene todos los mensajes retornados por <see
    /// cref="WaitListCommand"/>.
    /// </summary>
    public static class WaitListCommandMessages
    {
        /// <summary> No hay jugadores esperando para jugar.</summary>
        public const string NoPlayersWaitingToPlay =
            "**Error**: No hay jugadores esperando por oponente. Usa `!play` para inscribirte.";

        /// <summary>Lista de usuarios esperando.</summary>
        public static string PlayersWaiting(string users) =>
            $"**Okay**: esperan por oponente: {users}. Usa !play <usuario> para unirte.";
    }
}
