//------------------------------------------------------------------------------
// <copyright file="PlayCommand.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

using System;
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
        /// <summary>
        /// Implementa el comando 'play'.
        /// </summary>
        /// <param name="input">Nombre del usuario al que unirse.</param>
        /// <returns>Una tarea asíncrona para representar la ejecución del
        /// comando.</returns>
        [Command("play")]
        [Summary("Sin parámetros te inscribe; con un usuario te une a su partida.")]
        public async Task ExecuteAsync(
            [Remainder]
            [Summary("Usuario que está esperando oponente")]
            string input = null)
        {
            try
            {
                Parameters parameters = new Parameters(input);
                string userName;

                // Cuando no se indica un parámetro, se inscribe al usuario que
                // envía el mensaje o al alias para esperar un oponente.
                if (parameters.IsEmpty)
                {
                    userName = this.GetSenderOrAliasDisplayName(parameters);

                    Facade.Instance.AddUserToWaitingList(userName);
                    await this.ReplyAsync(
                        PlayCommandMessages.UserAddedToWaitingList(userName))
                        .ConfigureAwait(false);

                    return;
                }

                // Cuando hay más de un parámetro es un error y se informa cómo usar
                // el comando.
                if (parameters.Count != 1)
                {
                    await this.ReplyAsync(PlayCommandMessages.CommandUsage)
                        .ConfigureAwait(false);

                    return;
                }

                // Cuando se indica exactamente un parámetro, se asume que es el
                // nombre de un usuario que está esperando un oponente para jugar.
                string opponentId = parameters[0];
                userName = this.GetSenderOrAliasDisplayName(parameters);
                string opponentName = this.GetDisplayName(opponentId);

                Result<Game> result = Facade.Instance.StartGame(userName, opponentName);

                if (result.IsSuccess)
                {
                    await this.ReplyAsync(
                        PlayCommandMessages.GameStarted(opponentName))
                        .ConfigureAwait(false);
                }
                else
                {
                    await this.ReplyAsync(result.Errors).ConfigureAwait(false);
                }
            }
            catch (ArgumentException exception)
            {
                await this.ReplyAsync(
                    UserInfoCommandMessages.Error(exception.Message))
                    .ConfigureAwait(false);
            }
        }
    }
}
