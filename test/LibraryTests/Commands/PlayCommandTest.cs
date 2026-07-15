using System.Threading.Tasks;
using Moq.Protected;
using NUnit.Framework;
using Ucu.Poo.RunasDices.Commands;
using Ucu.Poo.RunasDices.Discord;
using Ucu.Poo.RunasDices.Domain;

namespace Ucu.Poo.RunasDices.Tests.Commands
{
    [TestFixture]
    public class PlayCommandTests : CommandTestBase<PlayCommand>
    {
        private const string opponentUser = "opponent";
        private const string aliasUser = "alias";

        [SetUp]
        public void SetUp()
        {
            this.ResetFacadeCreateRepliesAndMock();
        }

        [Test]
        public async Task ExecuteAsync_WithoutParameters_AddsSenderToWaitingList_AndSendsConfirmation()
        {
            // Arrange: nada extra; GetSenderOrAliasDisplayName usará el usuario que envía el comando.

            // Act
            await this.CommandMock.Object.ExecuteAsync();

            // Assert
            using (Assert.EnterMultipleScope())
            {
                var waitingResult = Facade.Instance.UserIsWaiting(SendingUser);
                Assert.That(waitingResult.IsSuccess, Is.True);
                Assert.That(waitingResult.Value, Is.True);
                Assert.That(this.Reply, Is.Not.Null);
                Assert.That(
                    this.Reply,
                    Is.EqualTo(PlayCommandMessages.UserAddedToWaitingList(SendingUser)));
            }
        }

        [Test]
        public async Task ExecuteAsync_WithMoreThanOneParameter_SendsUsageMessage()
        {
            // Arrange: nada extra

            // Act
            await this.CommandMock.Object.ExecuteAsync("uno dos");

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(this.Reply, Is.Not.Null);
                Assert.That(this.Reply, Is.EqualTo(PlayCommandMessages.CommandUsage));
            }
        }

        [Test]
        public async Task ExecuteAsync_WithOpponentParameter_StartsGame_AndSendsJoinMessage()
        {
            // Arrange
            Facade.Instance.AddUserToWaitingList(opponentUser);

            // GetDisplayName(opponentUser) debe resolver el nombre visible del oponente
            this.CommandMock
                .Protected()
                .Setup<string>("GetDisplayName", opponentUser)
                .Returns(opponentUser);

            // Act
            await this.CommandMock.Object.ExecuteAsync(opponentUser);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                var sendingWaiting = Facade.Instance.UserIsWaiting(SendingUser);
                var opponentWaiting = Facade.Instance.UserIsWaiting(opponentUser);

                Assert.That(sendingWaiting.IsSuccess, Is.True);
                Assert.That(sendingWaiting.Value, Is.False);
                Assert.That(opponentWaiting.IsSuccess, Is.True);
                Assert.That(opponentWaiting.Value, Is.False);

                Assert.That(this.Reply, Is.Not.Null);
                Assert.That(
                    this.Reply,
                    Is.EqualTo(PlayCommandMessages.GameStarted(opponentUser)));
            }
        }

        [Test]
        public async Task ExecuteAsync_WithOpponentParameter_WhenOpponentIsNotWaiting_SendsFacadeErrorMessage()
        {
            // Arrange: el oponente NO se agrega a la lista, StartGame devolverá Failure.
            this.CommandMock
                .Protected()
                .Setup<string>("GetDisplayName", opponentUser)
                .Returns(opponentUser);

            // Act
            await this.CommandMock.Object.ExecuteAsync(opponentUser);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                var sendingWaiting = Facade.Instance.UserIsWaiting(SendingUser);
                var opponentWaiting = Facade.Instance.UserIsWaiting(opponentUser);

                Assert.That(sendingWaiting.IsSuccess, Is.True);
                Assert.That(sendingWaiting.Value, Is.False);
                Assert.That(opponentWaiting.IsSuccess, Is.True);
                Assert.That(opponentWaiting.Value, Is.False);

                Assert.That(this.Reply, Is.Not.Null);
                Assert.That(
                    this.Reply,
                    Is.EqualTo(FacadeMessages.OpponentIsNotWaiting(opponentUser)));
            }
        }

        [Test]
        public async Task ExecuteAsync_WithOpponentParameter_WhenGetDisplayNameThrowsArgumentException_SendsErrorMessage()
        {
            // Arrange
            const string errorMessage = "Nombre de usuario inválido";

            this.CommandMock
                .Protected()
                .Setup<string>("GetDisplayName", opponentUser)
                .Throws(new System.ArgumentException(errorMessage));

            // Act
            await this.CommandMock.Object.ExecuteAsync(opponentUser);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                var sendingWaiting = Facade.Instance.UserIsWaiting(SendingUser);
                var opponentWaiting = Facade.Instance.UserIsWaiting(opponentUser);

                Assert.That(sendingWaiting.IsSuccess, Is.True);
                Assert.That(sendingWaiting.Value, Is.False);
                Assert.That(opponentWaiting.IsSuccess, Is.True);
                Assert.That(opponentWaiting.Value, Is.False);

                Assert.That(this.Reply, Is.Not.Null);
                Assert.That(
                    this.Reply,
                    Is.EqualTo(UserInfoCommandMessages.Error(errorMessage)));
            }
        }

        [Test]
        public async Task ExecuteAsync_WithAliasOnly_AddsAliasToWaitingList_AndSendsConfirmation()
        {
            // Arrange: nada extra, el mock de GetSenderOrAliasDisplayName ya usa Alias si existe.

            // Act
            await this.CommandMock.Object.ExecuteAsync($"as:{aliasUser}");

            // Assert
            using (Assert.EnterMultipleScope())
            {
                var sendingWaiting = Facade.Instance.UserIsWaiting(SendingUser);
                var aliasWaiting = Facade.Instance.UserIsWaiting(aliasUser);

                Assert.That(sendingWaiting.IsSuccess, Is.True);
                Assert.That(sendingWaiting.Value, Is.False);

                Assert.That(aliasWaiting.IsSuccess, Is.True);
                Assert.That(aliasWaiting.Value, Is.True);

                Assert.That(this.Reply, Is.Not.Null);
                Assert.That(
                    this.Reply,
                    Is.EqualTo(PlayCommandMessages.UserAddedToWaitingList(aliasUser)));
            }
        }

        [TestCase("opponent as:alias")]
        [TestCase("as:alias opponent")]
        public async Task ExecuteAsync_WithOpponentAndAlias_UsesAliasAsSender_AndStartsGame(string input)
        {
            // Arrange
            Facade.Instance.AddUserToWaitingList(opponentUser);

            this.CommandMock
                .Protected()
                .Setup<string>("GetDisplayName", opponentUser)
                .Returns(opponentUser);

            // Act
            await this.CommandMock.Object.ExecuteAsync(input);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                var aliasWaiting = Facade.Instance.UserIsWaiting(aliasUser);
                var opponentWaiting = Facade.Instance.UserIsWaiting(opponentUser);

                Assert.That(aliasWaiting.IsSuccess, Is.True);
                Assert.That(aliasWaiting.Value, Is.False);

                Assert.That(opponentWaiting.IsSuccess, Is.True);
                Assert.That(opponentWaiting.Value, Is.False);

                Assert.That(this.Reply, Is.Not.Null);
                Assert.That(
                    this.Reply,
                    Is.EqualTo(PlayCommandMessages.GameStarted(opponentUser)));
            }
        }
    }
}
