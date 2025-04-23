using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraperProject.WebScraping.Options
{
    public class SearchFor
    {
        public required string NodeType { private get; set; } // ex. "div", "span" and so on
        public required KeyValuePair<string, string> SearchTerm { private get; set; } // ex. <"class", "cool-class-name">

        public string GetSearchString() // ex. //div[@class='cool-class-name']"
        {
            return $"//{NodeType}[@{SearchTerm.Key}='{SearchTerm.Value}']";
        }
    }
}
