using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Entities.Shared
{
    public class HasNameDescriptionEntity : HasNameEntity
    {
        public string Description { get; set; }
    }
}
