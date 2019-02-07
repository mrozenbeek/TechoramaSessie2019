using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TechoramaSessie.API.Routing.Models
{
    public class ExampleViewModel
    {
        [Required(AllowEmptyStrings = false)]
        //[MaxLength(5)]
        [StringLength(1,MinimumLength = 3)]
        public string Value { get; set; }
    }
}
