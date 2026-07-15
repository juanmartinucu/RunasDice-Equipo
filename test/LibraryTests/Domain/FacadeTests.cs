using System;
using System.Collections.Generic;
using NUnit.Framework;
using Ucu.Poo.RunasDices.Domain;

namespace Ucu.Poo.RunasDices.Tests.Domain
{
    [TestFixture]
    public class FacadeTests
    {
        [SetUp]
        public void SetUp()
        {
            Facade.Reset();
        }

        #region Singleton

        [Test]
        public void Instance_ReturnsSameSingletonInstance()
        {
            // Arrange & act
            Facade f1 = Facade.Instance;
            Facade f2 = Facade.Instance;

            // Assert
            Assert.That(f1, Is.SameAs(f2));
        }

        [Test]
        public void Reset_AfterGettingInstance_CreatesNewInstance()
        {
            // Arrange
            Facade beforeReset = Facade.Instance;

            // Act
            Facade.Reset();
            Facade afterReset = Facade.Instance;

            // Assert
            Assert.That(object.ReferenceEquals(afterReset, beforeReset), Is.False);
        }

        #endregion

        #region GetUserInfo

        [Test]
        public void GetUserInfo_WithNullUserName_ThrowsArgumentNullException()
        {
            // Arrange
            Facade facade = Facade.Instance;
            Action act = () => facade.GetUserInfo(null);

            // Act & assert
            Assert.That(act, Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void GetUserInfo_WithEmptyUserName_ThrowsArgumentException()
        {
            // Arrange
            Facade facade = Facade.Instance;
            Action act = () => facade.GetUserInfo(string.Empty);

            // Act & assert
            Assert.That(act, Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void GetUserInfo_WithWhiteSpaceUserName_ThrowsArgumentException()
        {
            // Arrange
            Facade facade = Facade.Instance;
            Action act = () => facade.GetUserInfo("   ");

            // Act & assert
            Assert.That(act, Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void GetUserInfo_WithValidUserName_ReturnsNewUserMessage()
        {
            // Arrange
            Facade facade = Facade.Instance;

            // Act
            string actual = facade.GetUserInfo("Test");

            // Assert
            Assert.That(actual, Is.EqualTo(FacadeMessages.UserIsNew("Test")));
        }

        [Test]
        public void GetUserInfo_WhenUserAlreadyExistsAndNotWaiting_ReturnsCanPlayMessage()
        {
            // Arrange
            Facade facade = Facade.Instance;
            facade.GetUserInfo("Test");

            // Act
            string actual = facade.GetUserInfo("Test");

            // Assert
            Assert.That(actual, Is.EqualTo(FacadeMessages.UserCanPlay("Test")));
        }

        [Test]
        public void GetUserInfo_WhenUserIsWaiting_ReturnsWaitingMessage()
        {
            // Arrange
            Facade facade = Facade.Instance;
            facade.AddUserToWaitingList("Test");

            // Act
            string actual = facade.GetUserInfo("Test");

            // Assert
            Assert.That(actual, Is.EqualTo(FacadeMessages.UserIsWaiting("Test")));
        }

        #endregion

        #region AddUserToWaitingList

        [Test]
        public void AddUserToWaitingList_WithNullUserName_ThrowsArgumentNullException()
        {
            // Arrange
            Facade facade = Facade.Instance;
            Action act = () => facade.AddUserToWaitingList(null);

            // Act & assert
            Assert.That(act, Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void AddUserToWaitingList_WithEmptyUserName_ThrowsArgumentException()
        {
            // Arrange
            Facade facade = Facade.Instance;
            Action act = () => facade.AddUserToWaitingList(string.Empty);

            // Act & assert
            Assert.That(act, Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void AddUserToWaitingList_WithWhiteSpaceUserName_ThrowsArgumentException()
        {
            // Arrange
            Facade facade = Facade.Instance;
            Action act = () => facade.AddUserToWaitingList("   ");

            // Act & assert
            Assert.That(act, Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void AddUserToWaitingList_WhenUserAlreadyWaiting_ReturnsFailureWithMessage()
        {
            // Arrange
            Facade facade = Facade.Instance;
            facade.AddUserToWaitingList("Test");

            // Act
            Result actual = facade.AddUserToWaitingList("Test");

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(actual.IsFailure, Is.True);
                Assert.That(actual.Errors, Is.Not.Null);
                Assert.That(actual.Errors, Is.EqualTo(FacadeMessages.UserAlreadyWaiting("Test")));
            }
        }

        [Test]
        public void AddUserToWaitingList_WithValidUser_AddsUserToWaitingList()
        {
            // Arrange
            Facade facade = Facade.Instance;

            // Act
            Result actual = facade.AddUserToWaitingList("Test");

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(actual.IsSuccess, Is.True);
                Assert.That(Facade.Instance.UserIsWaiting("Test").Value, Is.True);
            }
        }

        #endregion

        #region  UserIsWaiting

        [Test]
        public void UserIsWaiting_WithNullUserName_ThrowsArgumentNullException()
        {
            // Arrange
            Facade facade = Facade.Instance;
            Action act = () => facade.UserIsWaiting(null);

            // Act & assert
            Assert.That(act, Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void UserIsWaiting_WithEmptyUserName_ThrowsArgumentException()
        {
            // Arrange
            Facade facade = Facade.Instance;
            Action act = () => facade.UserIsWaiting(string.Empty);

            // Act & assert
            Assert.That(act, Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void UserIsWaiting_WithWhiteSpaceUserName_ThrowsArgumentException()
        {
            // Arrange
            Facade facade = Facade.Instance;
            Action act = () => facade.UserIsWaiting("   ");

            // Act & assert
            Assert.That(act, Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void UserIsWaiting_WhenUserIsNotWaiting_ReturnsFalse()
        {
            // Arrange
            Facade facade = Facade.Instance;

            // Act
            Result<bool> actual = facade.UserIsWaiting("Test");

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(actual.IsSuccess, Is.True);
                Assert.That(actual.Value, Is.False);
            }
        }

        [Test]
        public void UserIsWaiting_IsCaseInsensitive_ReturnsTrueForDifferentCasing()
        {
            // Arrange
            Facade facade = Facade.Instance;
            facade.AddUserToWaitingList("Test");

            // Act
            Result<bool> actual = facade.UserIsWaiting("test");

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(actual.IsSuccess, Is.True);
                Assert.That(actual.Value, Is.True);
            }
        }

        #endregion

        #region StartGame

        [Test]
        public void StartGame_WithNullUserName_ThrowsArgumentNullException()
        {
            // Arrange
            Facade facade = Facade.Instance;
            Action act = () => facade.StartGame(null, "Test2");

            // Act & assert
            Assert.That(act, Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void StartGame_WithNullOpponentName_ThrowsArgumentNullException()
        {
            // Arrange
            Facade facade = Facade.Instance;
            Action act = () => facade.StartGame("Test", null);

            // Act & assert
            Assert.That(act, Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void StartGame_WithEmptyUserName_ThrowsArgumentException()
        {
            // Arrange
            Facade facade = Facade.Instance;
            Action act = () => facade.StartGame(string.Empty, "Pepe");

            // Act & assert
            Assert.That(act, Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void StartGame_WithEmptyOpponentName_ThrowsArgumentException()
        {
            // Arrange
            Facade facade = Facade.Instance;
            Action act = () => facade.StartGame("Test", string.Empty);

            // Act & assert
            Assert.That(act, Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void StartGame_WithWhiteSpaceUserName_ThrowsArgumentException()
        {
            // Arrange
            Facade facade = Facade.Instance;
            Action act = () => facade.StartGame("   ", "Pepe");

            // Act & assert
            Assert.That(act, Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void StartGame_WithWhiteSpaceOpponentName_ThrowsArgumentException()
        {
            // Arrange
            Facade facade = Facade.Instance;
            Action act = () => facade.StartGame("Test", "   ");

            // Act & assert
            Assert.That(act, Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void StartGame_WithOpponentNotWaiting_ReturnsFailureWithMessage()
        {
            // Arrange
            Facade facade = Facade.Instance;

            // Act
            Result<Game> actual = facade.StartGame("Test1", "Test2");

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(actual.IsFailure, Is.True);
                Assert.That(actual.Errors, Is.Not.Null);
                Assert.That(actual.Errors, Is.EquivalentTo(FacadeMessages.OpponentIsNotWaiting("Test2")));
            }
        }

        [Test]
        public void StartGame_WithValidUsers_ReturnsGameWithExpectedPlayers()
        {
            // Arrange
            Facade facade = Facade.Instance;
            facade.AddUserToWaitingList("Test");
            facade.AddUserToWaitingList("Pepe");

            // Act
            Result<Game> actual = facade.StartGame("Test", "Pepe");

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(actual.IsSuccess, Is.True);
                Assert.That(actual.Value, Is.Not.Null);
                Assert.That(actual.Value.Player1.User.UserName, Is.EqualTo("Test"));
                Assert.That(actual.Value.Player2.User.UserName, Is.EqualTo("Pepe"));
            }
        }

        [Test]
        public void StartGame_WhenUsersWereWaiting_RemovesBothFromWaitingList()
        {
            // Arrange
            Facade facade = Facade.Instance;
            facade.AddUserToWaitingList("Test1");
            facade.AddUserToWaitingList("Test2");

            // Act
            facade.StartGame("Test1", "Test2");

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Result<bool> actual = facade.UserIsWaiting("Test1");
                Assert.That(actual.IsSuccess, Is.True);
                Assert.That(actual.Value, Is.False);

                actual = facade.UserIsWaiting("Test2");
                Assert.That(actual.IsSuccess, Is.True);
                Assert.That(actual.Value, Is.False);
            }
        }

        #endregion

        #region GetUsersWaitingForOpponent

        [Test]
        public void GetUsersWaitingForOpponent_WhenUsersAreNotWaiting_ReturnsEmpty()
        {
            // Arrange
            Facade facade = Facade.Instance;

            // Act
            IReadOnlyList<string> actual = facade.GetUsersWaitingForOpponent();

            // Assert
            Assert.That(actual, Is.Empty);
        }

        [Test]
        public void GetUsersWaitingForOpponent_WhenUsersAreWaiting_ReturnsUsers()
        {
            // Arrange
            Facade facade = Facade.Instance;
            facade.AddUserToWaitingList("Test");

            // Act
            IReadOnlyList<string> actual = facade.GetUsersWaitingForOpponent();

            // Assert
            Assert.That(actual, Does.Contain("Test"));
        }

        #endregion
    }
}
