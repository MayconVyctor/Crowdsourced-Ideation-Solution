using Cis.Core.Domain.Exceptions;
using Cis.Presentation.Requests;
using Cis.Presentation.Validators;
using FluentAssertions;
using Xunit;

namespace Cis.UnitTests.Presentation;

public class TopicCreateRequestValidatorTest
{
    [Fact]
    public void Validate_WhenRequestIsNull_ShouldThrowCisValidationException()
    {

        TopicCreateRequest? request = null;

        Action act = () =>
            TopicCreateRequestValidator.Validate(request);

        act.Should()
            .Throw<CisValidationException>()
            .WithMessage("Request body cannot be null.");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_WhenTitleIsNullOrWhiteSpace_ShouldThrowCisValidationException(string? invalidTitle)
    {
        var request = new TopicCreateRequest(invalidTitle!, "Valid description");

        Action act = () =>
            TopicCreateRequestValidator.Validate(request);

        act.Should()
            .Throw<CisValidationException>()
            .WithMessage("Title is required and cannot be empty.");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_WhenDescriptionIsNullOrWhiteSpace_ShouldThrowCisValidationException(string? invalidDescription)
    {
        var request = new TopicCreateRequest("Valid Title", invalidDescription!);

        Action act = () =>
            TopicCreateRequestValidator.Validate(request);

        act.Should()
            .Throw<CisValidationException>()
            .WithMessage("Description is required and cannot be empty.");
    }

    [Fact]
    public void Validate_WhenRequestIsValid_ShouldNotThrow()
    {
        var request = new TopicCreateRequest("Valid Title", "Valid Description");

        Action act = () =>
            TopicCreateRequestValidator.Validate(request);


        act.Should()
            .NotThrow();
    }
}