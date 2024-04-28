using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StarWarsAPI.Models
{
    public class PlanetResponse
    {   
        public int count { get; set; }
        public string next { get; set; }
        public object previous { get; set; }
        public List<PlanetResult> results { get; set; }
    }
}
