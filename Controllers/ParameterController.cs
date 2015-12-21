using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using PSMSData.Model;
using PSMS_Beheer.Models;

namespace PSMS_Beheer.Controllers
{
    public class ParameterController : Controller
    {
        // GET: Parameter
        public ActionResult Index()
        {
            return View();
        }

        // GET: Parameter/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Parameter/Create
        public ActionResult ParameterZonderSubtypeToevoegen(string warning)
        {
            PSMSEntities db = new PSMSEntities();
            var paraZonderVm = new ParameterZonderSubtypeToevoegenViewModel();
            //paraZonderVm.Afdeling = new List<SelectListItem>();  
            //paraZonderVm.Afdeling = getAfdeling();
            paraZonderVm.DomeinnaamLijst = new SelectList(db.domein.Select(m => m.domeinnaam).Distinct());
            if (warning != null)
            {
                paraZonderVm.domeinCodeBestaatNiet = warning;
            }else
            {
                paraZonderVm.domeinCodeBestaatNiet = "";
            }
                   
       
            ViewBag.uniqueDomein = new SelectList(db.domein.Select(m => m.domeinnaam).Distinct());


            return View(paraZonderVm);
        }

        //Functie wordt later veranderd naar afdeling
        //private IEnumerable<SelectListItem> getAfdeling()
        //{
        //    PSMSEntities db = new PSMSEntities();
        //    var afdelingList = from p in db.afdeling
        //                      select new SelectListItem { Text = p.afdelingsnaam, Value = "1" };
        //    return afdelingList;
        //}

        // POST: Parameter/Create
        [HttpPost]
        public ActionResult ParameterZonderSubtypeToevoegen(ParameterZonderSubtypeToevoegenViewModel pm, string selectDomeinNaam, string DomeinCode)
        {
            PSMSEntities db = new PSMSEntities();
            var paraZonderVm = new ParameterZonderSubtypeToevoegenViewModel();
            //Voorlopig hoeft rechten toewijzen niet worden gedaan via de GUI.
            //paraZonderVm.Afdeling = new List<SelectListItem>();
            //paraZonderVm.Afdeling = getAfdeling();
         
            paraZonderVm.DomeinnaamLijst = new SelectList(db.domein.Select(m => m.domeinnaam).Distinct());

            if (selectDomeinNaam != null && pm.paraZonderSubtype.datatypecode == null)
            {
                var DomeinCodeLijst = from p in db.domein
                                      where p.domeinnaam == selectDomeinNaam
                                      select p.domeincode;

                if (!DomeinCodeLijst.Contains(DomeinCode))
                {

                    paraZonderVm.domeinCodeBestaatNiet = "Domeincode bestaat niet in de database!";
                    return RedirectToAction("ParameterZonderSubtypeToevoegen", new { warning = paraZonderVm.domeinCodeBestaatNiet });
                }
                else if (DomeinCodeLijst.Contains(DomeinCode))
                {
                    try
                    {
                        db.ParamToevoegen(pm.paraZonderSubtype.parameternaam, pm.paraZonderSubtype.waarde, null, selectDomeinNaam, DomeinCode, pm.paraZonderSubtype.beschrijving_kort, pm.paraZonderSubtype.beschrijving_lang,
                                           null, pm.paraZonderSubtype.geldigheidvan, pm.paraZonderSubtype.geldigheidtot);

                    }
                    catch (Exception)
                    {


                    }
                }
            }


                if (pm.paraZonderSubtype.datatypecode != null)
                {
                    try
                    {
                        db.ParamToevoegen(pm.paraZonderSubtype.parameternaam, pm.paraZonderSubtype.waarde, pm.paraZonderSubtype.datatypecode, null, null, pm.paraZonderSubtype.beschrijving_kort, pm.paraZonderSubtype.beschrijving_lang,
                                           null, pm.paraZonderSubtype.geldigheidvan, pm.paraZonderSubtype.geldigheidtot);

                    }
                    catch (Exception)
                    {


                    }
                }
            return View(paraZonderVm);
        }

        [HttpGet]
        public ActionResult Zoek()
        {
            var zoekVm = new ParameterZonderSubtypeZoekViewModel();
            PSMSEntities db = new PSMSEntities();
            var overZicht = db.parameterzondersubtype;
            zoekVm.para = overZicht;

            return View(zoekVm); 
        }

        [HttpPost]
        public ActionResult Zoek(ParameterZonderSubtypeZoekViewModel zoekVm)
        {
            PSMSEntities db = new PSMSEntities();
            var zoekResultaat = db.parameterzondersubtype.Where(m => m.parameternaam == zoekVm.Zoek);
            zoekVm.para = zoekResultaat;

            return View(zoekVm);
        }
        // GET: Parameter/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Parameter/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Parameter/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Parameter/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
