using StrativAvProj.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace StrativAvProj.Controllers
{
    public class WeatherUpdateController : ApiController
    {
        // GET: api/WeatherUpdate
        public IHttpActionResult GetCoolestPlace()
        {
            string urlDistrict = "https://raw.githubusercontent.com/strativ-dev/technical-screening-test/main/bd-districts.json";

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;


            JavaScriptSerializer jss = new JavaScriptSerializer();
            string getUrl = urlDistrict;
            string getdata;

            HttpWebRequest webRequest = WebRequest.CreateHttp(getUrl);

            using (WebResponse webResponse = webRequest.GetResponse())
            using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
            {
                getdata = reader.ReadToEnd();
            }

            DistData getjsondata = jss.Deserialize<DistData>(getdata);  // get JsonData string
            List<DistData> fdata = getjsondata.districts.ToList();

            List<TemperatureCollection> nwTempList = new List<TemperatureCollection>();

            // loop all district latitude and longitude and pass as parameter in wether API
            foreach (DistData item in fdata)
            {
                string lati = item.lat;
                string Longi = item.Long;
                string districtName = item.name;


                string urlWeatherLat = "https://api.open-meteo.com/v1/forecast?latitude=" + lati;
                string urlWeatherLong = "&longitude=" + Longi;
                string urlTemp = urlWeatherLat + urlWeatherLong + "&hourly=temperature_2m";

                string getweatherUrl = urlTemp;
                string getweatherdata;

                HttpWebRequest webRequestWeather = WebRequest.CreateHttp(getweatherUrl);

                using (WebResponse webResponse = webRequestWeather.GetResponse())
                using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
                {
                    getweatherdata = reader.ReadToEnd();
                }

                WeatherUpdateData wupData = new WeatherUpdateData(getweatherdata); // json data string
                Array tim = wupData.time;
                Array tem = wupData.temperature_2m;

                ArrayList lstTim = new ArrayList(tim);
                ArrayList lstTem = new ArrayList(tem);

                ArrayList arrayList = new ArrayList();
                List<TemperatureCollection> lstTempp = new List<TemperatureCollection>();

                for (int m = 0; m < lstTim.Count; m++)  // join array Time/Date with Temperature using index
                {
                    string dtime = lstTim[m].ToString();
                    string[] spdtime = dtime.Split('T');

                    string dtemp = lstTem[m].ToString();

                    string time14 = (string)spdtime[1];

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
                TemperatureCollection lowTemData = lstTempp.OrderBy(x => x.Temperature).FirstOrDefault();
                nwTempList.Add(lowTemData);
            }

            IEnumerable<TemperatureCollection> coolest10Place = nwTempList.OrderBy(x => x.Temperature).Take(10);
            //List<TemperatureCollection> coolest10Place = nwTempList.OrderBy(x => x.Temperature).Take(10).ToList();

            return Ok(coolest10Place);  // return list of JsonData
            //return (IEnumerable<TemperatureCollection>)Ok(new { success = "Request Successfully Submited", objCoolest10PlaceData = coolest10Place });

        }

        // GET: api/WeatherUpdate/5
        public string Get(int id)
        {
            return "value";
        }

        [Route("api/WeatherUpdate/AllDistrictTemData")]   // route the name as AllDistrictTemData
        public IEnumerable<TemperatureCollection> AllDistrictNext7DayTemp()
        {
            string urlDistrict = "https://raw.githubusercontent.com/strativ-dev/technical-screening-test/main/bd-districts.json";

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;


            JavaScriptSerializer jss = new JavaScriptSerializer();
            string getUrl = urlDistrict;
            string getdata;

            HttpWebRequest webRequest = WebRequest.CreateHttp(getUrl);

            using (WebResponse webResponse = webRequest.GetResponse())
            using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
            {
                getdata = reader.ReadToEnd();
            }

            DistData getjsondata = jss.Deserialize<DistData>(getdata);  // get dynamic JsonData
            List<DistData> fdata = getjsondata.districts.ToList();

            List<TemperatureCollection> lstTempp = new List<TemperatureCollection>();

            foreach (DistData item in fdata)
            {
                string lati = item.lat;
                string Longi = item.Long;
                string districtName = item.name;


                string urlWeatherLat = "https://api.open-meteo.com/v1/forecast?latitude=" + lati;
                string urlWeatherLong = "&longitude=" + Longi;
                string urlTemp = urlWeatherLat + urlWeatherLong + "&hourly=temperature_2m";

                string getweatherUrl = urlTemp;
                string getweatherdata;

                HttpWebRequest webRequestWeather = WebRequest.CreateHttp(getweatherUrl);

                using (WebResponse webResponse = webRequestWeather.GetResponse())
                using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
                {
                    getweatherdata = reader.ReadToEnd();
                }

                WeatherUpdateData wupData = new WeatherUpdateData(getweatherdata);

                Array tim = wupData.time;
                Array tem = wupData.temperature_2m;

                ArrayList lstTim = new ArrayList(tim);
                ArrayList lstTem = new ArrayList(tem);

                ArrayList arrayList = new ArrayList();

                for (int m = 0; m < lstTim.Count; m++)
                {
                    string dtime = lstTim[m].ToString();
                    string[] spdtime = dtime.Split('T');

                    string dtemp = lstTem[m].ToString();

                    string time14 = (string)spdtime[1];


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
            //return (IQueryable<TemperatureCollection>)lstTempp;
        }


        // POST: api/WeatherUpdate
        public IHttpActionResult PostTempUpdate(TemperaturePost temperaturePost)
        {
           // List<TemperatureCollection> tempDataAll = AllDistrictNext7DayTemp().ToList();
            IEnumerable<TemperatureCollection> tempDataAll = AllDistrictNext7DayTemp();

            IEnumerable<TemperatureCollection> fltDataTemCurrLocation = tempDataAll
                                            .Where(x => x.DistrictName == temperaturePost.CurrentLocation
                                                   && x.TemDate == temperaturePost.SearchDate);

            IEnumerable<TemperatureCollection> fltDataTemDestLocation = tempDataAll
                                          .Where(x => x.DistrictName == temperaturePost.Destination
                                                   && x.TemDate == temperaturePost.SearchDate);

            //List<TemperatureCollection> fltDataTemDestLocation = tempDataAll
            //                              .Where(x => x.DistrictName == temperaturePost.Destination
            //                                       && x.TemDate == temperaturePost.SearchDate).ToList();

            List<TemperatureCollection> nwList = new List<TemperatureCollection>();
            nwList.Add(fltDataTemCurrLocation.FirstOrDefault());
            nwList.Add(fltDataTemDestLocation.FirstOrDefault());

            var compareData = new
            {
                //objLocationTemperatureData = fltDataTemCurrLocation,
                //objDestinationTemperatureData = fltDataTemDestLocation
                allData = nwList
            };

            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }

            //return Ok(new { success = "Request Successfully Submited", objLocationTemperatureData = fltDataTemCurrLocation, objDestinationTemperatureData = fltDataTemDestLocation });
            return Ok(compareData);
        }


        // PUT: api/WeatherUpdate/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/WeatherUpdate/5
        public void Delete(int id)
        {
        }

    }
}
