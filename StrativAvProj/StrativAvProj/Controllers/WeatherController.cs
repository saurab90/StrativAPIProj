using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StrativProj.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace StrativAvProj.Controllers
{
    public class WeatherController : ApiController
    {
        // GET: api/Weather
        public IEnumerable<TemperatureCollection> GetCoolestPlace()
        {
            var urlDistrict = "https://raw.githubusercontent.com/strativ-dev/technical-screening-test/main/bd-districts.json";

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;


            JavaScriptSerializer jss = new JavaScriptSerializer();
            string getUrl = urlDistrict;
            string getdata;

            HttpWebRequest webRequest = WebRequest.CreateHttp(getUrl);

            using (var webResponse = webRequest.GetResponse())
            using (var reader = new StreamReader(webResponse.GetResponseStream()))
            {
                getdata = reader.ReadToEnd();
            }

            var getjsondata = jss.Deserialize<DistData>(getdata);  // get JsonData string
            var fdata = getjsondata.districts.ToList();

            List<TemperatureCollection> nwTempList = new List<TemperatureCollection>(); 

            // loop all district latitude and longitude and pass as parameter in wether API
            foreach (var item in fdata) 
            {
                var lati = item.lat;
                var Longi = item.Long;
                var districtName = item.name;


                var urlWeatherLat = "https://api.open-meteo.com/v1/forecast?latitude=" + lati;
                var urlWeatherLong = "&longitude=" + Longi;
                var urlTemp = urlWeatherLat + urlWeatherLong + "&hourly=temperature_2m";

                string getweatherUrl = urlTemp;
                string getweatherdata;

                HttpWebRequest webRequestWeather = WebRequest.CreateHttp(getweatherUrl);

                using (var webResponse = webRequestWeather.GetResponse())
                using (var reader = new StreamReader(webResponse.GetResponseStream()))
                {
                    getweatherdata = reader.ReadToEnd();
                }

                WeatherUpdateData wupData = new WeatherUpdateData(getweatherdata); // json data string
                var tim = wupData.time;
                var tem = wupData.temperature_2m;

                ArrayList lstTim = new ArrayList(tim);
                ArrayList lstTem = new ArrayList(tem);

                ArrayList arrayList = new ArrayList();
                List<TemperatureCollection> lstTempp = new List<TemperatureCollection>();

                for (int m = 0; m < lstTim.Count; m++)  // join array Time/Date with Temperature using index
                {
                    var dtime = lstTim[m].ToString();
                    var spdtime = dtime.Split('T');

                    var dtemp = lstTem[m].ToString();

                    var time14 = (string)spdtime[1];

                    if (time14 == "14:00")
                    {
                        TemperatureCollection temperatureCollection = new TemperatureCollection();

                        temperatureCollection.Latitude = lati;
                        temperatureCollection.Longitude = Longi;
                        temperatureCollection.DistrictName = districtName;
                        temperatureCollection.TemDate = (string)spdtime[0];
                        temperatureCollection.Time = (string)spdtime[1];
                        temperatureCollection.Temperature = (string)dtemp;

                        lstTempp.Add(temperatureCollection);
                    }
                }

                // take lowest tem from 7 tem of 1 district each
                var lowTemData = lstTempp.OrderBy(x => x.Temperature).FirstOrDefault(); 

                TemperatureCollection coolestDistrictTemperature2Pm = new TemperatureCollection();

                coolestDistrictTemperature2Pm.Latitude = lowTemData.Latitude;
                coolestDistrictTemperature2Pm.Longitude = lowTemData.Longitude;
                coolestDistrictTemperature2Pm.TemDate = lowTemData.TemDate;
                coolestDistrictTemperature2Pm.Temperature = lowTemData.Temperature;
                coolestDistrictTemperature2Pm.Time = lowTemData.Time;
                coolestDistrictTemperature2Pm.DistrictName = lowTemData.DistrictName;

                nwTempList.Add(coolestDistrictTemperature2Pm);
            }

            var coolest10Place = nwTempList.OrderBy(x => x.Temperature).Take(10);

            return coolest10Place;  // return list of JsonData
        }

        // GET: api/Weather/5
        public string Get(int id)
        {
            return "value";
        }

        public IEnumerable<TemperatureCollection> AllDistrictNext7DayTemp()
        {
            var urlDistrict = "https://raw.githubusercontent.com/strativ-dev/technical-screening-test/main/bd-districts.json";

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;


            JavaScriptSerializer jss = new JavaScriptSerializer();
            string getUrl = urlDistrict;
            string getdata;

            HttpWebRequest webRequest = WebRequest.CreateHttp(getUrl);

            using (var webResponse = webRequest.GetResponse())
            using (var reader = new StreamReader(webResponse.GetResponseStream()))
            {
                getdata = reader.ReadToEnd();
            }

            var getjsondata = jss.Deserialize<DistData>(getdata);  // get dynamic JsonData
            var fdata = getjsondata.districts.ToList();

            List<TemperatureCollection> lstTempp = new List<TemperatureCollection>();

            foreach (var item in fdata)
            {
                var lati = item.lat;
                var Longi = item.Long;
                var districtName = item.name;


                var urlWeatherLat = "https://api.open-meteo.com/v1/forecast?latitude=" + lati;
                var urlWeatherLong = "&longitude=" + Longi;
                var urlTemp = urlWeatherLat + urlWeatherLong + "&hourly=temperature_2m";

                string getweatherUrl = urlTemp;
                string getweatherdata;

                HttpWebRequest webRequestWeather = WebRequest.CreateHttp(getweatherUrl);

                using (var webResponse = webRequestWeather.GetResponse())
                using (var reader = new StreamReader(webResponse.GetResponseStream()))
                {
                    getweatherdata = reader.ReadToEnd();
                }

                WeatherUpdateData wupData = new WeatherUpdateData(getweatherdata);

                var tim = wupData.time;
                var tem = wupData.temperature_2m;

                ArrayList lstTim = new ArrayList(tim);
                ArrayList lstTem = new ArrayList(tem);

                ArrayList arrayList = new ArrayList();

                for (int m = 0; m < lstTim.Count; m++)
                {
                    var dtime = lstTim[m].ToString();
                    var spdtime = dtime.Split('T');

                    var dtemp = lstTem[m].ToString();

                    var time14 = (string)spdtime[1];


                    if (time14 == "14:00")
                    {
                        TemperatureCollection temperatureCollection = new TemperatureCollection();

                        temperatureCollection.Latitude = lati;
                        temperatureCollection.Longitude = Longi;
                        temperatureCollection.DistrictName = districtName;
                        temperatureCollection.TemDate = (string)spdtime[0];
                        temperatureCollection.Time = (string)spdtime[1];
                        temperatureCollection.Temperature = (string)dtemp;

                        lstTempp.Add(temperatureCollection);
                        
                    }
                }
            }
            return lstTempp;
        }

        // POST: api/Weather
        public IHttpActionResult PostTempUpdate(TemperaturePost temperaturePost)
        {
           var tempDataAll = AllDistrictNext7DayTemp().ToList();
            
            var fltDataTemCurrLocation =  tempDataAll
                                            .Where(x => x.DistrictName == temperaturePost.CurrentLocation
                                                   && x.TemDate == temperaturePost.SearchDate).OrderBy(y => y.Temperature).ToList();

            var fltDataTemDestLocation = tempDataAll
                                          .Where(x => x.DistrictName == temperaturePost.Destination
                                                   && x.TemDate == temperaturePost.SearchDate);



            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }

            return Ok(new { success = "Request Successfully Submited", objLocationTemperatureData = fltDataTemCurrLocation, objDestinationTemperatureData = fltDataTemDestLocation });
        }


        // PUT: api/Weather/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Weather/5
        public void Delete(int id)
        {
        }
    }
}
