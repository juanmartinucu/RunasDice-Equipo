using System;
using NUnit.Framework;
using Ucu.Poo.RunasDices.Domain;

namespace Ucu.Poo.RunasDices.Tests.Domain
{
    [TestFixture]
    public class UserTests
    {
        [Test]
        public void Constructor_WithValidUserName_SetsProperty()
        {
            User user = new User("Test");
            Assert.That(user.UserName, Is.EqualTo("Test"));
        }

        [Test]
        public void Constructor_WithNullUserName_ThrowsArgumentNullException()
        {
            Action act = () => new User(null);

            Assert.That(act, Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Constructor_WithEmptyUserName_ThrowsArgumentNullException()
        {
            Action act = () => new User(string.Empty);

            Assert.That(act, Throws.TypeOf<ArgumentException>());
        }
    }
}
