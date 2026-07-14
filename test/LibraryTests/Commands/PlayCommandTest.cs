using System.Threading.Tasks;
using Moq.Protected;
using NUnit.Framework;
using Ucu.Poo.RunasDices.Commands;
using Ucu.Poo.RunasDices.Domain;

namespace Ucu.Poo.RunasDices.Tests.Commands
{
    [TestFixture]
    public class PlayCommandTests : CommandTestBase<PlayCommand>
    {
        private const string opponentUser = "opponent";

        [SetUp]
        public void SetUp()
        {
            base.ResetFacadeCreateRepliesAndMock();
        }

        [Test]
        public async Task ExecuteAsync_WithoutParameters_AddsUserToWaitingList_AndSendsConfirmation()
        {
            // Arrange: nada

            // Act
            await CommandMock.Object.ExecuteAsync();

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(Facade.Instance.UserIsWaiting(SendingUser).Value, Is.True);
                Assert.That(Reply, Is.Not.Null);
                Assert.That(
                    Reply,
                    Is.EqualTo(PlayCommandMessages.UserAddedToWaitingList(SendingUser)));
            }
        }

        [Test]
        public async Task ExecuteAsync_WithMoreThanOneParameter_SendsUsageMessage()
        {
            // Arrange: nada

            // Act
            await CommandMock.Object.ExecuteAsync("uno dos");

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(Reply, Is.Not.Null);
                Assert.That(Reply, Is.EqualTo(PlayCommandMessages.CommandHelp));
            }
        }

        [Test]
        public async Task ExecuteAsync_WithOpponentParameter_StartsGame_AndSendsJoinMessage()
        {
            // Arrange
            Facade.Instance.AddUserToWaitingList(opponentUser);

            // Configura el comando para que al buscar opponentUser en Discord lo
            // encuentre sin usar Discord
            CommandMock
                .Protected()
                .Setup<string>("GetDisplayName", opponentUser)
                .Returns(opponentUser);

            // Act
            await CommandMock.Object.ExecuteAsync(opponentUser);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(Facade.Instance.UserIsWaiting(SendingUser).Value, Is.False);
                Assert.That(Facade.Instance.UserIsWaiting(opponentUser).Value, Is.False);
                Assert.That(Reply, Is.Not.Null);
                Assert.That(
                    Reply,
                    Is.EqualTo(PlayCommandMessages.GameStarted(opponentUser)));
            }
        }

        [Test]
        public async Task ExecuteAsync_WithOpponentParameter_WhenOpponentIsNotWaiting_SendsFacadeErrorMessage()
        {
            // Arrange
            CommandMock
                .Protected()
                .Setup<string>("GetDisplayName", opponentUser)
                .Returns(opponentUser);

            // Act
            await CommandMock.Object.ExecuteAsync(opponentUser);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(Facade.Instance.UserIsWaiting(SendingUser).Value, Is.False);
                Assert.That(Facade.Instance.UserIsWaiting(opponentUser).Value, Is.False);
                Assert.That(Reply, Is.Not.Null);
                Assert.That(
                    Reply,
                    Is.EqualTo(FacadeMessages.OpponentIsNotWaiting(opponentUser)));
            }
        }

        [Test]
        public async Task ExecuteAsync_WithOpponentParameter_WhenGetDisplayNameThrowsArgumentException_SendsExceptionMessage()
        {
            // Arrange
            
            // Configura el mock para que GetDisplayName dispare la excepción
            // ArgumentException con el mensaje a continuación.
            const string errorMessage = "Nombre de usuario inválido";

            CommandMock
                .Protected()
                .Setup<string>("GetDisplayName", opponentUser)
                .Throws(new System.ArgumentException(errorMessage));

            // Act
            await CommandMock.Object.ExecuteAsync(opponentUser);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(Facade.Instance.UserIsWaiting(SendingUser).Value, Is.False);
                Assert.That(Facade.Instance.UserIsWaiting(opponentUser).Value, Is.False);
                Assert.That(Reply, Is.Not.Null);
                Assert.That(Reply, Is.EqualTo(errorMessage));
            }
        }
    }
}
