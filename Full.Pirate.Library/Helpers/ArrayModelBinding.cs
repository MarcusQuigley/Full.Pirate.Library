using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Full.Pirate.Library.Helpers
{
    public class ArrayModelBinding : IModelBinder
    {

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
           if (!bindingContext.ModelMetadata.IsEnumerableType)
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).ToString();
            if (string.IsNullOrWhiteSpace(value))
            {
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;
            }

            var elementType = bindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0];
            var converter = TypeDescriptor.GetConverter(elementType);
            //var values = value.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
            var values = value.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                    .Select(stringValue => converter.ConvertFromString(stringValue.Trim()))
                                    .ToArray();

            var typedValues = Array.CreateInstance(elementType, values.Length);
            values.CopyTo(typedValues, 0);

            bindingContext.Model = typedValues;
            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
            return Task.CompletedTask;
        }

        //public Task BindModelAsync(ModelBindingContext bindingContext)
        //{
        //    if (!bindingContext.ModelMetadata.IsEnumerableType)
        //    {
        //        bindingContext.Result = ModelBindingResult.Failed();
        //        return Task.CompletedTask;
        //    }
        //    var value = bindingContext.ValueProvider
        //        .GetValue(bindingContext.ModelName).ToString();
        //    if (string.IsNullOrWhiteSpace(value))
        //    {
        //        bindingContext.Result = ModelBindingResult.Success(null);
        //        return Task.CompletedTask;
        //    }
        //    //find what type it is
        //    var elementType = bindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0];
        //    //convert the string value to the type of elementType above
        //    var converter = TypeDescriptor.GetConverter(elementType);
        //    var newValues = value.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
        //                         .Select(newVal => converter.ConvertFromString(newVal.Trim()))
        //                         .ToArray();
        //    var result = Array.CreateInstance(elementType, newValues.Length);
        //    newValues.CopyTo(result, 0);
        //    bindingContext.Model = result;

        //    bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
        //    return Task.CompletedTask;
        //}

    }
}
