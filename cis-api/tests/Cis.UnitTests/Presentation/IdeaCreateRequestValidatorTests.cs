using Cis.Core.Domain.Exceptions;
using Cis.Presentation.Requests;
using Cis.Presentation.Validators;
using FluentAssertions;
using Xunit;

namespace Cis.UnitTests.Presentation;

public class IdeaCreateRequestValidatorTests
{
    [Fact]
    public void Validate_WhenRequestIsNull_ShouldThrowCisValidationException()
    {
        IdeaCreateRequest? request = null;

        Action act = () =>
            IdeaCreateRequestValidator.Validate(request);

        act.Should()
            .Throw<CisValidationException>()
            .WithMessage("Request body cannot be null.");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_WhenTitleIsNullOrWhiteSpace_ShouldThrowCisValidationException(
        string? invalidTitle)
    {
        var request = new IdeaCreateRequest(
            invalidTitle!,
            "Valid description");

        Action act = () =>
            IdeaCreateRequestValidator.Validate(request);

        act.Should()
            .Throw<CisValidationException>()
            .WithMessage("Title is required and cannot be empty.");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_WhenDescriptionIsNullOrWhiteSpace_ShouldThrowCisValidationException(
        string? invalidDescription)
    {
        var request = new IdeaCreateRequest(
            "Valid title",
            invalidDescription!);

        Action act = () =>
            IdeaCreateRequestValidator.Validate(request);

        act.Should()
            .Throw<CisValidationException>()
            .WithMessage("Description is required and cannot be empty.");
    }

    [Fact]
    public void Validate_WhenRequestIsValid_ShouldNotThrow()
    {
        var request = new IdeaCreateRequest(
            "Valid title",
            "Valid description");

        Action act = () =>
            IdeaCreateRequestValidator.Validate(request);

        act.Should().NotThrow();
    }
}
