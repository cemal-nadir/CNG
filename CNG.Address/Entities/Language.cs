using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNG.Abstractions.Signatures;

namespace CNG.Address.Entities
{
    public class Language:IEntity<string>
    {
        public string Id { get; set; } = null!;
        public string? Description { get; set; }
        public virtual ICollection<Translation>?Translations { get; set; }
    }
}
