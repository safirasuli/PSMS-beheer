using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PSMSData.Model;

namespace PSMS_Beheer.Models
{
    public class ParameterModel
    {
        //domeinnaam wordt niet meer gebruikt. 
        public String ParameterNaam { get; set; }
        public String Datatype { get; set; }
        public String Domein { get; set; }
        public String Waarde { get; set; }
        public String beschrijvingKort  { get; set; }
        public String BeschrijvingLang { get; set; }
        public DateTime? GeldigheidVan { get; set; }
        public DateTime? GeldigheidTot { get; set; }
    }
}