using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StrativAvProj.Controllers
{
    public class WeatherUpdateController : ApiController
    {
        // GET: api/WeatherUpdate
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/WeatherUpdate/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/WeatherUpdate
        public void Post([FromBody]string value)
        {
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
