using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Mapping
{
    public static class MappingExtension
    {
        public static IList<TD> MapIList<TS, TD>(this IList<TS> items)
        {
            return AutoMapper.Mapper.Map<IList<TD>>(items);
        }

        public static T MapTo<T>(this object obj)
        {
            if (obj == null)
            {
                return default(T);
            }

            return AutoMapper.Mapper.Map<T>(obj);
        }
    }
}