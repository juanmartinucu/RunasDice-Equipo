// Esta clase no es una clase de prueba, sino una clase base para las clases de
// prueba de todos los comandos.

using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Moq;
using Moq.Protected;
using Ucu.Poo.RunasDices.Discord;
using Ucu.Poo.RunasDices.Domain;

namespace Ucu.Poo.RunasDices.Tests
{
    public class CommandTestBase<T>  where T: CommandBase
    {
        // El nombre del usuario que ejecuta el comando
        protected const string SendingUser = "user";

        // Un mock del comando a probar que independiza el comando de Discord
        protected Mock<T> CommandMock;

        // La lista de respuestas retornados por el comando
        protected List<string> Replies;

        // La primera respuesta del comando o null si no hay respuestas
        protected string Reply
        {
            get
            {
                if (this.Replies.Count >= 0)
                {
                    return Replies[0];
                }

                return null;
            }
        }

        protected void ResetFacadeCreateRepliesAndMock()
        {
            Facade.Reset();
            Replies = new List<string>();
            CommandMock = CreateCommandMock();
        }

        private Mock<T> CreateCommandMock()
        {
            var mock = new Mock<T> { CallBase = true };

            mock
                .Protected()
                .Setup<Task<IUserMessage>>(
                    "ReplyAsync",
                    ItExpr.IsAny<string>(),
                    ItExpr.IsAny<bool>(),
                    ItExpr.IsAny<Embed>(),
                    ItExpr.IsAny<RequestOptions>(),
                    ItExpr.IsAny<AllowedMentions>(),
                    ItExpr.IsAny<MessageReference>(),
                    ItExpr.IsAny<MessageComponent>(),
                    ItExpr.IsAny<ISticker[]>(),
                    ItExpr.IsAny<Embed[]>(),
                    ItExpr.IsAny<MessageFlags>())
                .Callback((
                    string message,
                    bool isTTS,
                    Embed embed,
                    RequestOptions options,
                    AllowedMentions allowedMentions,
                    MessageReference messageReference,
                    MessageComponent components,
                    ISticker[] stickers,
                    Embed[] embeds,
                    MessageFlags flags) =>
                {
                    if (message != null)
                    {
                        Replies.Add(message);
                    }
                })
                .Returns(Task.FromResult<IUserMessage>(null));

            mock
                .Protected()
                .Setup<string>("GetDisplayName", ItExpr.Is<string>(s => s == null))
                .Returns(SendingUser);

            return mock;
        }
    }
}
