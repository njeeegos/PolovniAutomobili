using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Identity.Mongo.Model;
using Microsoft.AspNetCore.Identity;

namespace PolovniAutomobili_proba.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : MongoUser
    {
        public string Ime { get; set; }
        public string Prezime { get; set; }

        [Display(Name="Kontakt telefon")]
        public string KontaktTelefon { get; set; }
    }
}
