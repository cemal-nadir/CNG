using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNG.Abstractions.Signatures;

namespace CNG.Address.Entities
{
    public class Timezone:IEntity<Guid>
    {
        public Guid Id { get; set; }
        public int CountryId { get; set; }
        public string? ZoneName { get; set; }
        public int? GmtOffset { get; set; }
        public string? GmtOffsetName { get; set; }
        public string? Abbreviation { get; set; }
        public string? TzName { get; set; }
        public virtual Country? Country { get; set; }
    }
}
