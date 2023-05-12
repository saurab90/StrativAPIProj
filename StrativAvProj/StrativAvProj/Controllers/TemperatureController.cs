using Newtonsoft.Json;
using StrativAvProj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace StrativAvProj.Controllers
{
    public class TemperatureController : Controller
    {
        //Hosted web API REST Service base url
        string Baseurl = "http://localhost:62742/";

        // GET: Temperature
        //public ActionResult Index()
        public async Task<ActionResult> Index()
        {
            IEnumerable<TemperatureCollectionUser> coolest10DistrictsInfo = new List<TemperatureCollectionUser>();
            
            using (HttpClient client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();

                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.Timeout = new TimeSpan(0, 5, 0); // 5 minute timeout

                //Sending request to find web api REST service resource GetCoolestPlace using HttpClient
                HttpResponseMessage Res = await client.GetAsync("api/WeatherUpdate");

                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    string dataResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the coolest10DistrictsInfo list
                    coolest10DistrictsInfo = JsonConvert.DeserializeObject<IEnumerable<TemperatureCollectionUser>>(dataResponse);
                }

                //returning the coolest10DistrictsInfo list to view
                return View(coolest10DistrictsInfo);
            }
        }

        // GET: Temperature/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Temperature/Create
        public ActionResult Create()
        {
            ViewBag.ResultAfterPost = null;
            return View();
        }

        // POST: Temperature/Create
        [HttpPost]
        public async Task<ActionResult> Create(TemperaturePost temperaturePost)
        {
            CompareTemperature compareTemperature  = new CompareTemperature();

            if (ModelState.IsValid)
            {
                using (HttpClient client = new HttpClient())
                {
                    //Passing service base url
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();

                    //Define request data format
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    client.Timeout = new TimeSpan(0, 5, 0); // 5 minute timeout

                    //Sending request to find web api REST service resource GetCoolestPlace using HttpClient
                    HttpResponseMessage Res = await client.PostAsJsonAsync("api/WeatherUpdate/TempUpdate", temperaturePost);

                    //Checking the response is successful or not which is sent using HttpClient
                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api
                        string dataResponse = Res.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the coolest10DistrictsInfo list
                        compareTemperature = JsonConvert.DeserializeObject<CompareTemperature>(dataResponse);

                        //JavaScriptSerializer jss = new JavaScriptSerializer();
                        //CompareTemperature getjsondata = jss.Deserialize<CompareTemperature>(dataResponse);  

                        var finalData = compareTemperature.allData.ToList();

                        ViewBag.ResultAfterPost = finalData;

                    }
                }
            }

            //returning the coolest10DistrictsInfo list to view
            return View();
        }

        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: Temperature/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Temperature/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Temperature/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Temperature/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
