using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Framework.Application.Presentation.HtmlHelperExtensions
{
    public static class RadioHtmlHelperExtension
    {
        public static IHtmlContent SelectGroupFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, IEnumerable<RadioItem> items)
        {
            ModelExpressionProvider modelExpressionProvider =
                (ModelExpressionProvider)htmlHelper.ViewContext.HttpContext.RequestServices.GetService(typeof(IModelExpressionProvider));

            var name = modelExpressionProvider.GetExpressionText(expression);
            var modelExplorer =
                modelExpressionProvider.CreateModelExpression(htmlHelper.ViewData, expression);

            var metadata = modelExplorer.Metadata;
            var model = modelExplorer.Model;

            return SelectGroup(name, metadata.DisplayName, model?.ToString(), metadata.IsReadOnly, items);
        }

        private static IHtmlContent SelectGroup(
            string name,
            string label,
            string value,
            bool isReadOnly,            
            IEnumerable<RadioItem> items)
        {
            var formGroupDivTag = CommonHtmlHelperExtension.GetFormGroupDivTag();

            if (!string.IsNullOrEmpty(label))
            {
                var labelTag = CommonHtmlHelperExtension.GetLabelTag(name, label, false);
                formGroupDivTag.InnerHtml.AppendHtml(labelTag);
            }

            var selectGroup = new TagBuilder("div");
            selectGroup.AddCssClass("selectgroup w-100 flex-wrap");

            foreach(var item in items)
            {
                var selectGroupItem = new TagBuilder("label");
                selectGroupItem.AddCssClass("selectgroup-item w-30p");
                var radio = BaseRadio(name, item.Value, isReadOnly, false, item.Value.ToLower().Equals(value.ToLower()), "selectgroup-input");
                var selectGroupButton = new TagBuilder("span");
                selectGroupButton.AddCssClass("selectgroup-button");
                selectGroupButton.InnerHtml.AppendHtml(item.Label);
                selectGroupItem.InnerHtml.AppendHtml(radio);
                selectGroupItem.InnerHtml.AppendHtml(selectGroupButton);
                selectGroup.InnerHtml.AppendHtml(selectGroupItem);
            }

            formGroupDivTag.InnerHtml.AppendHtml(selectGroup);

            return formGroupDivTag;
        }


        private static IHtmlContent BaseRadio(
            string name,
            string value,
            bool isReadOnly,
            bool isRequired,
            bool isChecked,
            string customClass)
        {
            var mainControlTag = new TagBuilder("input");
            mainControlTag.Attributes.Add("type", "radio");

            if (!string.IsNullOrEmpty(customClass)) mainControlTag.AddCssClass(customClass);

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
