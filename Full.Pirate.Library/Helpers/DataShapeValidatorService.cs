using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Full.Pirate.Library.Helpers
{
    public class DataShapeValidatorService : IDataShapeValidatorService
    {
        public bool CheckFieldsExist<T>(string fields)
        {
            if (string.IsNullOrEmpty(fields))
            {
                return true;
            }
            var repositoryType = typeof(T);
            foreach (var field in fields.Split(','))
            {
                if (repositoryType.GetProperty(field.Trim(), BindingFlags.Instance | BindingFlags.Public|BindingFlags.IgnoreCase) == null)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
