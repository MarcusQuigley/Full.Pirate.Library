using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Full.Pirate.Library.Services.Sorting
{
    public class PropertyMapping<TSource, TDestination> : IPropertyMapping
    {
        public PropertyMapping(Dictionary<string, PropertyMappingValue> mappingProperties)
        {
            MappingProperties = mappingProperties ?? throw new ArgumentNullException(nameof(mappingProperties));
        }

        public Dictionary<string, PropertyMappingValue> MappingProperties { get; private set; }
    }

    public interface IPropertyMapping
    {
    }
}
