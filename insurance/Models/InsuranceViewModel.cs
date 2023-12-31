using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace insurance.Models
{
    public class InsuranceViewModel : IValidatableObject 
    {
        public int Id { get; set; }
        public int Uid { get; set; }

        [Required(ErrorMessage = "Vyplňte druh pojištění")]
        [Display(Name = "Druh pojištění")]
        public string InsuranceType { get; set; } = "";

        [NotMapped]
        [ValidateNever]
        public List<SelectListItem> InsuranceList { get; set; }

        [Required(ErrorMessage = "Vyplňte velikost částky")]
        [Display(Name = "Částka")]
        public int Sum { get; set; }

        [Required(ErrorMessage = "Vyplňte předmět pojištění")]
        [Display(Name = "Předmět pojištění")]
        public string SubjectOfInsurance { get; set; } = "";

        [Required(ErrorMessage = "Vyplňte platnost pojištění od")]
        [Display(Name = "Platnost od")]
        [DataType(DataType.Date)]
        public DateTime ValidFrom { get; set; }

        [Required(ErrorMessage = "Vyplňte platnost pojištění do")]
        [Display(Name = "Platnost do")]
        [DataType(DataType.Date)]
        public DateTime ValidUntil {  get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(ValidFrom < DateTime.Today)
            {
                yield return new ValidationResult(
                    "Platnost od nemůže být menší než dnešní datum", new[] {nameof(ValidFrom)});
            }
            if(ValidUntil <= ValidFrom)
            {
                yield return new ValidationResult(
                    "Platnost do nemůže být menší nebo rovno datum od", new[] { nameof(ValidUntil) });
            }
        }
        public InsuranceViewModel()
        {
            InsuranceList = new List<SelectListItem>();
        }
    }
}
