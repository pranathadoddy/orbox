using Framework.Dto;
using Framework.ServiceContract.Response;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Framework.Application.Controllers
{
    public abstract class ApiBaseController : ControllerBase
    {
        protected ApiBaseController(
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

        protected string GetEmailTemplateFilePath()
        {
            var webRootDirectory = HostEnvironment.ContentRootPath;
            var emailTemplateDirectory = Path.Combine(webRootDirectory,
                Configuration.GetValue<string>("EmailConfiguration:EmailTemplatesFolder"));
            return emailTemplateDirectory;
        }


        protected JsonResult GetBasicSuccessJson()
        {
            return new JsonResult(new { IsSuccess = true });
        }

        protected JsonResult GetSuccessJson(BasicResponse response, object value)
        {
            return new JsonResult(new
            {
                IsSuccess = true,
                MessageInfoTextArray = response.GetMessageInfoTextArray(),
                Value = value
            });
        }

        protected JsonResult GetErrorJson(string[] messages)
        {
            return new JsonResult(new
            {
                IsSuccess = false,
                MessageErrorTextArray = messages
            });
        }

        protected JsonResult GetErrorJson(string message)
        {
            var messageArray = new[] { message };
            return new JsonResult(new
            {
                IsSuccess = false,
                MessageErrorTextArray = messageArray
            });
        }

        protected JsonResult GetErrorJson(BasicResponse response)
        {
            return new JsonResult(new
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

            return new JsonResult(jsonData);
        }

        protected void PopulateAuditFieldsOnCreate<T>(AuditableDto<T> dto, string username = "")
        {
            var currentUtcTime = DateTime.UtcNow;

            dto.CreatedBy = string.IsNullOrEmpty(username) ? User.Identity.Name : username;
            dto.CreatedDateTime = currentUtcTime;
            dto.LastModifiedBy = string.IsNullOrEmpty(username) ? User.Identity.Name : username;
            dto.LastModifiedDateTime = currentUtcTime;
        }

        protected void PopulateAuditFieldsOnUpdate<T>(AuditableDto<T> dto)
        {
            var currentUtcTime = DateTime.UtcNow;

            dto.LastModifiedBy = User.Identity.Name;
            dto.LastModifiedDateTime = currentUtcTime;
        }
    }
}
