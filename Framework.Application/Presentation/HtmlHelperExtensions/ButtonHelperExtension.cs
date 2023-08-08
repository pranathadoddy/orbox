using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Framework.Application.Presentation.HtmlHelperExtensions
{
    public static class ButtonHelperExtension
    {
        public static IHtmlContent LinkButtonWithLabel(this IHtmlHelper htmlHelper, string text, string id = "",
            string cssClass = "", string href="javascript:void(0)")
        {
            var tag = new TagBuilder("a");
            tag.AddCssClass("btn");
            tag.Attributes.Add("href", href);

            if (!string.IsNullOrEmpty(id)) tag.Attributes.Add("id", id);

            if (!string.IsNullOrEmpty(cssClass)) tag.AddCssClass(cssClass);

            tag.InnerHtml.AppendHtml(text);

            return tag;
        }

        public static IHtmlContent ButtonWithLabel(this IHtmlHelper htmlHelper, string text,
            string buttonType = PresentationConstant.ButtonType.Button, string id = "",
            string cssClass = "")
        {
            var tag = new TagBuilder("button");
            tag.AddCssClass("btn");
            tag.Attributes.Add("type", buttonType);

            if (!string.IsNullOrEmpty(id)) tag.Attributes.Add("id", id);

            if (!string.IsNullOrEmpty(cssClass)) tag.AddCssClass(cssClass);

            tag.InnerHtml.AppendHtml(text);

            return tag;
        }

        public static IHtmlContent FormFooterButton(this IHtmlHelper htmlHelper, string text,
            string buttonType = PresentationConstant.ButtonType.Button, string id = "",
            string cssClass = "")
        {
            var tag = new TagBuilder("div");
            tag.AddCssClass("form-footer");

            var button = ButtonWithLabel(htmlHelper, text, buttonType, id, cssClass);
            tag.InnerHtml.AppendHtml(button);

            return tag;
        }

        public static IHtmlContent MaintenanceButtonRowWithLabel<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression)
        {
            ModelExpressionProvider modelExpressionProvider = 
                (ModelExpressionProvider)htmlHelper.ViewContext.HttpContext.RequestServices.GetService(typeof(IModelExpressionProvider));

            var modelExplorer =
                modelExpressionProvider.CreateModelExpression(htmlHelper.ViewData, expression);

            var model = modelExplorer.Model;

            return MaintenanceButtonRowWithLabel(model as MaintenanceButtonRow);
        }

        private static IHtmlContent MaintenanceButtonRowWithLabel(MaintenanceButtonRow maintenanceButtonRow)
        {
            var formGroupDivTag = CommonHtmlHelperExtension.GetFormGroupDivTag();
            formGroupDivTag.AddCssClass("maintenance-button-row");

            foreach (var genericButton in maintenanceButtonRow.Items)
            {
                var buttonTag = new TagBuilder("button");
                buttonTag.Attributes.Add("type", genericButton.Type);
                buttonTag.AddCssClass(genericButton.CssClass);
                buttonTag.Attributes.Add("id", genericButton.Id);
                buttonTag.InnerHtml.AppendHtml(genericButton.Label);

                formGroupDivTag.InnerHtml.AppendHtml(buttonTag + " ");
            }

            return formGroupDivTag;
        }
    }
}