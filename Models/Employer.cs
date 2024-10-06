using System;
using System.ComponentModel.DataAnnotations;

namespace lab1.Models
{
    public class Employer
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Employer Name")]
        public string Name { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [Url]
        public string Website { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Incorporated Date")]
        public DateTime? IncorporatedDate { get; set; }
    }

}
