using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Extensions
{
    public static class Extensions
    {
        public static IEnumerable<T> AttachCollection<T>(this IEnumerable<T> collection, DbContext context)
            where T : class
        {
            foreach (var item in collection)
            {
                context.Set<T>().Attach(item);
            }
            return collection;
        }
    }
}
