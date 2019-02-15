using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TechoramaSessie2019.WebApi.Documentation.Models
{
    public class ExampleViewModel
    {

            [Required(AllowEmptyStrings = false)]
            //[MaxLength(5)]
            [StringLength(6, MinimumLength = 3)]
            public string Value { get; set; }
        
    }
}
