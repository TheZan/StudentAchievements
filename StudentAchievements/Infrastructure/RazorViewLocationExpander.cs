using Microsoft.AspNetCore.Mvc.Razor;
using System.Collections.Generic;
using System.Linq;

namespace StudentAchievements.Infrastructure
{
    public class RazorViewLocationExpander : IViewLocationExpander
    {
        public void PopulateValues(ViewLocationExpanderContext context) { }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            List<string> locations = viewLocations.ToList();

            locations.Add("/Areas/Authorization/Views/Account/{0}.cshtml");

            return locations;
        }
    }
}