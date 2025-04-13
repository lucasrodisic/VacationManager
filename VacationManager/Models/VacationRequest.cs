using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacationManager.Models
{
    public enum VacationType
    {
        Paid,
        Unpaid,
        Sick
    }

    public class VacationRequest
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Тип отпуск")]
        public VacationType Type { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "От дата")]
        public DateTime FromDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "До дата")]
        public DateTime ToDate { get; set; }

        [Display(Name = "Дата на заявка")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Половин ден")]
        public bool IsHalfDay { get; set; }

        [Display(Name = "Одобрена")]
        public bool IsApproved { get; set; } = false;

        
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }


        [Display(Name = "Болничен документ")]
        public string? SickNoteFilePath { get; set; }

        [NotMapped]
        public IFormFile? SickNoteUpload { get; set; }
    }
}
