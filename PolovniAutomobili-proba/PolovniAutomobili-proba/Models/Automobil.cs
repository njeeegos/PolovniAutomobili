using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PolovniAutomobili_proba.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PolovniAutomobili_proba.Models
{
    public class Automobil
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string Marka { get; set; }
        public string Model { get; set; }
        public string Lokacija { get; set; }
        public int Cena { get; set; }
        public int Godiste { get; set; }

        [Display(Name="Dodatne informacije")]
        public List<string> Oznake { get; set; }
        public ApplicationUser Korisnik { get; set; }
    }
}
