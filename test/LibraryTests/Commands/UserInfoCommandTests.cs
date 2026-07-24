using System.Threading.Tasks;
using Moq.Protected;
using NUnit.Framework;
using Ucu.Poo.RunasDices.Commands;
using Ucu.Poo.RunasDices.Discord;
using Ucu.Poo.RunasDices.Domain;

namespace Ucu.Poo.RunasDices.Tests.Commands
{
    [TestFixture]
    public class UserInfoCommandTests : CommandTestBase<UserInfoCommand>
    {
        [SetUp]
        public void SetUp()
        {
            this.ResetFacadeCreateRepliesAndMock();
        }

        [Test]
        public async Task ExecuteAsync_WithoutParameters_UsesSendingUserAndRepliesWithUserInfo()
        {
            // Arrange: nada extra; GetSenderOrAliasDisplayName(parameters) devolverá SendingUser

            // Act
            await this.CommandMock.Object.ExecuteAsync().ConfigureAwait(false);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.Reply, Is.Not.Null);
                Assert.That(
                    this.Reply,
                    Is.EqualTo(FacadeMessages.UserIsNew(SendingUser)));
            }
        }

        [Test]
        public async Task ExecuteAsync_WithAliasOnly_UsesAliasAndRepliesWithUserInfo()
        {
            // Arrange
            const string otherUser = "other";
            // El alias se pasa como "as:other". Parameters marcará AliasIncluded = true
            // y CommandBase.GetSenderOrAliasDisplayName (mockeado en CommandTestBase)
            // devolverá 'other' como "sender efectivo".

            // Act
            await this.CommandMock.Object.ExecuteAsync($"as:{otherUser}").ConfigureAwait(false);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.Reply, Is.Not.Null);
                Assert.That(
                    this.Reply,
                    Is.EqualTo(FacadeMessages.UserIsNew(otherUser)));
            }
        }

        [Test]
        public async Task ExecuteAsync_WithNonExistingDisplayName_RepliesUserNotFound()
        {
            // Arrange
            const string unknownUser = "unknown";

            // Simulamos que el usuario NO existe en el contexto de Discord
            this.CommandMock
                .Protected()
                .Setup<bool>("UserExists", unknownUser)
                .Returns(false);

            // Act
            await this.CommandMock.Object.ExecuteAsync(unknownUser).ConfigureAwait(false);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.Reply, Is.Not.Null);
                Assert.That(
                    this.Reply,
                    Is.EqualTo(UserInfoCommandMessages.UserNotFound(unknownUser)));
            }
        }

        [Test]
        public async Task ExecuteAsync_WithMoreThanOneParameter_SendsCommandUsage()
        {
            // Arrange: nada extra

            // Act
            await this.CommandMock.Object.ExecuteAsync("uno dos").ConfigureAwait(false);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.Reply, Is.Not.Null);
                Assert.That(this.Reply, Is.EqualTo(UserInfoCommandMessages.CommandUsage));
            }
        }

        [Test]
        public async Task ExecuteAsync_WhenParametersThrowsInvalidOperationException_SendsErrorMessage()
        {
            // Arrange
            // Forzamos un caso en el que Parameters lance InvalidOperationException,
            // por ejemplo usando dos alias "as:" (según tu implementación de Parameters).


            // Act
            await this.CommandMock.Object.ExecuteAsync("as:uno as:dos").ConfigureAwait(false);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.Reply, Is.Not.Null);
                // El mensaje final lo forma UserInfoCommandMessages.Error(exception.Message)
                // donde exception.Message será "MultipleAliases" o "InvalidAlias"
                Assert.That(
                    this.Reply,
                    Is.EqualTo(UserInfoCommandMessages.Error(ParametersMessages.MultipleAliases)).Or
                        .EqualTo(UserInfoCommandMessages.Error(ParametersMessages.InvalidAlias)));
            }
        }
    }
}
