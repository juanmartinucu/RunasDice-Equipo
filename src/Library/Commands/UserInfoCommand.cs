//------------------------------------------------------------------------------
// <copyright file="UserInfoCommand.cs" company="Universidad Católica del Uruguay">
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
    /// Esta clase implementa el comando 'who' del bot. Este comando retorna
    /// información sobre el usuario que envía el mensaje o sobre otro usuario
    /// si se incluye como parámetro.
    /// </summary>
    public class UserInfoCommand : CommandBase
    {
        /// <summary>
        /// Implementa el comando 'who' del bot.
        /// </summary>
        /// <param name="input">El nombre de usuario de Discord a
        /// buscar.</param>
        /// <returns>Una tarea asíncrona para representar la ejecución del
        /// comando.</returns>
        [Command("who")]
        [Summary(
            "Devuelve información sobre el usuario que se indica como " +
            "parámetro o sobre el usuario que envía el mensaje si no se " +
            "indica otro usuario.")]
        public async Task ExecuteAsync(
            [Remainder]
            [Summary("El usuario del que tener información, opcional")]
            string input = null)
        {
            try
            {
                Parameters parameters = new Parameters(input);

                // Cuando hay más de un parámetro es un error y se informa cómo usar
                // el comando
                if (parameters.Count > 1)
                {
                    await this.ReplyAsync(UserInfoCommandMessages.CommandUsage).ConfigureAwait(false);

                    return;
                }

                // Cuando hay un solo parámetro ese parámetro es el nombre del
                // usuario del cual obtener información.
                if (parameters.Count == 1)
                {
                    bool exists = this.UserExists(parameters[0]);

                    if (!exists)
                    {
                        await this.ReplyAsync(
                            UserInfoCommandMessages.UserNotFound(parameters[0]))
                            .ConfigureAwait(false);

                        return;
                    }
                }

                string userName = this.GetSenderOrAliasDisplayName(parameters);

                var result = Facade.Instance.GetUserInfo(userName);
                await this.ReplyAsync(result).ConfigureAwait(false);
            }
            catch (InvalidOperationException exception)
            {
                await this.ReplyAsync(
                    UserInfoCommandMessages.Error(exception.Message))
                    .ConfigureAwait(false);
            }
        }
    }
}
