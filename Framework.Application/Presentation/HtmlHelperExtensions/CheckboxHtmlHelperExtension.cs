using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Framework.Application.Presentation.HtmlHelperExtensions
{
    public static class CheckboxHtmlHelperExtension
    {
        public static IHtmlContent SwitcherFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, string onString = "On", string offString = "Off", string additionalClass="")
        {
            ModelExpressionProvider modelExpressionProvider =
                (ModelExpressionProvider)htmlHelper.ViewContext.HttpContext.RequestServices.GetService(typeof(IModelExpressionProvider));

            var name = modelExpressionProvider.GetExpressionText(expression);
            var modelExplorer =
                modelExpressionProvider.CreateModelExpression(htmlHelper.ViewData, expression);

            var metadata = modelExplorer.Metadata;
            var model = modelExplorer.Model;

            var isChecked = (bool?) model ?? false;

            return Switcher(name, metadata.DisplayName, metadata.IsReadOnly, isChecked, onString, offString, additionalClass);
        }

        private static IHtmlContent Switcher(
            string name,
            string label,
            bool isReadOnly,
            bool isChecked,
            string onString,
            string offString,
            string additionalClass = "")
        {
            var formGroupDivTag = CommonHtmlHelperExtension.GetFormGroupDivTag(additionalClass);

            if (!string.IsNullOrEmpty(label))
            {
                var labelTag = CommonHtmlHelperExtension.GetLabelTag(name, label, false);
                formGroupDivTag.InnerHtml.AppendHtml(labelTag);
            }

            var checkbox = BaseCheckbox(name, true.ToString(), isReadOnly, false, isChecked, "custom-switch-input");
            var checkboxTagBuilder = checkbox as TagBuilder;
            checkboxTagBuilder.Attributes.Add("data-toggle", "toggle");
            checkboxTagBuilder.Attributes.Add("data-style", "slow");
            checkboxTagBuilder.Attributes.Add("data-on", onString);
            checkboxTagBuilder.Attributes.Add("data-off", offString);

            formGroupDivTag.InnerHtml.AppendHtml(checkbox);

            return formGroupDivTag;
        }

        private static IHtmlContent BaseCheckbox(
            string name,
            string value,
            bool isReadOnly,
            bool isRequired,
            bool isChecked,
            string customClass)
        {
            var mainControlTag = new TagBuilder("input");
            mainControlTag.Attributes.Add("type", "checkbox");

            if (!string.IsNullOrEmpty(customClass)) mainControlTag.AddCssClass(customClass);

            mainControlTag.Attributes.Add("id", name);
            mainControlTag.Attributes.Add("name", name);
            mainControlTag.Attributes.Add("value", value);

            if (isReadOnly)
                mainControlTag.Attributes.Add("readonly", "readonly");

            if (isRequired)
                mainControlTag.Attributes.Add("required", "");

            if (isChecked)
                mainControlTag.Attributes.Add("checked", "");

            return mainControlTag;
        }
    }
}