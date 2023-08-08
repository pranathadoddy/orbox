using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Framework.Application.Presentation.HtmlHelperExtensions
{
    public static class TextEditorHelperExtension
    {
        #region Public Methods

        public static IHtmlContent TextEditorFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, string customClasses = "")
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
                model?.ToString(),
                metadata.IsReadOnly,
                metadata.IsRequired,
                PresentationConstant.TextInputType.Text,
                PresentationConstant.TextInputAttribute.ShortTextMaxLength,
                customClasses: customClasses);
        }

        public static IHtmlContent TextEditorDatePickerFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, string customClasses = "")
        {
            ModelExpressionProvider modelExpressionProvider =
                (ModelExpressionProvider)htmlHelper.ViewContext.HttpContext.RequestServices.GetService(typeof(IModelExpressionProvider));

            var name = modelExpressionProvider.GetExpressionText(expression);
            var modelExplorer =
                modelExpressionProvider.CreateModelExpression(htmlHelper.ViewData, expression);

            var metadata = modelExplorer.Metadata;
            var model = (DateTime?)modelExplorer.Model ?? DateTime.MinValue;

            return SimpleFormGroupDatePickerDivTag(name,
                metadata.DisplayName,
                metadata.Description,
                model,
                metadata.IsReadOnly,
                metadata.IsRequired,
                PresentationConstant.TextInputType.Text,
                PresentationConstant.TextInputAttribute.ShortTextMaxLength,
                customClasses: customClasses);
        }

        public static IHtmlContent TextEditorInputGroupFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            string textToAppendOnValue,
            string customClasses = "")
        {
            ModelExpressionProvider modelExpressionProvider =
                (ModelExpressionProvider)htmlHelper.ViewContext.HttpContext.RequestServices.GetService(typeof(IModelExpressionProvider));

            var name = modelExpressionProvider.GetExpressionText(expression);
            var modelExplorer =
                modelExpressionProvider.CreateModelExpression(htmlHelper.ViewData, expression);

            var metadata = modelExplorer.Metadata;
            var model = modelExplorer.Model;

            return FormGroupWithInputGroupDivTag(name,
                metadata.DisplayName,
                metadata.Description,
                model?.ToString(),
                metadata.IsReadOnly,
                metadata.IsRequired,
                PresentationConstant.TextInputType.Text,
                PresentationConstant.TextInputAttribute.ShortTextMaxLength,
                textToAppendOnValue,
                customClasses: customClasses);
        }

        public static IHtmlContent PasswordEditorFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression)
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
                model?.ToString(),
                metadata.IsReadOnly,
                metadata.IsRequired,
                PresentationConstant.TextInputType.Password,
                PresentationConstant.TextInputAttribute.ShortTextMaxLength);
        }

        public static IHtmlContent EmailEditorFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression)
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
                model?.ToString(),
                metadata.IsReadOnly,
                metadata.IsRequired,
                PresentationConstant.TextInputType.Email,
                PresentationConstant.TextInputAttribute.ShortTextMaxLength);
        }

        public static IHtmlContent NumberEditorFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, string customClasses = "", int maxLength = PresentationConstant.TextInputAttribute.MiniTextMaxLength)
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
                model?.ToString(),
                metadata.IsReadOnly,
                metadata.IsRequired,
                PresentationConstant.TextInputType.Number,
                maxLength,
                customClasses: customClasses);
        }

        public static IHtmlContent MultilineTextEditorFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, string customClasses = "", int maxLength = PresentationConstant.TextInputAttribute.MediumTextMaxLength)
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
                model?.ToString(),
                metadata.IsReadOnly,
                metadata.IsRequired,
                string.Empty,
                maxLength,
                true,
                customClasses: customClasses);
        }

        public static IHtmlContent BaseTextEditorFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            string type = PresentationConstant.TextInputType.Text,
            int length = PresentationConstant.TextInputAttribute.ShortTextMaxLength)
        {
            ModelExpressionProvider modelExpressionProvider =
                (ModelExpressionProvider)htmlHelper.ViewContext.HttpContext.RequestServices.GetService(typeof(IModelExpressionProvider));

            var name = modelExpressionProvider.GetExpressionText(expression);
            var modelExplorer =
                modelExpressionProvider.CreateModelExpression(htmlHelper.ViewData, expression);

            var metadata = modelExplorer.Metadata;
            var model = modelExplorer.Model;

            return BaseTextEditor(name,
                metadata.Description,
                model?.ToString(),
                metadata.IsReadOnly,
                metadata.IsRequired,
                type,
                length);
        }

        #endregion

        #region Private Methods

        private static IHtmlContent SimpleFormGroupDivTag(
            string name,
            string label,
            string placeholder,
            string value,
            bool isReadOnly,
            bool isRequired,
            string type,
            int maxLength,
            bool isMultiline = false,
            string customClasses = "")
        {
            var formGroupDivTag = CommonHtmlHelperExtension.GetFormGroupDivTag(customClasses);

            if (!string.IsNullOrEmpty(label))
            {
                var labelTag = CommonHtmlHelperExtension.GetLabelTag(name, label, isRequired);
                formGroupDivTag.InnerHtml.AppendHtml(labelTag);
            }

            var textEditorTag = BaseTextEditor(name,
                placeholder,
                value,
                isReadOnly,
                isRequired,
                type,
                maxLength,
                isMultiline);

            formGroupDivTag.InnerHtml.AppendHtml(textEditorTag);

            return formGroupDivTag;
        }

        private static IHtmlContent SimpleFormGroupDatePickerDivTag(
            string name,
            string label,
            string placeholder,
            DateTime value,
            bool isReadOnly,
            bool isRequired,
            string type,
            int maxLength,
            string customClasses = "")
        {
            var formGroupDivTag = CommonHtmlHelperExtension.GetFormGroupDivTag(customClasses);

            if (!string.IsNullOrEmpty(label))
            {
                var labelTag = CommonHtmlHelperExtension.GetLabelTag(name, label, isRequired);
                formGroupDivTag.InnerHtml.AppendHtml(labelTag);
            }

            var textEditorTag = DatePickerTextEditor(name,
                placeholder,
                value,
                isReadOnly,
                isRequired,
                type,
                maxLength);

            formGroupDivTag.InnerHtml.AppendHtml(textEditorTag);

            return formGroupDivTag;
        }


        private static IHtmlContent FormGroupWithInputGroupDivTag(
            string name,
            string label,
            string placeholder,
            string value,
            bool isReadOnly,
            bool isRequired,
            string type,
            int maxLength,
            string textToAppendOnValue,
            string customClasses = "")
        {
            var formGroupDivTag = CommonHtmlHelperExtension.GetFormGroupDivTag(customClasses);

            if (!string.IsNullOrEmpty(label))
            {
                var labelTag = CommonHtmlHelperExtension.GetLabelTag(name, label, isRequired);
                formGroupDivTag.InnerHtml.AppendHtml(labelTag);
            }

            var inputGroup = new TagBuilder("div");
            inputGroup.AddCssClass("input-group");

            var textEditorTag = BaseTextEditor(name,
                placeholder,
                value,
                isReadOnly,
                isRequired,
                type,
                maxLength);

            inputGroup.InnerHtml.AppendHtml(textEditorTag);

            var inputGroupAppend = new TagBuilder("span");
            inputGroupAppend.AddCssClass("input-group-append");

            var inputGroupText = new TagBuilder("span");
            inputGroupText.AddCssClass("input-group-text");

            inputGroupText.InnerHtml.AppendHtml(textToAppendOnValue);
            inputGroupAppend.InnerHtml.AppendHtml(inputGroupText);

            inputGroup.InnerHtml.AppendHtml(inputGroupAppend);

            formGroupDivTag.InnerHtml.AppendHtml(inputGroup);

            return formGroupDivTag;
        }

        private static IHtmlContent DatePickerTextEditor(
            string name,
            string placeholder,
            DateTime value,
            bool isReadOnly,
            bool isRequired,
            string type,
            int maxLength)
        {
            var mainControlTag = new TagBuilder("input");

            mainControlTag.Attributes.Add("type", type);
            if (value != DateTime.MinValue)
                mainControlTag.Attributes.Add("value", value.ToString("yyyy-MM-dd"));

            mainControlTag.AddCssClass("form-control");
            mainControlTag.Attributes.Add("id", name);
            mainControlTag.Attributes.Add("name", name);
            mainControlTag.Attributes.Add("maxlength", maxLength.ToString());
            mainControlTag.Attributes.Add("readonly", "readonly");

            if (!string.IsNullOrEmpty(placeholder))
                mainControlTag.Attributes.Add("placeholder", placeholder);

            if (isReadOnly)
                mainControlTag.Attributes.Add("readonly", "readonly");

            if (isRequired)
                mainControlTag.Attributes.Add("required", "");

            return mainControlTag;
        }

        private static IHtmlContent BaseTextEditor(
            string name,
            string placeholder,
            string value,
            bool isReadOnly,
            bool isRequired,
            string type,
            int maxLength,
            bool isMultiline = false)
        {
            var mainControlTag = isMultiline ? new TagBuilder("textarea") : new TagBuilder("input");
            if (!isMultiline)
            {
                mainControlTag.Attributes.Add("type", type);
                if (!string.IsNullOrEmpty(value))
                    mainControlTag.Attributes.Add("value", value);
            }
            else
            {
                mainControlTag.Attributes.Add("rows", PresentationConstant.TextInputAttribute.MaxTextareaRows.ToString());
                mainControlTag.InnerHtml.AppendHtml(value);
            }

            mainControlTag.AddCssClass("form-control");
            mainControlTag.Attributes.Add("id", name);
            mainControlTag.Attributes.Add("name", name);
            mainControlTag.Attributes.Add("maxlength", maxLength.ToString());

            if (!string.IsNullOrEmpty(placeholder))
                mainControlTag.Attributes.Add("placeholder", placeholder);

            if (isReadOnly)
                mainControlTag.Attributes.Add("readonly", "readonly");

            if (isRequired)
                mainControlTag.Attributes.Add("required", "");

            return mainControlTag;
        }

        #endregion
    }
}