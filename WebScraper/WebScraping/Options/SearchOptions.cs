using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraperProject.WebScraping.Options
{
    public enum NodeNavigation
    {
        Parent,
        Child
    }
    public class NavigationOptions
    {
        public KeyValuePair<NodeNavigation, SearchFor?>[]? Navigation { private get; set; } // Manually go up or down the html hierarki (Sometimes there isn't much to search for on the right node but it might have a child or parent that is)
        public required SearchFor SearchFor { get; set; } // Search criteria for the first found node
        public HtmlNode? Navigate(HtmlDocument doc) // Navigates from the found node to the desired node (Sometimes there isn't much to search for on the right node but it might have a child or parent that is)
        {
            HtmlNode? node = doc.DocumentNode.SelectSingleNode(SearchFor.GetSearchString());
            if (node == null) return null;

            if (Navigation == null) return node;

            foreach (var nav in Navigation)
            {
                if (nav.Key == NodeNavigation.Parent)
                    node = node.ParentNode;
                else
                {
                    if (nav.Value == null)
                        throw new Exception("SearchFor cannot be null, when searching for a ChildNode");
                    HtmlNode? n = node.SelectSingleNode(nav.Value.GetSearchString());
                    if (n == null) return node;
                    node = n;
                }
            }
            return node;
        }
    }
}
