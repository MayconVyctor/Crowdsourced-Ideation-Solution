using System.ComponentModel.DataAnnotations;

namespace Cis.Presentation.Requests;

public record TopicCreateRequest(
    [Required(ErrorMessage = "Title is required.")] string? Title,
    [Required(ErrorMessage = "Description is required.")] string? Description);
