//------------------------------------------------------------------------------
// <copyright file="WaitListCommandMessages.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace Ucu.Poo.RunasDices.Commands
{
    /// <summary>
    /// Esta clase contiene todos los mensajes retornados por la clase <see
    /// cref="WaitListCommand"/>.
    /// </summary>
    public static class WaitListCommandMessages
    {
        /// <summary> No hay jugadores esperando para jugar.</summary>
        public const string NoPlayersWaitingToPlay =
            "No hay jugadores esperando por oponente. Usa `!play` para inscribirte.";

        /// <summary>Lista de usuarios esperando.</summary>
        public static string PlayersWaiting(string users) =>
            $"**Okay**: esperan por oponente: {users}. Usa `!play <usuario>` para unirte.";
    }
}
