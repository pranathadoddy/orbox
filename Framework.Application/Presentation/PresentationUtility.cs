using Framework.Core.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Framework.Application.Presentation
{
    public class PresentationUtility
    {
        public static SelectList YesNoOptionSelectList(bool defaultNo = false)
        {
            var selectListItems = new List<SelectListItem>() {
                new SelectListItem() { Value = PresentationConstant.ListItemValue.True, Text = GeneralResource.General_Yes},
                new SelectListItem() { Value = PresentationConstant.ListItemValue.False, Text = GeneralResource.General_No, Selected = defaultNo},
            };

            return new SelectList(selectListItems, "Value", "Text");
        }
    }
}
