using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Moq;
using Moq.Protected;
using Ucu.Poo.RunasDices.Discord;
using Ucu.Poo.RunasDices.Domain;

namespace Ucu.Poo.RunasDices.Tests
{
    public class CommandTestBase<T> where T : CommandBase
    {
        protected const string SendingUser = "user";

        protected Mock<T> CommandMock;

        protected List<string> Replies;

        protected string Reply
        {
            get
            {
                if (this.Replies.Count > 0)
                {
                    return this.Replies[0];
                }

                return null;
            }
        }

        protected void ResetFacadeCreateRepliesAndMock()
        {
            Facade.Reset();
            this.Replies = new List<string>();
            this.CommandMock = this.CreateCommandMock();
        }

        private Mock<T> CreateCommandMock()
        {
            var mock = new Mock<T> { CallBase = true };

            // Crea un mock del comando T para guardar en Replies el resultado
            // de la ejecución del comando.
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
                        this.Replies.Add(message);
                    }
                })
                .Returns(Task.FromResult<IUserMessage>(null));

            // Para los tests, el "sender" sin alias siempre es SendingUser.
            mock
                .Protected()
                .Setup<string>("GetSenderOrAliasDisplayName", ItExpr.IsAny<Parameters>())
                .Returns((Parameters p) =>
                {
                    if (p == null)
                    {
                        return SendingUser;
                    }

                    return p.AliasIncluded ? p.Alias : SendingUser;
                });

            // Si en algún test necesitás un comportamiento distinto, lo podés sobrescribir
            // con otro Setup en ese test concreto.

            return mock;
        }
    }
}
