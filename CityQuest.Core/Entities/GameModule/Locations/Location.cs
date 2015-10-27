using Abp.Domain.Entities.Auditing;
using CityQuest.Entities.MainModule.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Entities.GameModule.Locations
{
    public class Location : FullAuditedEntity<long, User>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ShortName { get; set; }
        public string DisplayName { get; set; }
    }
}
