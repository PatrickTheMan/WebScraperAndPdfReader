using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using WebScraperProject.WebScraping.Options;

namespace WebScraperProject.WebScraping
{
    public static class WebScraper
    {
        #region Private Variables
        private static readonly HttpClient _httpClient = new();
        #endregion

        #region Public Scrape Methods
        public static HtmlNode? ScrapeHtmlNode(string url, SearchFor searchFor) => CreateHtmlDocument(GetRequest(url)).DocumentNode.SelectSingleNode(searchFor.GetSearchString());
        public static string? SingleNodeScrape(string url, NavigationOptions navigationOptions)
        {
            HtmlDocument doc = CreateHtmlDocument(GetRequest(url));
            return ScrapeForString(doc, navigationOptions);
        }
        public static string[] SingleScrapeLinks(string url, NavigationOptions navigationOptions)
        {
            HtmlDocument doc = CreateHtmlDocument(GetRequest(url));
            return ScrapeForLinks(GetDefaultUrl(url), doc, navigationOptions);
        }
        public static string[] SingleScrape(string url, NavigationOptions navigationOptions)
        {
            HtmlDocument doc = CreateHtmlDocument(GetRequest(url));
            return ScrapeForStrings(doc, navigationOptions);
        }
        public static string[] MultiScrape(string url, NavigationOptions[] navigationNodeOptions, NavigationOptions navigationLinkOptions)
        {
            List<string> urlVisited = [url];
            Stack<string> urlStack = new([url]);
            List<string> resultList = [];

            while (urlStack.Count != 0)
            {
                HtmlDocument doc = CreateHtmlDocument(GetRequest(urlStack.Pop()));
                if (doc == null) continue;

                foreach (var nNO in navigationNodeOptions)
                {
                    resultList.AddRange(ScrapeForStrings(doc, nNO));
                }

                foreach (string urlString in ScrapeForLinks(GetDefaultUrl(url), doc, navigationLinkOptions)) 
                {
                    if (!urlVisited.Contains(urlString))
                    {
                        urlStack.Push(urlString);
                        urlVisited.Add(urlString);
                    }
                }
            } 

            return [.. resultList];
        }
        #endregion

        #region Private Setup Methods
        private static string GetDefaultUrl(string url)
        {
            string[] urlParts = url.Split(".");
            return $"{urlParts[0]}.{urlParts[1]}.{urlParts[2].Split('/')[0]}"; // ex. https://www.namehere.com
        }
        private static string GetRequest(string url)
        {
            return _httpClient.GetStringAsync(url).Result;
        }
        private static HtmlDocument CreateHtmlDocument(string html)
        {
            HtmlDocument htmlDocument = new();
            htmlDocument.LoadHtml(html);
            return htmlDocument;
        }
        #endregion

        #region Document Link Scrape Methods
        private static string[] ScrapeForLinks(string defaultUrl, HtmlDocument document, NavigationOptions navigationOptions)
        {
            List<string> strings = [];

            HtmlNode? node = navigationOptions.Navigate(document);
            if (node == null) return [];

            strings.AddRange(GetHRefs(defaultUrl, node, []));

            return [.. strings];
        }
        private static List<string> GetHRefs(string defaultUrl, HtmlNode node, List<string> strings)
        {
            if (!node.GetAttributeValue("href", string.Empty).Equals(string.Empty) && !strings.Contains(defaultUrl + node.GetAttributeValue("href", string.Empty).Trim()))
                strings.Add(defaultUrl + node.GetAttributeValue("href", string.Empty).Trim());

            foreach (var childNode in node.ChildNodes)
            {
                GetHRefs(defaultUrl, childNode, strings);
            }

            return [.. strings];
        }
        #endregion

        #region Document Text Scrape Methods
        public static string? ScrapeForString(HtmlDocument document, NavigationOptions navigationOptions)
        {
            HtmlNode? node = navigationOptions.Navigate(document);
            if (node == null) return null;
            return node.InnerText.Trim();
        }
        private static string[] ScrapeForStrings(HtmlDocument document, NavigationOptions navigationOptions)
        {
            List<string> strings = [];

            HtmlNode? node = navigationOptions.Navigate(document);
            if (node == null) return [];

            strings.AddRange(GetInnerTexts(node, []));

            return [.. strings];
        }
        private static List<string> GetInnerTexts(HtmlNode node, List<string> strings)
        {
            if (node.InnerText != null && !strings.Contains(node.InnerText.Trim()))
                strings.Add(node.InnerText.Trim());

            foreach (var childNode in node.ChildNodes)
            {
                GetInnerTexts(childNode, strings);
            }

            return [.. strings];
        }
        #endregion
    }
}
