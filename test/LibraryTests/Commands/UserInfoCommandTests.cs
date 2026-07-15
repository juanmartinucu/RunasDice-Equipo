using System.Threading.Tasks;
using Moq.Protected;
using NUnit.Framework;
using Ucu.Poo.RunasDices.Commands;
using Ucu.Poo.RunasDices.Domain;

namespace Ucu.Poo.RunasDices.Tests.Commands
{
    [TestFixture]
    public class UserInfoCommandTests : CommandTestBase<UserInfoCommand>
    {
        [SetUp]
        public void SetUp()
        {
            ResetFacadeCreateRepliesAndMock();
        }

        [Test]
        public async Task ExecuteAsync_WithoutParameters_UsesSendingUserAndRepliesWithUserInfo()
        {
            // Arrange: nada

            // Act
            await CommandMock.Object.ExecuteAsync();

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(Reply, Is.Not.Null);
                Assert.That(
                    Reply,
                    Is.EqualTo(FacadeMessages.UserIsNew(SendingUser)));
            }
        }

        [Test]
        public async Task ExecuteAsync_WithExistingDisplayName_UsesThatUserAndRepliesWithUserInfo()
        {
            // Arrange
            const string otherUser = "other";

            // Configura el mock para simular que el usuario 'other' existe en
            // el contexto de Discord
            CommandMock
                .Protected()
                .Setup<bool>("UserExists", otherUser)
                .Returns(true);

            // Act
            await CommandMock.Object.ExecuteAsync(otherUser);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(Reply, Is.Not.Null);
                Assert.That(
                    Reply,
                    Is.EqualTo(FacadeMessages.UserIsNew(otherUser)));
            }
        }

        [Test]
        public async Task ExecuteAsync_WithNonExistingDisplayName_RepliesUserNotFound()
        {
            // Arrange
            const string unknownUser = "unknown";

            // Configura el mock para simular que el usuario NO existe
            CommandMock
                .Protected()
                .Setup<bool>("UserExists", unknownUser)
                .Returns(false);

            // Act
            await CommandMock.Object.ExecuteAsync(unknownUser);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(Reply, Is.Not.Null);
                Assert.That(
                    Reply,
                    Is.EqualTo(
                        UserInfoCommandMessages.UserNotFound(unknownUser)));
            }
        }
    }
}
