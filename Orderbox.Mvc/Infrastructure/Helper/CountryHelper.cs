using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Orderbox.Mvc.Infrastructure.Helper
{
    public static class CountryHelper
    {
        private static readonly string[] excludedCountries = { "001", "029", "049" , "DO"};

        public static SelectList GenerateCountrySelecList(string selectedCountryCode)
        {
            var countries = new List<RegionInfo>();
            foreach (var culture in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                var country = new RegionInfo(culture.Name);
                if (!excludedCountries.Contains(country.Name) && 
                    countries.Where(p => p.Name == country.Name).Count() == 0)
                    countries.Add(country);
            }

            var selectList = new SelectList(countries.OrderBy(item => item.DisplayName), "Name", "DisplayName");

            selectedCountryCode = string.IsNullOrEmpty(selectedCountryCode) ? "ID" : selectedCountryCode;

            var selectedCountry = selectList.Where(item => item.Value == selectedCountryCode).First();
            selectedCountry.Selected = true;

            return selectList;
        }
    }
}
