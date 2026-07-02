using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.DTOs;

public class CreateTaskDto
{
    [Required(ErrorMessage = "Title is required.")]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required]
    public DateTime DueDate { get; set; }
}
