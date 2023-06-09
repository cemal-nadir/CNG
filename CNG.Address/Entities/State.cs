﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNG.Address.Entities
{
    public class State
    {

        public int Id { get; set; }
        public int CountryId { get; set; }
        public string? Name { get; set; }
        public string? CountryCode { get; set; }
        public string? CountryName { get; set; }
        public string? StateCode { get; set; }
        public string? Type { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public virtual Country? Country { get; set; }
        public virtual ICollection<City>?Cities { get; set; }

    }
}
