using Full.Pirate.Library.Services.Sorting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Full.Pirate.Library.Helpers
{
    public static class IQueryableExtensions
    {

        public static IQueryable<T> CreateSort<T>(this IQueryable<T> source, Dictionary<string, PropertyMappingValue> mappingDictionary, string orderBy)
        {
            //orderBy = "Name desc, Age"
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (mappingDictionary == null)
            {
                throw new ArgumentNullException(nameof(mappingDictionary));
            }

            if (string.IsNullOrWhiteSpace(orderBy))
            {
                return source;
            }

            var orderBySplit = orderBy.Split(',');
            foreach (var order in orderBySplit.Reverse())
            {

                var orderTrim = order.Trim();
                bool orderDescending = orderTrim.EndsWith(" desc");
                if (orderDescending)
                {
                    orderTrim = orderTrim.Remove(orderTrim.IndexOf(" "));
                }

                if (mappingDictionary.ContainsKey(orderTrim))
                {
                    var mappingColumns = mappingDictionary[orderTrim];
                    if (mappingColumns == null)
                    {
                        throw new ArgumentNullException("propertyMappingDict");
                    }
                    orderDescending = (mappingColumns.Revert == true) ? !orderDescending : orderDescending;


                    foreach (var mapColumn in mappingColumns.DestinationProperties.Reverse())
                    {
                        source = source.OrderBy(mapColumn + (orderDescending ? " descending" : " ascending"));
                    }
                }
                else
                {
                    throw new ArgumentException($"Key mapping for {orderTrim} is missing");
                }
            }
            return source;
        }
    }
}
