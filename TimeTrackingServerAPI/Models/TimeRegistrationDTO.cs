using System;
using System.ComponentModel.DataAnnotations;

namespace TimeTrackingServerAPI.Models
{
	public class TimeRegistrationDTO
	{
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Dato")]
        public DateTime Date { get; set; } = DateTime.Now   ;

        [Required]
        [Range(0, 24)]
        [Display(Name = "Antal Timer")]
        public double HoursWorked { get; set; }
    }
}

