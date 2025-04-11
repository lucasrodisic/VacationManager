


using System.ComponentModel.DataAnnotations;

namespace VacationManager.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Името на проекта е задължително.")]
        [StringLength(100, ErrorMessage = "Името трябва да е до 100 символа.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Описанието е задължително.")]
        [StringLength(1000, ErrorMessage = "Описанието трябва да е до 1000 символа.")]
        public string Description { get; set; } = null!;

        public ICollection<Team> Teams { get; set; } = new List<Team>();
    }
}
