using Cis.Core.Domain.Exceptions;
using Cis.Presentation.Validators;
using FluentAssertions;
using Xunit;

namespace Cis.UnitTests.Presentation;

public class TopicCloseValidatorTests
{
    [Fact]
    public void Validate_WhenUserIdHeaderIsValid_ShouldReturnParsedUserId()
    {
        // Arrange
        const string header = "42";

        // Act
        var result = TopicCloseValidator.Validate(header);

        // Assert
        result.Should().Be(42);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_WhenUserIdHeaderIsNullOrWhitespace_ShouldThrowCisValidationException(string? header)
    {
        // Act
        Action act = () => TopicCloseValidator.Validate(header);

        // Assert
        act.Should()
            .Throw<CisValidationException>()
            .WithMessage("X-User-Id header is required and must contain a valid user ID.");
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("12.34")]
    [InlineData("-5")]
    [InlineData("0")]
    public void Validate_WhenUserIdHeaderIsNotValidPositiveInteger_ShouldThrowCisValidationException(string header)
    {
        // Act
        Action act = () => TopicCloseValidator.Validate(header);

        // Assert
        act.Should()
            .Throw<CisValidationException>()
            .WithMessage("X-User-Id header is required and must contain a valid user ID.");
    }
}

