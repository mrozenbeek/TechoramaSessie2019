using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TechoramaSessie.API.Core;
using TechoramaSessie.API.Routing.Models;

namespace TechoramaSessie.API.Routing.Controllers
{
    public class ValuesController : BaseController
    {
        private static IList<string> _data = new List<string>() {
             "value1", "value2"
        };

        // GET api/values
        [HttpGet]
        public ActionResult<IList<string>> Get()
        {
            return Ok(_data);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return Ok( _data[id]);
        }

        // POST api/values
        [HttpPost]
        public ActionResult<string> Post([FromBody] ExampleViewModel model)
        {
            //if (string.IsNullOrWhiteSpace(model.Value))
            //    return BadRequest();

            if (ModelState.IsValid == false)
                return BadRequest();

            string manipulatedString = $"added '{model.Value}' by using the post ";

            _data.Add(manipulatedString);

            return Ok(manipulatedString);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (id < 0)
                return BadRequest();

            //Will fail if id exceeds the length of the list....
            _data.RemoveAt(id);

            return Ok();
        }
    }
}
