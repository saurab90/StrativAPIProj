using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StrativProj.Models
{
    public class WeatherUpdateData
    {
        public WeatherUpdateData(string json)
        {
            JObject jObject = JObject.Parse(json);
            JToken jtoken = jObject["hourly"];
            latitude = (string)jObject["latitude"];
            
            time = jtoken["time"].ToArray();
            temperature_2m = jtoken["temperature_2m"].ToArray();
        }

        public string latitude { get; set; }
        
        public Array time { get; set; }
        public Array temperature_2m { get; set; }
        
    }
}