using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Framework.Application.Presentation.HtmlHelperExtensions
{
    public static class DropDownHtmlHelperExtension
    {
        #region Public Methods

        public static IHtmlContent DropDownFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            SelectList items,
            string customClasses = "")
        {
            ModelExpressionProvider modelExpressionProvider = 
                (ModelExpressionProvider)htmlHelper.ViewContext.HttpContext.RequestServices.GetService(typeof(IModelExpressionProvider));

            var name = modelExpressionProvider.GetExpressionText(expression);
            var modelExplorer =
                modelExpressionProvider.CreateModelExpression(htmlHelper.ViewData, expression);

            var metadata = modelExplorer.Metadata;
            var model = modelExplorer.Model;

            return SimpleFormGroupDivTag(name,
                metadata.DisplayName,
                metadata.Description,
                items,
                model?.ToString(),
                metadata.IsRequired,
                metadata.IsReadOnly,
                customClasses: customClasses);
        }

        #endregion

        #region Private Methods

        private static IHtmlContent SimpleFormGroupDivTag(
            string name,
            string label,
            string placeholder,
            SelectList items,
            string value,
            bool isRequired,
            bool isReadOnly,
            string customClasses = "")
        {
            var formGroupDivTag = CommonHtmlHelperExtension.GetFormGroupDivTag(customClasses);

            if (!string.IsNullOrEmpty(label))
            {
                var labelTag = CommonHtmlHelperExtension.GetLabelTag(name, label, isRequired);
                formGroupDivTag.InnerHtml.AppendHtml(labelTag);
            }

            var dropDownTag = BaseDropDown(name,
                placeholder,
                items,
                value,
                isRequired,
                isReadOnly);

            formGroupDivTag.InnerHtml.AppendHtml(dropDownTag);

            return formGroupDivTag;
        }

        private static IHtmlContent BaseDropDown(
            string name,
            string placeholder,
            SelectList items,
            string value,
            bool isRequired,
            bool isReadOnly)
        {
            // The select tag
            var selectTag = new TagBuilder("select");
            selectTag.Attributes.Add("id", name);
            selectTag.Attributes.Add("name", name);
            selectTag.Attributes.Add("class", "form-control");

            if (isRequired)
                selectTag.Attributes.Add("required", "");

            if (isReadOnly)
                selectTag.Attributes.Add("disabled", "disabled");

            // Create the default option tag
            if (!string.IsNullOrEmpty(placeholder))
            {
                var defaultOptionTag = new TagBuilder("option");
                defaultOptionTag.Attributes.Add("value", "");
                defaultOptionTag.InnerHtml.AppendHtml(placeholder);
                selectTag.InnerHtml.AppendHtml(defaultOptionTag);
            }

            // The option tags
            foreach (var item in items)
            {
                var optionTag = new TagBuilder("option");
                optionTag.Attributes.Add("value", item.Value);

                if (string.IsNullOrEmpty(value))
                {
                    if (item.Selected)
                        optionTag.Attributes.Add("selected", "selected");
                }
                else
                {
                    if (item.Value == value)
                        optionTag.Attributes.Add("selected", "selected");
                }

                optionTag.InnerHtml.AppendHtml(item.Text);
                selectTag.InnerHtml.AppendHtml(optionTag);
            }

            return selectTag;
        }

        #endregion
    }
}