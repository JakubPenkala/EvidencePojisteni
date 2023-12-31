using System.ComponentModel.DataAnnotations;

namespace insurance.Models
{
    public class InsuranceEventsViewModel
    {
        public int Id { get; set; }
        public int Iid { get; set; }
        [Required(ErrorMessage = "Vyplňte událost")]
        [Display(Name = "Událost")]
        public string Event { get; set; } = "";
        [Required(ErrorMessage = "Vyplňte datum a čas události")]
        [Display(Name = "Datum a čas události")]
        public DateTime EventDate { get; set; }
    }
}
