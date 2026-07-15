using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using Ucu.Poo.RunasDices.Commands;
using Ucu.Poo.RunasDices.Domain;

namespace Ucu.Poo.RunasDices.Tests.Commands
{
    [TestFixture]
    public class WaitListCommandTests : CommandTestBase<WaitListCommand>
    {
        [SetUp]
        public void SetUp()
        {
            ResetFacadeCreateRepliesAndMock();
        }

        [Test]
        public async Task ExecuteAsync_WhenNoUsersAreWaiting_RepliesWithNoPlayersMessage()
        {
            // Arrange: no se agrega ningún usuario a la waitlist
            Facade facade = Facade.Instance;

            // Act
            await CommandMock.Object.ExecuteAsync();

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(Reply, Is.Not.Null);
                Assert.That(
                    Reply,
                    Is.EqualTo(
                        WaitListCommandMessages.NoPlayersWaitingToPlay));
            }
        }

        [Test]
        public async Task ExecuteAsync_WhenOneUserIsWaiting_ListsThatUser()
        {
            // Arrange
            Facade facade = Facade.Instance;
            facade.AddUserToWaitingList("Test");

            // Act
            await CommandMock.Object.ExecuteAsync();

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(Reply, Is.Not.Null);
                Assert.That(
                    Reply,
                    Is.EqualTo(
                        WaitListCommandMessages.PlayersWaiting("'Test'")));
            }
        }

        [Test]
        public async Task ExecuteAsync_WhenMultipleUsersAreWaiting_ListsAllUsersCommaSeparated()
        {
            // Arrange
            Facade facade = Facade.Instance;
            facade.AddUserToWaitingList("Test1");
            facade.AddUserToWaitingList("Test2");
            facade.AddUserToWaitingList("Test3");

            // Act
            await CommandMock.Object.ExecuteAsync();

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(Reply, Is.Not.Null);
                // El orden depende de cómo Facade devuelva la lista; asumimos que respeta el orden de inserción.
                Assert.That(
                    Reply,
                    Is.EqualTo(
                        WaitListCommandMessages.PlayersWaiting("'Test1', 'Test2', 'Test3'")));
            }
        }
    }
}
