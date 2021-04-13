using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PolovniAutomobili_proba.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Korisničko ime")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Loznika")]
        public string Password { get; set; }

        [Display(Name = "Zapamti me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Korisničko ime")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Loznika")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potvrdi lozinku")]
        [Compare("Password", ErrorMessage = "Unesene lozinke se ne poklapaju.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Ime")]
        public string Ime { get; set; }

        [Required]
        [Display(Name = "Prezime")]
        public string Prezime { get; set; }

        [Required]
        [Display(Name = "Kontakt telefon")]
        public string KontaktTelefon { get; set; }
    }
}
