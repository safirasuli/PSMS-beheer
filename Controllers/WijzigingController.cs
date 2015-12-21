using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using PSMSData.Code.Entities;
using PSMSData.Model;
using PSMSData.Models;
using PSMSData.Code;
using PSMS_Beheer.Models;

namespace PSMS_Beheer.Controllers
{
    public class WijzigingController : Controller
    {
        // GET: Wijziging
        public ActionResult Index()
        {
            return viewFillBag();
        }

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            var model = new WijzigingTabelModel();
            PSMSEntities db = new PSMSEntities();
            foreach (String wijzigingId in form.Keys)
            {

                switch (db.wijziging.Where(m => m.wijzigingid == new Guid(wijzigingId)).Select(m => m.settingtype).FirstOrDefault())
                {
                    case "Domein":

                        break;

                    case "ParameterMetSubtype":

                        break;

                    case "ParameterZonderSubtype":

                        break;

                    case "Rangetabel":

                        break;

                    case "Referentie":

                        break;

                    default:

                        break;


                }         
                
            }
            return viewFillBag();
        }


        private ActionResult viewFillBag()
        {
            var model = new WijzigingTabelModel();
            PSMSEntities db = new PSMSEntities();

            var query = db.wijziging;

            var settingsTypeLijst = db.settingtype.Select(m => m.settingtype1);
            var wijzigingsLijst = query;

            var paraMetSubtype = query.Where(m => m.settingtype == "ParameterMetSubtype");
            var paraZonderSubtype = query.Where(m => m.settingtype == "ParameterZonderSubtype");


            model.ListWijzigingen = Wijziging.GetWijzigingen();
            model.ListSettingtypenWijzigingen = Wijziging.GetSettingtypenWijzigingen();
            model.ListParameterMetSubtype = Wijziging.GetParametersMetSubtypen();

            return View(model);
        }
    }
}