using Abp.Domain.Entities.Auditing;
using CityQuest.Entities.MainModule.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Entities.Shared
{
    public class HasNameEntity : FullAuditedEntity<long, User>
    {
        public string Name { get; set; }
    }
}