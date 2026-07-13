//------------------------------------------------------------------------------
// <copyright file="UserInfoCommand.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

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
        /// <param name="displayName">El nombre de usuario de Discord a
        /// buscar.</param>
        [Command("who")]
        [Summary(
            "Devuelve información sobre el usuario que se indica como " +
            "parámetro o sobre el usuario que envía el mensaje si no se " +
            "indica otro usuario.")]
        public async Task ExecuteAsync(
            [Remainder]
            [Summary("El usuario del que tener información, opcional")]
            string displayName = null)
        {
            if (displayName != null)
            {
                bool exists = this.UserExists(displayName);

                if (!exists)
                {
                    await ReplyAsync(
                        $"No encuentro el usuario '{displayName}' en esta aplicación")
                        .ConfigureAwait(false);

                    return;
                }
            }

            string userName =
                displayName ?? this.GetDisplayName();

            var result = Facade.Instance.GetUserInfo(userName);
            if (result.IsSuccess)
            {
                await ReplyAsync(result.Value).ConfigureAwait(false);
            }
            else
            {
                await ReplyAsync(result.Errors).ConfigureAwait(false);
            }
        }
    }
}
