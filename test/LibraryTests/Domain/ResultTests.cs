using System;
using System.Collections.Generic;
using NUnit.Framework;
using Ucu.Poo.RunasDices.Domain;

namespace Ucu.Poo.RunasDices.Tests.Domain
{
    [TestFixture]
    public class ResultTests
    {
        [Test]
        public void Success_IsSuccessful_And_HasNoErrors()
        {
            // Act
            Result actual = Result.Success();

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(actual.IsSuccess, Is.True);
                Assert.That(actual.IsFailure, Is.False);
                Assert.That(actual.Errors, Is.Null);
            }
        }

        [Test]
        public void Failure_WithError_IsFailure_And_HasThatError()
        {
            // Arrange
            const string errorMessage = "Some error";

            // Act
            Result actual = Result.Failure(errorMessage);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(actual.IsSuccess, Is.False);
                Assert.That(actual.IsFailure, Is.True);
                Assert.That(actual.Errors, Is.Not.Null);
                Assert.That(actual.Errors, Does.Contain(errorMessage));
            }
        }
    }

    [TestFixture]
    public class ResultGenericTests
    {
        [Test]
        public void Success_WithValue_ShouldBeSuccess_AndExposeValue()
        {
            // Arrange
            int expected = 42;

            // Act
            Result<int> actual = Result.Success<int>(expected);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(actual.IsSuccess, Is.True);
                Assert.That(actual.IsFailure, Is.False);
                Assert.That(actual.Errors, Is.Null);
                Assert.That(actual.Value, Is.EqualTo(expected));
            }
        }

        [Test]
        public void Failure_WithError_ShouldBeFailure_AndHaveDefaultValue()
        {
            // Arrange
            string errors = "Some error";

            // Act
            Result<int> actual = Result.Failure<int>(errors);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(actual.IsSuccess, Is.False);
                Assert.That(actual.IsFailure, Is.True);
                Assert.That(actual.Errors, Is.Not.Null);
                Assert.That(actual.Errors, Is.EquivalentTo(errors));
                Assert.That(actual.Value, Is.Default);
            }
        }
    }
}
