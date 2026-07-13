// Filename: UsersRepositoryTests.cs

using NUnit.Framework;
using Ucu.Poo.RunasDices.Domain;

namespace Ucu.Poo.RunasDices.Tests
{
    [TestFixture]
    public class UsersRepositoryTests
    {
        [Test]
        public void Find_WhenUserExists_ReturnsUser()
        {
            UsersRepository repository = new UsersRepository();
            User user = repository.Add("test-user");

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
    }
}
