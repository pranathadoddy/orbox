using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Application.ModelBinder
{
    public class GridModelBinder : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            Dictionary<string, StringValues> queryCollection;

            if (bindingContext.HttpContext.Request.Method == "POST")
            {
                if (bindingContext.HttpContext.Request.ContentType.Contains("application/json"))
                {
                    using (StreamReader stream = new StreamReader(bindingContext.HttpContext.Request.Body))
                    {
                        var body = await stream.ReadToEndAsync();
                        var dictionaryResult = JsonConvert.DeserializeObject<Dictionary<string, string>>(body);
                        queryCollection = dictionaryResult.ToDictionary(item => item.Key, item => new StringValues(item.Value));
                    }
                }
                else
                {
                    queryCollection = bindingContext.HttpContext.Request.Form.ToDictionary(f => f.Key, f => f.Value);
                }
            }
            else
            {
                queryCollection = bindingContext.HttpContext.Request.Query.ToDictionary(q => q.Key, q => q.Value);
            }

            var model = Activator.CreateInstance(bindingContext.ModelType);
            var pageIndex = bindingContext.ModelType.GetProperty("PageIndex");
            var pageSize = bindingContext.ModelType.GetProperty("PageSize");
            var orderByFieldName = bindingContext.ModelType.GetProperty("OrderByFieldName");
            var sortOrder = bindingContext.ModelType.GetProperty("SortOrder");
            var keyword = bindingContext.ModelType.GetProperty("Keyword");
            var filters = bindingContext.ModelType.GetProperty("Filters");

            if (queryCollection.ContainsKey("current") && !string.IsNullOrEmpty(queryCollection["current"]))
            {
                pageIndex?.SetValue(model, int.Parse(queryCollection["current"]));
            }

            if (queryCollection.ContainsKey("rowCount") && !string.IsNullOrEmpty(queryCollection["rowCount"]))
            {
                pageSize?.SetValue(model, int.Parse(queryCollection["rowCount"]));
            }

            var sortKey = queryCollection.Keys.SingleOrDefault(item => item.Contains("sort"));
            if (!string.IsNullOrEmpty(sortKey) && !string.IsNullOrEmpty(queryCollection[sortKey]))
            {
                orderByFieldName?.SetValue(model, sortKey.Replace("sort", "").Replace("[", "").Replace("]", ""));
                sortOrder?.SetValue(model, queryCollection[sortKey].ToString());
            }

            if (queryCollection.ContainsKey("searchPhrase") && !string.IsNullOrEmpty(queryCollection["searchPhrase"]))
            {
                keyword?.SetValue(model, queryCollection["searchPhrase"].ToString());
            }

            if (queryCollection.ContainsKey("filters") && !string.IsNullOrEmpty(queryCollection["filters"]))
            {
                filters?.SetValue(model, queryCollection["filters"].ToString());
            }

            bindingContext.Result = ModelBindingResult.Success(model);
        }
    }
}
