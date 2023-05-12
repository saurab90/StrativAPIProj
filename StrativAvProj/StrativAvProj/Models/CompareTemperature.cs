using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StrativAvProj.Models
{
    public class CompareTemperature
    {
        public long Id { get; set; }
        public string TemDate { get; set; }
        public string Time { get; set; }
        public string Temperature { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string DistrictName { get; set; }
        public CompareTemperature[] allData { get; set; }
        
    }
}