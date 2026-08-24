using System.ComponentModel.DataAnnotations;

namespace FlashOffer.API.Application.DTOs.Requests;

public class CreateCommentDto
{
    [Required(ErrorMessage = "Content is required")]
    [MaxLength(1000, ErrorMessage = "Content cannot exceed 1000 characters")]
    public string Content { get; set; } = string.Empty;

    public Guid? ParentCommentId { get; set; }
}