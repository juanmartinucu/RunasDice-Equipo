//------------------------------------------------------------------------------
// <copyright file="PlayCommand.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.CodeDom.Compiler;
using System.Threading.Tasks;
using Discord.Commands;
using Ucu.Poo.RunasDices.Discord;
using Ucu.Poo.RunasDices.Domain;

namespace Ucu.Poo.RunasDices.Commands
{
    /// <summary>
    /// Esta clase implementa el comando 'play' del bot. Este comando permite al
    /// usuario que envía el mensaje inscribirse para esperar un oponente, o
    /// unirse a otro usuario que esté esperando un oponente: cuando no se
    /// indica un parámetro al comando, se inscribe al usuario que envía el
    /// mensaje para esperar un oponente; cuando se indica un parámetro, y ese
    /// parámetro corresponde con un usuario que está esperando para jugar, se
    /// crea una partida entre el usuario que envía el mensaje y ese usuario que
    /// está esperando un oponente para jugar.
    /// </summary>
    public class PlayCommand : CommandBase
    {
        private static readonly char[] separator = new[] { ' ' };

        /// <summary>
        /// Implementa el comando 'play'.
        /// </summary>
        /// <param name="parameters">Nombre del usuario al que unirse.</param>
        [Command("play")]
        [Summary("Sin parámetros te inscribe; con un usuario te une a su partida.")]
        public async Task ExecuteAsync(
            [Remainder]
            [Summary("Usuario que está esperando oponente")]
            string parameters = null)
        {
            // Cuando no se indica un parámetro, se inscribe al usuario que
            // envía el mensaje para esperar un oponente.
            if (string.IsNullOrWhiteSpace(parameters))
            {
                string userName = this.GetDisplayName();

                try
                {
                    Facade.Instance.AddUserToWaitingList(userName);
                    await ReplyAsync(
                        PlayCommandMessages.UserAddedToWaitingList(userName))
                        .ConfigureAwait(false);
                }
                catch (InvalidOperationException exception)
                {
                    await ReplyAsync(exception.Message).ConfigureAwait(false);
                }

                return;
            }

            // Cuando se indica uno o más parámetros, se valida que sea un único
            // parámetro.
            string[] args = parameters.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            if (args.Length != 1)
            {
                await ReplyAsync(PlayCommandMessages.CommandHelp).ConfigureAwait(false);

                return;
            }

            // Cuando se indica exactamente un parámetro, se asume que es el
            // nombre de un usuario que está esperando un oponente para jugar.
            string opponentId = args[0];

            try
            {
                string userName = this.GetDisplayName();
                string opponentName = this.GetDisplayName(opponentId);

                Facade.Instance.StartGame(userName, opponentName);

                await ReplyAsync(
                    PlayCommandMessages.GameStarted(opponentName))
                    .ConfigureAwait(false);
            }
            catch (ArgumentException exception)
            {
                await ReplyAsync(exception.Message).ConfigureAwait(false);
            }
            catch (InvalidOperationException exception)
            {
                await ReplyAsync(exception.Message).ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// Esta clase contiene todos los mensajes retornados por <see
    /// cref="PlayCommand"/>.
    /// </summary>
    public static class PlayCommandMessages
    {
        /// <summary>Usuario agregado a la lista de espera.</summary>
        public static string UserAddedToWaitingList(string userName) =>
            $"**Okay**: '{userName}' agregado a lista de espera para jugar.";

        /// <summary>Ayuda del comando.</summary>
        public static string CommandHelp =>
            "Usa !play o !play <usuario>.";

        /// <summary>Partida iniciada.</summary>
        public static string GameStarted(string opponentName) =>
            $"**Okay**: unido a la partida con '{opponentName}'.";
    }

}