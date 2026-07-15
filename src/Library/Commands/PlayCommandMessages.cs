//------------------------------------------------------------------------------
// <copyright file="PlayCommandMessages.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

namespace Ucu.Poo.RunasDices.Commands
{
    /// <summary>
    /// Esta clase contiene todos los mensajes retornados por la clase <see
    /// cref="PlayCommand"/>.
    /// </summary>
    public static class PlayCommandMessages
    {
        /// <summary>Obtiene ayuda del comando.</summary>
        public static string CommandUsage =>
            "Usa `!play` o `!play <usuario>`.";

        /// <summary>Usuario agregado a la lista de espera.</summary>
        public static string UserAddedToWaitingList(string userName) =>
            $"**Okay**: '{userName}' agregado a lista de espera para jugar.";

        /// <summary>Partida iniciada.</summary>
        public static string GameStarted(string opponentName) =>
            $"**Okay**: unido a la partida con '{opponentName}'.";
    }
}
