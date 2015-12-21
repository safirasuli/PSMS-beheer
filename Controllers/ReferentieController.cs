using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.WebPages;
using PSMSData.Code;
using PSMSData.Code.Entities;

namespace PSMS_Beheer.Controllers
{
    public class ReferentieController : Controller
    {
        // GET: Referentie
        public ActionResult Index()
        {
            var model = new ReferentieTabelModel();

            return View(model);
        }

        public ActionResult TabelToevoegen()
        {
            var model = new ReferentieTabelModel();
            return View(model);
        }

        public ActionResult KolommenDefinieren(string tabelNaam, int aantalKolommen, FormCollection form)
        {
            var model = new ReferentieTabelModel();
            model.AantalKolommen = aantalKolommen;
            model.TabelNaam = tabelNaam;

            return View(model);
        }

        public ActionResult RijenInvoeren(string tabelNaam, int aantalKolommen, FormCollection form)
        {
            var model = new ReferentieTabelModel();
            if (!TryValidateModel(model, "")) return null;

            var kolomKeys = form.AllKeys.Where(m => m.Contains("Kolomnaam") && !m.Contains("datatype") && !m.Contains("domein")).ToList();
            model.Kolommen = new List<Kolommen>();
            foreach (var kolom in kolomKeys)
            {
                model.Kolommen.Add(new Kolommen {Kolom = kolom, Kolomnaam = form.GetValue(kolom).AttemptedValue, Datatype = form.GetValue(kolom + "datatype").AttemptedValue ?? string.Empty, Domein = form.GetValue(kolom + "domein").AttemptedValue ?? string.Empty });
            }
            model.TabelNaam = tabelNaam;
            model.AantalKolommen = kolomKeys.Count;

            return View(model);
        }

        public ActionResult Voltooien(string tabelNaam, int aantalKolommen, string kolomnamen, FormCollection form)
        {
            var model = new ReferentieTabelModel();
            if (!TryUpdateModel(model, "")) return null;
            model.TabelNaam = tabelNaam;
            model.AantalKolommen = aantalKolommen;
            model.Rijen = new Dictionary<int, Dictionary<string, string>>();

            JavaScriptSerializer ser = new JavaScriptSerializer();
            model.Kolommen = ser.Deserialize<List<Kolommen>>(kolomnamen);

            var blaap = form.AllKeys.Where(m => m.Contains("Rij")).ToList();
            foreach (var key in blaap)
            {
                var kolom = key.Substring(key.IndexOf('K'));
                var rij = key.Substring(0, (key.Length - kolom.Length));

                var rijnummer = rij.Substring(3).AsInt();

                if (model.Rijen.ContainsKey(rijnummer))
                {
                    model.Rijen[rijnummer].Add(kolom, form.GetValue(key).AttemptedValue);
                }
                else
                {
                    var temp = new Dictionary<string, string>();
                    temp.Add(kolom, form.GetValue(key).AttemptedValue);
                    model.Rijen.Add(rijnummer, temp);
                }


            }

            Referentie.AddReferentieTabel(model);

            return View(model);
        }



        public ActionResult GetDatatypesForAutoCompletion(string term)
        {
            return Json((Datatype.GetDataTypesOpString(term)).ToArray(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDomeinenForAutoCompletion(string term)
        {
            return Json((Domein.GetDomeinenOpString(term)).ToArray(), JsonRequestBehavior.AllowGet);
        }


        public ActionResult ReferentieWijzigen()
        {
            var model = new ReferentieTabelModel();



            return View(model);
        }

    }
}