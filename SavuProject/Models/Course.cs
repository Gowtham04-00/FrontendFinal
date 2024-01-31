using System.ComponentModel.DataAnnotations;

namespace SavuProject.Models
{
    public class Course
    {
        public int CId { get; set; }

        [Required]
        public string CName { get; set; }

        [Required]
        public string CDuration { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public string CAvailability { get; set; }

        [Required]
        [MaxLength(10000)] // Set the desired maximum length
        public string CDescription { get; set; }

        [Required]
        [MaxLength(10000)] // Set the desired maximum length
        public string CPre { get; set; }

        [Required]
        [MaxLength(10000)] // Set the desired maximum length
        public string OutCome { get; set; }
    }
}
