using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PSMSData.Model;

namespace PSMS_Beheer.Models
{
    public class ParameterZonderSubtypeZoekViewModel 
    {
        public String parameterTypeDropdownlist { get; set; }
        public IEnumerable<parameterzondersubtype> para { get; set; }
        public String Zoek { get; set; }
       


    }
}