//------------------------------------------------------------------------------
// <copyright file="CommandHelper.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Ucu.Poo.RunasDices.Discord
{
    /// <summary>
    /// Esta clase es la clase base para todos los comandos del bot. Permite
    /// conocer el nombre visible del usuario que ejecuta el comando o de un
    /// usuario cualquiera y provee un mecanismo para poder probar los comandos
    /// del bot pero sin ejecutar el bot.
    /// </summary>
    public abstract class CommandBase : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// Obtiene o establece una acción a ejecutar cada vez que el comando
        /// devuelve una respuesta con <see cref="ModuleBase{T}.ReplyAsync(string,
        /// bool, Embed, RequestOptions, AllowedMentions, MessageReference,
        /// MessageComponent, ISticker[], Embed[], MessageFlags)"/>. Esto permite
        /// obtener las respuestas retornadas por el comando en los casos de prueba.
        /// </summary>
        public Action<string> OnReplyAsync { get; set; }

        /// <summary>
        /// Retorna el nombre visible del usuario de Discord que ejecuta este
        /// comando. En el contexto de este comando el nombre visible del
        /// usuario retornado es un usuario válido en el servidor actual.
        /// </summary>
        /// <param name="name">El nombre de usuario a obtener.</param>
        /// <returns>
        /// Cuando no se provee un nombre, retorna el nombre visible del usuario
        /// que envía el comando en el servidor de Discord del contexto
        /// provisto. Cuando se provee un nombre, asume que ese nombre puede ser
        /// el nombre visible, el nickname, o el nombre de usuario global de
        /// Discord, pero retorna el nombre visible de usuario. Esto permite
        /// usar de forma consistente el nombre visible del usuario
        /// independiente de cómo se obtenga.
        /// </returns>
        protected virtual string GetDisplayName(string name = null)
        {
            if (name == null)
            {
                name = this.Context.Message.Author.Username;
            }

            foreach (SocketGuildUser user in this.Context.Guild.Users)
            {
                if (user.Username == name
                    || user.DisplayName == name
                    || user.Nickname == name
                    || user.GlobalName == name)
                {
                    return user.DisplayName;
                }
            }

            return name;
        }

        /// <summary>
        /// Determina si existe un usuario en el contexto de este comando.
        /// </summary>
        /// <param name="name">El nombre del usuario a buscar.</param>
        /// <returns>
        /// Busca un usuario cuyo nombre visible, nickname, o nombre de usuario
        /// global de Discord coincida con el nombre provisto como parámetro.
        /// Retorna <c>true</c> si existe y <c>false</c> en caso contrario.
        /// </returns>
        protected virtual bool UserExists(string name)
        {
            if (name == null)
            {
                return false;
            }

            foreach (SocketGuildUser user in this.Context.Guild.Users)
            {
                if (user.Username == name
                    || user.DisplayName == name
                    || user.Nickname == name
                    || user.GlobalName == name)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override Task<IUserMessage> ReplyAsync(string message = null,
            bool isTTS = false, Embed embed = null, RequestOptions options = null,
            AllowedMentions allowedMentions = null, MessageReference messageReference = null,
            MessageComponent components = null, ISticker[] stickers = null,
            Embed[] embeds = null, MessageFlags flags = MessageFlags.None)
        {
            OnReplyAsync?.Invoke(message);
            return base.ReplyAsync(
                message,
                isTTS,
                embed,
                options,
                allowedMentions,
                messageReference,
                components,
                stickers,
                embeds,
                flags);
        }
    }
}
