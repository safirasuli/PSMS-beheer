using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PSMSData.Model;
using PSMS_Beheer.Models;
using System.Globalization;

namespace PSMS_Beheer.Controllers
{
    public class parametermetsubtypeController : Controller
    {
        private PSMSEntities db = new PSMSEntities();

        // GET: parametermetsubtype
        public ActionResult Index()
        {
            var parametermetsubtype = db.parametermetsubtype;
            var query = from p in db.parametermetsubtype
                        group p by new { p.parameternaam, p.domeinID, p.datatypecode } into g
                        select new PMSIndexViewModel { parameternaam = g.Key.parameternaam, domeinID = g.Key.domeinID, datatypecode = g.Key.datatypecode };


            return View(query.ToList());
        }

        // GET: parametermetsubtype/Create
        public ActionResult Create()
        {
            ViewBag.datatypecode = new SelectList(db.datatype, "datatypecode", "datatypecode");
            ViewBag.domeinID = new SelectList(db.domein, "domeinID", "domeinnaam");
            return View();
        }

        public ActionResult SubtypeSelecteren(String id)
        {
            var model = new ParameterMetSubtypeModel();
            var parameternaam = id;

            model.Datatype = db.parametermetsubtype.Where(n => n.parameternaam.Equals(parameternaam)).Select(m => m.datatypecode).FirstOrDefault();
            var domeinID = db.parametermetsubtype.Select(m => m.domeinID).FirstOrDefault();

            if (model.Datatype == null)
            {
                model.Domein = db.domein.Where(m => m.domeinID == domeinID).Select(m => m.domeinnaam).First();
            }
            model.ParameterNaam = parameternaam;
            model.Parameter = db.parametermetsubtype.Where(d => d.parameternaam.Equals(parameternaam)).ToList();

            return View(model);
        }


        public ActionResult Subtype()
        {
            ViewBag.errormessage = TempData["UserMessage"];

            ViewBag.datatypecode = new SelectList(db.datatype, "datatypecode", "datatypecode");
            ViewBag.domeinID = new SelectList(db.domein, "domeinID", "domeinnaam");

            // vul domeinlijst
            var query = db.domein.Select(m => m.domeinnaam).Distinct();
            ViewBag.uniqueDomein = new SelectList(query, null);

            // vul datatypen.
            query = db.datatype.Select(n => n.datatypecode);
            ViewBag.datatype = new SelectList(query);


            return View();
        }


        // POST: parametermetsubtype/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "parameterid,subtypenaam,waarde,beschrijving_kort,beschrijving_lang,geldigheidvan,domeinID,datatypecode,parameternaam,isgoedgekeurd,geldigheidtot")] parametermetsubtype parametermetsubtype)
        {
            if (ModelState.IsValid)
            {
                parametermetsubtype.parameterid = Guid.NewGuid();
                db.parametermetsubtype.Add(parametermetsubtype);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.datatypecode = new SelectList(db.datatype, "datatypecode", "netdatatype", parametermetsubtype.datatypecode);
            ViewBag.domeinID = new SelectList(db.domein, "domeinID", "domeinnaam", parametermetsubtype.domeinID);
            return View(parametermetsubtype);
        }

        // GET: parametermetsubtype/Edit/5/
        public ActionResult Edit(Guid id,DateTime geldigheidvan)
        {
            
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            parametermetsubtype parametermetsubtype = db.parametermetsubtype.Find(id, geldigheidvan);
            if (parametermetsubtype == null)
            {
                return HttpNotFound();
            }
            ViewBag.datatypecode = new SelectList(db.datatype, "datatypecode", "netdatatype", parametermetsubtype.datatypecode);
            ViewBag.domeinID = new SelectList(db.domein, "domeinID", "domeinnaam", parametermetsubtype.domeinID);
            return View(parametermetsubtype);
        }
        [HttpPost]
        public ActionResult Edit(FormCollection form)
        {

            string Geldigheidvan = Request.Form["geldigheidtot"];

            string dateTimeString = Geldigheidvan.Split(new[] { '(', ')' },
                                             StringSplitOptions.RemoveEmptyEntries)
                                      .Last();

            DateTime convertedGeldigheidvan = DateTime.ParseExact(dateTimeString,
                                               "yyyy,M,d", CultureInfo.InvariantCulture);

            Request.Form["geldigheidtot"] = convertedGeldigheidvan.ToString("yyyy-mm-dd");

            PSMSEntities db = new PSMSEntities();
            //db.ParamWijzigen(Request.Form[""])
            
            
           

            return View();
        }


        [HttpPost]
        public ActionResult CreatedSubtype(FormCollection form)
        {
            
            // Informatie over de parameter zelf.
            ViewBag.parameternaam = Request.Form["parameternaam"];


            if (Request.Form["gekozenDD"] == "Datatype")
            {
                ViewBag.domeinOfDatatype = Request.Form["datatypeselectie"];
            }
            else
            {
                ViewBag.domeinOfDatatype = Request.Form["domeinselectie"];
            }
            ViewBag.gekozenDD = Request.Form["gekozenDD"];
            
            // vul domeinwaarden
            var query = db.domein.Where(m => m.domeinnaam.Equals("Automerken")).Select(n => n.domeincode);
            ViewBag.domeinwaarde = new SelectList(query);

            var succesmessage = "test 1" + Request.Form["gekozenDD"];

            //Als er een subtype verstuurd is. 
            if (Request.Form["subtypeOpslaan"] != null)
            {
                succesmessage = "test2" + Request.Form["gekozenDD"];
                ViewBag.subtypenaam = Request.Form["subtypenaam"];
                ViewBag.geldigheidvan = Request.Form["geldigheidvan"];
                ViewBag.waarde = Request.Form["waarde"];
                ViewBag.geldigheidtot = Request.Form["geldigheidtot"];
                ViewBag.beschrijving_kort = Request.Form["beschrijving_kort"];
                ViewBag.beschrijving_lang = Request.Form["beschrijving_lang"];
                if (Request.Form["gekozenDD"] == "Datatype")
                {
                    try
                    {
                        db.ParamToevoegen(Request.Form["parameternaam"], Request.Form["waarde"], Request.Form["datatypeselectie"], null, null, Request.Form["beschrijving_kort"], Request.Form["beschrijving_lang"],
                        Request.Form["subtypenaam"], Convert.ToDateTime(Request.Form["geldigheidvan"]), Convert.ToDateTime(Request.Form["geldigheidtot"]));
                        succesmessage = "Parameter met subtype succesvol toegevoegd.2";
                    }
                    catch (Exception)
                    {
                        succesmessage = "Er is een fout opgetreden. Controleer de data en probeer het opnieuw.";

                    }

                }
                else if (Request.Form["gekozenDD"] == "Domein")
                {
                    try
                    {
                        db.ParamToevoegen(Request.Form["parameternaam"], Request.Form["waarde"], null, Request.Form["domeinselectie"], Request.Form["waarde"], Request.Form["beschrijving_kort"], Request.Form["beschrijving_lang"],
                        Request.Form["subtypenaam"], Convert.ToDateTime(Request.Form["geldigheidvan"]), Convert.ToDateTime(Request.Form["geldigheidtot"]));
                        succesmessage = "Parameter met subtype succesvol toegevoegd.3";
                    }
                    catch (Exception)
                    {
                        succesmessage = "Fout.";
                    }
                }
            }
            
            else if (Request.Form["CreatedSubtype"] != null)
            {
                //db.parametermetsubtype.Select(p => p.parameternaam).Where();
                var formwaarde = Request.Form["parameternaam"].ToString();
                var parameternaamtest = db.parametermetsubtype.Where(p => p.parameternaam == formwaarde).Select(m => m.parameternaam);
         
                if (parameternaamtest.Contains(formwaarde))
                {
                    TempData["UserMessage"] = "Parameternaam bestaat al";
                    return RedirectToAction("Subtype");
                    
                }

            }

            ViewBag.succesmessage = succesmessage;
            return View();
        }

        [HttpPost]
        public ActionResult SubtypeControle(FormCollection form)
        {
            ViewBag.subtypenaam = Request.Form["subtypenaam"];
            ViewBag.geldigheidvan = Request.Form["geldigheidvan"];
            ViewBag.waarde = Request.Form["waarde"];
            ViewBag.geldigheidtot = Request.Form["geldigheidtot"];
            ViewBag.beschrijving_kort = Request.Form["beschrijving_kort"];
            ViewBag.beschrijving_lang = Request.Form["beschrijving_lang"];

            return View("subtype");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
