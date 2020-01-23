using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Full.Pirate.Library.Helpers
{
    public static class ObjectExtensions
    {

        public static ExpandoObject ShapeData<TSource>(this TSource source,
            string fields)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
           // var expandoObjectList = new ExpandoObject();
            var propertyInfoList = new List<PropertyInfo>();

            if (string.IsNullOrEmpty(fields))
            {
                var propertyInfos = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                propertyInfoList.AddRange(propertyInfos);
            }
            else
            {
                foreach (var field in fields.Split(','))
                {
                    var propertyName = field.Trim();
                    var propertyInfo = typeof(TSource).GetProperty(
                       propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    if (propertyInfo == null)
                    {
                        throw new Exception($"property {propertyName} not found on {typeof(TSource)}");
                    }
                    propertyInfoList.Add(propertyInfo);
                }
            }
 
            dynamic shapedObject = new ExpandoObject();
            foreach (var propertyInfo in propertyInfoList)
            {
                var value = propertyInfo.GetValue(source);
                ((IDictionary<string, object>)shapedObject).Add(propertyInfo.Name, value);
                
            }
 

            return shapedObject;
        }
    }
}
