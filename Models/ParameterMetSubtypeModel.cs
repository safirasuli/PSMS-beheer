using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PSMSData.Model;

namespace PSMS_Beheer.Models
{
    public class ParameterMetSubtypeModel
    {
        public string Datatype { get; set; }
        public string Domein { get; set; }

        public string ParameterNaam { get; set; }

        public List<parametermetsubtype> Parameter { get; set; }
    }
}