using System.ComponentModel.DataAnnotations;

namespace Work.ApiModels
{
    public class UserModelDto
    {
        [Required]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime Birthday { get; set; }
    }
}
