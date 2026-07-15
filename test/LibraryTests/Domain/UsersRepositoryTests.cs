using System.Collections.Generic;
using NUnit.Framework;
using Ucu.Poo.RunasDices.Domain;

namespace Ucu.Poo.RunasDices.Tests.Domain
{
    [TestFixture]
    public class UsersRepositoryTests
    {
        [Test]
        public void Add_WithValidUserName_AddsUserAndReturnsIt()
        {
            // Arrange
            UsersRepository repo = new UsersRepository();
            const string userName = "user";

            // Act
            User added = repo.Add(userName);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(added, Is.Not.Null);
                Assert.That(added.UserName, Is.EqualTo(userName));

                User found = repo.Find(userName);
                Assert.That(found, Is.SameAs(added));
            }
        }

        [Test]
        public void Find_WhenUserExists_ReturnsUser()
        {
            UsersRepository repository = new UsersRepository();
            User user = repository.Add("user");

            User actual = repository.Find(user.UserName);

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Is.EqualTo(user));
        }

        [Test]
        public void Find_WhenUserDoesNotExist_ReturnsNull()
        {
            UsersRepository repo = new UsersRepository();

            User actual = repo.Find("no-existe");

            Assert.That(actual, Is.Null);
        }

        [Test]
        public void Find_WithNullUserName_ReturnsNull()
        {
            UsersRepository repo = new UsersRepository();

            User actual = repo.Find(null);

            Assert.That(actual, Is.Null);
        }

        [Test]
        public void Find_WithEmptyUserName_ReturnsNull()
        {
            UsersRepository repo = new UsersRepository();

            User actual = repo.Find(string.Empty);

            Assert.That(actual, Is.Null);
        }

        [Test]
        public void AllUsers_WhenUsersAreAdded_ReturnsReadonlyCollectionWithAllUsers()
        {
            // Arrange
            UsersRepository repo = new UsersRepository();
            User u1 = repo.Add("user");
            User u2 = repo.Add("anotherUser");

            // Act
            IReadOnlyCollection<User> all = repo.AllUsers;

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(all, Is.Not.Null);
                Assert.That(all, Has.Count.EqualTo(2));
                Assert.That(all, Does.Contain(u1));
                Assert.That(all, Does.Contain(u2));
            }
        }
    }
}
