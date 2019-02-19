using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TechoramaSessie.API.Core;

namespace TechoramaSessie2091.WebApi.Documentationi.Controllers
{
    [ApiExplorerSettings(GroupName = "v2")]
    [ApiVersion("2.0")]
    public class Values2Controller : BaseController
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }
    }
}
