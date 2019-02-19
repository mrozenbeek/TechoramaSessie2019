using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using TechoramaSessie.API.Core;
using TechoramaSessie.API.Routing.Advanced.Models;

namespace TechoramaSessie.API.Routing.Advanced.Controllers
{
    //[Route("api/values")]
    [Route("api/AdvancedRouting")]
    [ApiController]
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
        [HttpGet("{id:maxlength(6):minlength(3)}")]
        public ActionResult<string> Get(string id)
        {
            return Ok(_data.FirstOrDefault(el => el == id));
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
        [HttpDelete("{id:maxlength(6):minlength(3)}")]
        public ActionResult Delete(string id)
        {
            //No need to check since we use routecontstraints.
            //if (id < 0)
            //    return BadRequest();

            //Will fail if id exceeds the length of the list....
            var removed = _data.Remove(id);

            return removed ? Ok() : (ActionResult)NotFound();
        }

        //Demo

        [HttpPost("AcceptVerbesDemo")]
        [HttpGet("AcceptVerbesDemo")]
        public ActionResult AcceptVerbesDemo()
        {
            return Ok("Multiple verbs example");
        }

        [NonAction]
        public ActionResult NotAnActualAPIMethod()
        {
            return Ok("Multiple verbs example");
        }
    }
}
