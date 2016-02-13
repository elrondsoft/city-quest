using Abp.Dependency;
using Abp.Timing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest
{
    public static class Extensions
    {
        public static IEnumerable<string> GetAllExceptionMessages(this IEnumerable<Exception> ex)
        {
            return ex.SelectMany(r => r.GetAllExceptionMessages());
        }

        public static IEnumerable<string> GetAllExceptionMessages(this Exception ex)
        {
            if (ex == null)
                throw new ArgumentNullException("Null ex", "ex");
            IList<string> msg = new List<string>();
            var exType = ex.GetType();
            if (exType == typeof(AggregateException))
            {
                var aex = ex as AggregateException;
                msg.AddRange(aex.InnerExceptions.SelectMany(r => r.GetAllExceptionMessages()));
            }
            else if (ex.InnerException != null)
            {
                msg.Add(ex.Message);
                msg.AddRange(ex.InnerException.GetAllExceptionMessages());
            }
            else
            {
                msg.Add(ex.Message);
            }
            return msg;
        }

        public static Type GetRealEntityType(this Type sourceType)
        {
            if (sourceType.BaseType != null && sourceType.Namespace == "System.Data.Entity.DynamicProxies")
            {
                return sourceType.BaseType;
            }
            else
            {
                return sourceType;
            }
        }

        public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, string property, object value)
        {
            var type = typeof(TSource);
            var pe = Expression.Parameter(type, "p");
            var propertyReference = Expression.Property(pe, property);
            var constantReference = Expression.Constant(value);
            var filter = Expression.Lambda<Func<TSource, bool>>
                (Expression.Equal(propertyReference, constantReference),
                new[] { pe }).Compile();
            source = source.Where(filter);
            return source;
        }

        /// <summary>
        /// Alternative version of <see cref="Type.IsSubclassOf"/> that supports raw generic types (generic types without
        /// any type parameters).
        /// </summary>
        /// <param name="baseType">The base type class for which the check is made.</param>
        /// <param name="toCheck">To type to determine for whether it derives from <paramref name="baseType"/>.</param>
        public static bool IsSubclassOfRawGeneric(this Type toCheck, Type baseType)
        {
            if (toCheck == null)
                return false;
            if (baseType == null)
                return false;

            while (toCheck != null && toCheck != typeof(object))
            {
                Type cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (baseType == cur)
                {
                    return true;
                }

                toCheck = toCheck.BaseType;
            }

            return false;
        }

        public static bool HasDateTimeRangeSuperposition(this IEnumerable<DateTimeRange> ranges)
        {
            var timeRangesArray = ranges.OrderBy(r => r.StartTime).ToArray();
            if (timeRangesArray.Length < 2)
                return false;
            for (int i = 1; i < timeRangesArray.Length; i++)
            {
                if (timeRangesArray[i - 1].EndTime > timeRangesArray[i].StartTime)
                    return true;
            }
            return false;
        }

        public static void RemoveRange<T>(this IList<T> list, IEnumerable<T> itemsToRemove)
        {
            foreach (var itemToRemove in itemsToRemove)
            {
                list.Remove(itemToRemove);
            }
        }

        public static bool IsNullOrEmpty<T>(this ICollection<T> list)
        {
            if (list == null || list.Count == 0)
                return true;
            else
                return false;
        }

        public static void AddRange<T>(this IList<T> ilist, IEnumerable<T> newCollection)
        {
            var list = ilist as List<T>;
            if (list != null)
                list.AddRange(newCollection);
        }

        public static string EmptyIfNull(this string value)
        {
            if (value == null)
                return string.Empty;

            return value;
        }

        public static IQueryable<T> WhereIf<T>(this IQueryable<T> collection, bool condition, Expression<Func<T, bool>> predicate)
        {
            if (condition)
            {
                return collection.Where(predicate);
            }
            else
            {
                return collection;
            }
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            if (collection == null)
                return true;
            if (collection.Count() == 0)
                return true;
            return false;
        }

        public static string JoinAsString<T>(this IEnumerable<T> collection, string separator)
        {
            if (collection == null)
                return null;
            if (collection.Count() == 0)
                return null;
            return String.Join(separator, collection);
        }

        public static IDictionary<TKey, TValue> AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }

            return dictionary;
        }

        public static IEnumerable<T> ToSingleEnumerable<T>(this T obj)
        {
            return Enumerable.Repeat(obj, 1);
        }

        public static IDictionary<TKey, TValue> Merge<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
            params IDictionary<TKey, TValue>[] other)
        {
            if (other != null && other.Length > 0)
            {
                var newDicts = other.SelectMany(r => r);
                foreach (var item in newDicts)
                {
                    dictionary.AddOrUpdate(item.Key, item.Value);
                }
            }
            return dictionary;
        }

        public static DateTime RoundDateTime(this DateTime value)
        {
            return new DateTime(((value.Ticks + (CityQuestConsts.TicksToRoundDateTime / 2) + 1) /
                CityQuestConsts.TicksToRoundDateTime) * CityQuestConsts.TicksToRoundDateTime);
        }
    }
}
