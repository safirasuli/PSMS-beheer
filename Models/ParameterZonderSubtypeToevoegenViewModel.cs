using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PSMSData.Model;
using System.Web.Mvc;

namespace PSMS_Beheer.Models
{
    public class ParameterZonderSubtypeToevoegenViewModel
    {
        public parameterzondersubtype paraZonderSubtype { get; set; }
        public int[] idAfdelingWijzigen { get; set; }
        public IEnumerable<SelectListItem> Afdeling{ get; set; }
        public IEnumerable<SelectListItem> DomeinnaamLijst { get; set; }
        public SelectList domeinidList { get; set; }
        public int[] idAfdelingRecht { get; set; }
        public string domeinCodeBestaatNiet { get; set; }
    }
}