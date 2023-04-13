using System.ComponentModel.DataAnnotations;

namespace Work.ApiModels;

public class UserModelDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }

    public DateTime Birthday { get; set; }
}