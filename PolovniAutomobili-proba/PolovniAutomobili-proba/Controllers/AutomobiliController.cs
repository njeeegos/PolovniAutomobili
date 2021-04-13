using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.Web.CodeGeneration.Utils;
using MongoDB.Bson;
using MongoDB.Driver;
using PolovniAutomobili_proba.Areas.Identity.Data;
using PolovniAutomobili_proba.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolovniAutomobili_proba.Controllers
{
    public class AutomobiliController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AutomobiliController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: AutomobiliController
        public ActionResult Index()
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var client = new MongoClient(connectionString);
            var db = client.GetDatabase("PolovniAutomobili");

            var collection = db.GetCollection<Automobil>("automobili");

            List<Automobil> automobili = new List<Automobil>();
            foreach (Automobil a in collection.Find(new BsonDocument()).ToList())
            {
                automobili.Add(a);
            }

            IndexAutomobiliViewModel model = new IndexAutomobiliViewModel
            {
                Automobili = automobili
            };

            return View(model);
        }

        // GET: AutomobiliController/Details/5
        public ActionResult Details(string id)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var client = new MongoClient(connectionString);
            var db = client.GetDatabase("PolovniAutomobili");

            var collection = db.GetCollection<Automobil>("automobili");

            ObjectId objId = ObjectId.Parse(id);

            var filter = Builders<Automobil>.Filter.Eq("_id", objId);
            var document = collection.Find(filter).First();

            return View(document);
        }

        // GET: AutomobiliController/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: AutomobiliController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Automobil auto)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var client = new MongoClient(connectionString);
            var db = client.GetDatabase("PolovniAutomobili");

            var collection = db.GetCollection<Automobil>("automobili");
            //var user = _userManager.GetUserAsync(User).Result;


            Automobil automobil = new Automobil
            {
                Marka = auto.Marka,
                Model = auto.Model,
                Lokacija = auto.Lokacija,
                Cena = auto.Cena,
                Godiste = auto.Godiste,
                Oznake = auto.Oznake,
                Korisnik = _userManager.GetUserAsync(User).Result
            };

            collection.InsertOne(automobil);

            return RedirectToAction("Index");
        }


        [Authorize]
        public ActionResult MojiOglasi()
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var client = new MongoClient(connectionString);
            var db = client.GetDatabase("PolovniAutomobili");

            var collection = db.GetCollection<Automobil>("automobili");
            //var usersCollection = db.GetCollection<ApplicationUser>("Users");

            var userId = _userManager.GetUserAsync(User).Result.Id;

            var filter = Builders<Automobil>.Filter.Eq("Korisnik._id", userId);

            List<Automobil> model = new List<Automobil>();
            foreach (Automobil a in collection.Find(filter).ToCursor().ToEnumerable())
            {
                model.Add(a);
            }

            return View(model);
        }


        // GET: AutomobiliController/Edit/5
        [Authorize]
        public ActionResult Edit(string id)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var client = new MongoClient(connectionString);
            var db = client.GetDatabase("PolovniAutomobili");

            var collection = db.GetCollection<Automobil>("automobili");

            ObjectId objId = ObjectId.Parse(id);

            var filter = Builders<Automobil>.Filter.Eq("_id", objId);
            var document = collection.Find(filter).First();


            EditAutomobilViewModel viewModel = new EditAutomobilViewModel
            {
                Automobil = document,
                AutoId = document.Id.ToString(),
                UserId = document.Korisnik.Id.ToString()
            };
            return View(viewModel);
        }

        // POST: AutomobiliController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditAutomobilViewModel viewModel)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var client = new MongoClient(connectionString);
            var db = client.GetDatabase("PolovniAutomobili");

            var collection = db.GetCollection<Automobil>("automobili");

            ObjectId autoId = ObjectId.Parse(viewModel.AutoId);
            ObjectId userId = ObjectId.Parse(viewModel.UserId);

            //var filterBuilder = Builders<Automobil>.Filter;
            //var filter = filterBuilder.Eq("Marka", auto.Marka) & filterBuilder.Eq("Model", auto.Model) & filterBuilder.Eq("Lokacija", auto.Lokacija) 
            //                                                   & filterBuilder.Eq("Cena", auto.Cena) & filterBuilder.Eq("Godiste", auto.Godiste) 
            //                                                   & filterBuilder.Eq("Oznake", auto.Oznake) & filterBuilder.Eq("Korisnik._id", userId);

            var filter = Builders<Automobil>.Filter.Eq("_id", autoId);
            var update = Builders<Automobil>.Update.Set("Marka", viewModel.Automobil.Marka)
                                                   .Set("Model", viewModel.Automobil.Model)
                                                   .Set("Lokacija", viewModel.Automobil.Lokacija)
                                                   .Set("Cena", viewModel.Automobil.Cena)
                                                   .Set("Godiste", viewModel.Automobil.Godiste)
                                                   .Set("Oznake", viewModel.Automobil.Oznake);
            collection.UpdateOne(filter, update);

            return RedirectToAction("MojiOglasi");
        }

        // GET: AutomobiliController/Delete/5
        [Authorize]
        public ActionResult Delete(string id)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var client = new MongoClient(connectionString);
            var db = client.GetDatabase("PolovniAutomobili");

            var collection = db.GetCollection<Automobil>("automobili");

            var objectId = ObjectId.Parse(id);

            var filter1 = Builders<Automobil>.Filter.Eq("_id", objectId);
            collection.DeleteOne(filter1);

            return RedirectToAction("MojiOglasi");

        }

        // POST: AutomobiliController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Pretrazi(IndexAutomobiliViewModel model)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var client = new MongoClient(connectionString);
            var db = client.GetDatabase("PolovniAutomobili");

            var collection = db.GetCollection<Automobil>("automobili");

            var filterBuilder = Builders<Automobil>.Filter;
            var filter = filterBuilder.Empty;
            if (model.Automobil.Marka != null)
                filter = filter & filterBuilder.Eq("Marka", model.Automobil.Marka);
            if (model.Automobil.Model != null)
                filter = filter & filterBuilder.Eq("Model", model.Automobil.Model);
            if (model.Automobil.Lokacija != null)
                filter = filter & filterBuilder.Eq("Lokacija", model.Automobil.Lokacija);
            if (model.Automobil.Cena != 0)
                filter = filter & filterBuilder.Lt("Cena", model.Automobil.Cena);
            if (model.Automobil.Godiste != 0)
                filter = filter & filterBuilder.Gt("Godiste", model.Automobil.Godiste);
            if (model.Automobil.Oznake[0] != null)
                filter = filter & filterBuilder.In("Oznake", model.Automobil.Oznake);
            if (model.Automobil.Oznake[1] != null)
                filter = filter & filterBuilder.In("Oznake", model.Automobil.Oznake);

            List<Automobil> kola = new List<Automobil>();
            foreach (Automobil a in collection.Find(filter).ToCursor().ToEnumerable())
            {
                kola.Add(a);
            }

            //model.Automobili = kola;
            //model.Automobil = null;
            IndexAutomobiliViewModel noviModel = new IndexAutomobiliViewModel
            {
                Automobili = kola
            };

            return View("Index", noviModel);
        }
    }
}
