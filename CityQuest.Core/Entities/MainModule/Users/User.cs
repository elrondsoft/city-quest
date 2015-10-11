using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Entities.MainModule.Users
{
    public class User : FullAuditedEntity<long, User>
    {
        public string Name { get; set; }
    }
}
