using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.Http.Formatting;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using IOC.Abstraction.Business;

namespace IOC.Web.Controllers
{
    public class WebApiController 
        : ApiController
    {
        private readonly AbstractNoticiaBusiness _business;

        public WebApiController(AbstractNoticiaBusiness business)
        {
            this._business = business;
        }

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[]
            { 
                "value1", 
                "value2" 
            };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public ObjectTest Post([FromBody]ObjectTest test)
        {
            return test;
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }

    public class ObjectTest
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public char Sex { get; set; }

        public int Age { get; set; }

        public DateTime Create { get; set; }

        public DateTime? Update { get; set; }

        public bool Active { get; set; }
    }
}


//XmlSerializer xml = new XmlSerializer(typeof(ObjectTest));
//var t1 = new ObjectTest
//{
//    ID = 1,
//    Name = "Lucas",
//    Sex = 'm',
//    Age = 25,
//    Create = DateTime.Now,
//    Active = true
//};

//MemoryStream ms = new MemoryStream();
//DataContractSerializer s = new DataContractSerializer(typeof(ObjectTest));
//s.WriteObject(ms, t1);
//ms.Close();
//string xmlText = Encoding.Default.GetString(ms.ToArray());

//ms = new MemoryStream();
//DataContractJsonSerializer j = new DataContractJsonSerializer(typeof(ObjectTest));
//j.WriteObject(ms, t1);
//ms.Close();
//xmlText = Encoding.Default.GetString(ms.ToArray());