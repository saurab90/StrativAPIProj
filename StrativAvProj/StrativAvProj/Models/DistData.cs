using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StrativAvProj.Models
{
    public class DistData
    {
        public string id { get; set; }
        public string division_id { get; set; }
        public string name { get; set; }
        public string bn_name { get; set; }
        public string lat { get; set; }
        public string Long { get; set; }
        public DistData[] districts { get; set; }
    }
   
}