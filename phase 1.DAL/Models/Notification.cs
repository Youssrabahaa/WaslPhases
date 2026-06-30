using System.ComponentModel.DataAnnotations;

namespace phase_1.DAL.Models
{

    public class Notification
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }

    [Required, MaxLength(120)]
    public string Title { get; set; } = default!;

    [Required, MaxLength(600)]
    public string Message { get; set; } = default!;

    [MaxLength(80)]
    public string Type { get; set; } = "Info";

    public bool IsRead { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
}
