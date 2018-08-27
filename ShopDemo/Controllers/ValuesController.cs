using ShopDemo.BusinessLogic;
using ShopDemo.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;

namespace ShopDemo.Controllers
{
    public class ValuesController : ApiController
    {
        private Services Services { get; set; }

        public ValuesController()
        {
            Services = new Services();
        }

        // GET api/<controller>
        public IEnumerable<Product> Get()
        {
            var list = Services.GetAllProducts();
            return list;
        }

        // GET api/<controller>/5
        public Product Get(int id)
        {
            var product = Services.GetProductById(id);
            return product;
        }


        /* Sin usar
        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }*/


    }
}