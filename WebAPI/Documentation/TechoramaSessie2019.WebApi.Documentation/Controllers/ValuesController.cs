using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TechoramaSessie2019.WebApi.Documentation.Models;

namespace TechoramaSessie2019.WebApi.Documentation.Controllers
{
    [Route("api/[controller]")]
    //[Route("api/values")]
    //[Route("api/AdvancedRouting")]
    [ApiController]
    public class ValuesController : ControllerBase
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
        public ActionResult<string> Get(string id)
        {
            return Ok(_data.FirstOrDefault(el => el == id));
        }

        // POST api/values
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [HttpPost]
        public ActionResult<string> Post([FromBody] ExampleViewModel model)
        {

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
        [ProducesResponseType(404)]
        public ActionResult Delete(string id)
        {

            //returning not found will mention undocumenten http statuscode in de documentation.
            //[ProducesResponseType(404)] on this action will fix that.
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
