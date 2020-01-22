using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Full.Pirate.Library.Services.Sorting
{
    public class PropertyMappingService : IPropertyMappingService
    {
        Dictionary<string, PropertyMappingValue> authorsMapping = new Dictionary<string, PropertyMappingValue>()
            {
                {"Name", new PropertyMappingValue(new List<string>(){"FirstName", "LastName"}) },
                {"MainCategory", new PropertyMappingValue(new List<string>(){"MainCategory"}) },
                {"Age", new PropertyMappingValue(new List<string>(){"DateOfBirth"},true) }
            };


        private IList<IPropertyMapping> propertyMapping = new List<IPropertyMapping>();
        public PropertyMappingService()
        {
            propertyMapping.Add(new PropertyMapping<Models.AuthorDto, Entities.Author>(authorsMapping));
        }

        public bool ValidMappingExistsFor<TSource, TDestination>(string fields)
        {
            if (string.IsNullOrEmpty(fields))
            {
                return false;
            }

            Dictionary<string, PropertyMappingValue> mappingDictionary = null;
            try
            {
                mappingDictionary = GetPropertyMapping<TSource, TDestination>();
            }
            catch (Exception)
            {
                return false;
            }

            var fieldsSplit = fields.Split(',');

            foreach (var field in fieldsSplit)
            {
                var trimmedField = field.Trim();
                int spaceIndex = trimmedField.IndexOf(" ");
                if (spaceIndex != -1)
                {
                    trimmedField = trimmedField.Remove(spaceIndex);
                }
                if (!mappingDictionary.ContainsKey(trimmedField))
                {
                    return false;
                }
            }
            return true;
        }
 
        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {
            var matchingMapping = propertyMapping.OfType<PropertyMapping<TSource, TDestination>>();
            if (matchingMapping.Count() == 1)
            {
                return matchingMapping.First().MappingProperties;
            }
            throw new ArgumentException($"Couldnt find mapping for {typeof(TSource)}, {typeof(TDestination)}");
        }
    }
}
