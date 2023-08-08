using Framework.Dto;
using Framework.ServiceContract.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Framework.Application.Controllers
{
    public abstract class BaseController : Controller
    {
        protected BaseController(
            IConfiguration configuration, 
            IHostEnvironment hostEnvironment)
        {
            Configuration = configuration;
            HostEnvironment = hostEnvironment;
        }

        protected IConfiguration Configuration { get; }

        protected IHostEnvironment HostEnvironment { get; }

        private IEnumerable<string> GetModelStateError()
        {
            return ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage).ToList();
        }

        protected JsonResult GetBasicSuccessJson()
        {
            return Json(new { IsSuccess = true });
        }

        protected JsonResult GetSuccessJson(BasicResponse response, object value)
        {
            return Json(new
            {
                IsSuccess = true,
                MessageInfoTextArray = response.GetMessageInfoTextArray(),
                Value = value
            });
        }

        protected JsonResult GetErrorJson(string[] messages)
        {
            return Json(new
            {
                IsSuccess = false,
                MessageErrorTextArray = messages
            });
        }

        protected JsonResult GetErrorJson(string message)
        {
            var messageArray = new[] { message };
            return Json(new
            {
                IsSuccess = false,
                MessageErrorTextArray = messageArray
            });
        }

        protected JsonResult GetErrorJson(BasicResponse response)
        {
            return Json(new
            {
                IsSuccess = false,
                MessageErrorTextArray = response.GetMessageErrorTextArray()
            });
        }

        protected JsonResult GetErrorJsonFromModelState()
        {
            return GetErrorJson(GetModelStateError().ToArray());
        }

        protected ActionResult GetPagedSearchGridJson<TDto>(int pageIndex,
            int pageSize,
            List<object> rowJsonData,
            GenericPagedSearchResponse<TDto> response)
        {
            var jsonData = new
            {
                current = pageIndex,
                rowCount = pageSize,
                rows = rowJsonData,
                total = response.TotalCount
            };

            return Json(jsonData);
        }

        protected void PopulateAuditFieldsOnCreate<T>(AuditableDto<T> dto)
        {
            var currentUtcTime = DateTime.UtcNow;

            dto.CreatedBy = User.Identity.IsAuthenticated ? User.Identity.Name : "system";
            dto.CreatedDateTime = currentUtcTime;
            dto.LastModifiedBy = User.Identity.IsAuthenticated ? User.Identity.Name : "system";
            dto.LastModifiedDateTime = currentUtcTime;
        }

        protected void PopulateAuditFieldsOnUpdate<T>(AuditableDto<T> dto)
        {
            var currentUtcTime = DateTime.UtcNow;

            dto.LastModifiedBy = User.Identity.IsAuthenticated ? User.Identity.Name : "system";
            dto.LastModifiedDateTime = currentUtcTime;
        }
    }
}
