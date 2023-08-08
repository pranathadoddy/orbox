using System.Collections.Generic;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Framework.Application.Presentation.HtmlHelperExtensions
{
    public static class CommonHtmlHelperExtension
    {
        #region Public Methods

        public static IHtmlContent SimpleDivPlaceholder(this IHtmlHelper htmlHelper, string customAttributeName = "")
        {
            var divTag = new TagBuilder("div");
            divTag.Attributes.Add(customAttributeName, "");

            return divTag;
        }

        public static IHtmlContent GroupedFormGroupDivTag(this IHtmlHelper htmlHelper, string label,
            ICollection<IHtmlContent> contents,
            string colCssClass = "col-6")
        {
            var formGroupDivTag = GetFormGroupDivTag();

            if (!string.IsNullOrEmpty(label))
            {
                var labelTag = GetLabelTag("", label, false);
                formGroupDivTag.InnerHtml.AppendHtml(labelTag);
            }

            var row = new TagBuilder("div");
            row.AddCssClass("row");

            foreach (var htmlContent in contents)
            {
                var col = new TagBuilder("div");
                col.AddCssClass(colCssClass);
                col.InnerHtml.AppendHtml(htmlContent);
                row.InnerHtml.AppendHtml(col);
            }

            formGroupDivTag.InnerHtml.AppendHtml(row);

            return formGroupDivTag;
        }

        public static TagBuilder GetFormGroupDivTag(string additionalClass = "")
        {
            var tag = new TagBuilder("div");

            if (!string.IsNullOrEmpty(additionalClass))
                tag.AddCssClass("form-group " + additionalClass);
            else
                tag.AddCssClass("form-group");

            return tag;
        }

        public static TagBuilder GetLabelTag(string name, string displayName, bool isRequired)
        {
            var tag = new TagBuilder("label");
            tag.AddCssClass("form-label");

            if (!string.IsNullOrEmpty(name))
                tag.Attributes.Add("for", name);

            if (isRequired)
                tag.InnerHtml.AppendHtml(displayName + " *");
            else
                tag.InnerHtml.AppendHtml(displayName);

            return tag;
        }

        #endregion
    }
}