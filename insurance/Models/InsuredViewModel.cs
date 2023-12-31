using System.ComponentModel.DataAnnotations;

namespace insurance.Models
{
    public class InsuredViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Vyplňte jméno")]
        [Display(Name = "Jmeno")]
        public string Name { get; set; } = "";
        [Required(ErrorMessage = "Vyplňte příjmení")]
        [Display(Name = "Příjmení")]
        public string Surname { get; set; } = "";
        [Required(ErrorMessage = "Vyplňte emailovou adresu")]
        [EmailAddress(ErrorMessage = "Neplatná emailová adresa")]
        [Display(Name = "Email")]
        public string Email { get; set; } = "";
        [Required(ErrorMessage = "Vyplňte telefoní číslo")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Telefoní číslo")]
        [RegularExpression("^(\\+420)? ?[1-9][0-9]{2} ?[0-9]{3} ?[0-9]{3}$", ErrorMessage = "Neplatné telefoní číslo")]

        public string PhoneNumber { get; set; } = "";
        [Required(ErrorMessage = "Vyplňte ulici")]
        [Display(Name = "Ulice")]
        public string Street { get; set; } = "";
        [Required(ErrorMessage = "Vyplňte město")]
        [Display(Name = "Město")]
        public string City { get; set; } = "";
        [Required(ErrorMessage = "Vyplňte PSČ")]
        [Display(Name = "PSČ")]
        [Range(10000,99999, ErrorMessage = "Vyplňte PSČ")]
        public int ZipCode { get; set; }
    }
}
