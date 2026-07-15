//------------------------------------------------------------------------------
// <copyright file="CommandBase.cs" company="Universidad Católica del Uruguay">
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
    /// obtener mediante el método <see
    /// cref="CommandBase.GetSenderOrAliasDisplayName(Parameters)"/> el nombre
    /// visible del usuario que ejecuta el comando o de un alias si los
    /// parámetros lo incluyen. El método <see
    /// cref="CommandBase.GetDisplayName(string)"/> permite obtener el nombre
    /// visible de un usuario cualquiera. Además provee un mecanismo para poder
    /// probar los comandos del bot pero sin ejecutar el bot con la propiedad
    /// <see cref="CommandBase.OnReplyAsync"/>.
    /// </summary>
    public abstract class CommandBase : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// Obtiene o establece una acción a ejecutar cada vez que el comando
        /// devuelve una respuesta con <see
        /// cref="ModuleBase{T}.ReplyAsync(string, bool, Embed, RequestOptions,
        /// AllowedMentions, MessageReference, MessageComponent, ISticker[],
        /// Embed[], MessageFlags)"/>. Esto permite obtener las respuestas
        /// retornadas por el comando en los casos de prueba.
        /// </summary>
        public Action<string> OnReplyAsync { get; set; }

        /// <summary>
        /// Retorna el nombre visible del usuario de Discord que ejecuta este
        /// comando o del alias indicado como parámetro si lo hubiera. En el
        /// contexto de este comando el nombre visible del usuario retornado es
        /// un usuario válido en el servidor actual, excepto cuando es un alias
        /// que no se valida ni se busca el en servidor.
        /// </summary>
        /// <param name="parameters">La lista de parámetros recibida por el
        /// comando.</param>
        /// <returns>
        /// El valor retornado es consistente con el usado en el método <see
        /// cref="CommandBase.GetDisplayName(string)"/>. Esto permite usar de
        /// forma consistente el nombre visible del usuario independiente de
        /// cómo se obtenga.
        /// </returns>
        protected virtual string GetSenderOrAliasDisplayName(Parameters parameters)
        {
            ArgumentNullException.ThrowIfNull(parameters);

            if (parameters.AliasIncluded)
            {
                return parameters.Alias;
            }

            return this.Context.Message.Author.Username;
        }

        /// <summary>
        /// Retorna el nombre visible del usuario cuyo nombre visible, nickname,
        /// o nombre global que se recibe como argumento. Esto permite usar de
        /// forma consistente el nombre visible del usuario independiente de
        /// cómo se obtenga.
        /// </summary>
        /// <param name="name">El nombre visible, nickname, o nombre global de
        /// un usuario.</param>
        /// <returns>El nombre visible de ese usuario.</returns>
        protected virtual string GetDisplayName(string name)
        {
            ArgumentException.ThrowIfNullOrEmpty(name);
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

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

        /// <inheritdoc/>
        protected override Task<IUserMessage> ReplyAsync(
            string message = null,
            bool isTTS = false,
            Embed embed = null,
            RequestOptions options = null,
            AllowedMentions allowedMentions = null,
            MessageReference messageReference = null,
            MessageComponent components = null,
            ISticker[] stickers = null,
            Embed[] embeds = null,
            MessageFlags flags = MessageFlags.None)
        {
            this.OnReplyAsync?.Invoke(message);
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
