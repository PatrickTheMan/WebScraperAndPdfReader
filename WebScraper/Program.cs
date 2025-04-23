using Microsoft.VisualBasic;
using WebScraperProject.Logging;
using WebScraperProject.PDF;
using WebScraperProject.WebScraping;
using WebScraperProject.WebScraping.Options;

#region Start
Console.WriteLine("Program Start");
#endregion

#region WebScraper
/* This will target the same node as above
NavigationOptions nodeOptions = new()
{
    SearchFor = new()
    {
        NodeType = "h3",
        SearchTerm = new("class", "rte")
    }
};
*/
NavigationOptions nodeOptions = new()
{
    SearchFor = new()
    {
        NodeType = "h3",
        SearchTerm = new("class", "rte")
    },
    Navigation = // OPTIONAL: This can be used to go up and down the hierarki manually, however it is not mandatory (See above a "nodeOptions", that have same effect without Navigation) 
    [
        new(NodeNavigation.Parent, null),
        new(NodeNavigation.Child, 
            new(){
                NodeType = "h3",
                SearchTerm = new("class", "rte")
            }
        ),
    ]
};
NavigationOptions linkOptions = new()
{
    SearchFor = new()
    {
        NodeType = "div",
        SearchTerm = new("class", "tile-group-component product-group")
    }
};
string url_WebScraper = "https://www.danfoss.com/en/products/dcs/compressors/turbocor/turbocor-tg/";

// - SingleScrape: Gives you the innertext for the desired node and all its children (Use SingleNodeScrape() to only get inner text of the desired node)
//string[] singleScrape = WebScraper.SingleScrape(url_WebScraper, nodeOptions);
//foreach (var s in singleScrape) { Logger.Log(s); }

// - SingleScrapeLinks: Gives you all the href for the desired node and all its children
//string[] singleScrapeLinks = WebScraper.SingleScrapeLinks(url_WebScraper, linkOptions);
//foreach (var s in singleScrapeLinks) { Logger.Log(s); }

// - MultiScrape: Gives you the innertext for the desired node(s) and all its/their childrens, then looks for links desired and does the same on each linked page
//string[] strings = WebScraper.MultiScrape(url_WebScraper, [nodeOptions], linkOptions);
//foreach (var s in multiScrape) { Logger.Log(s); }
#endregion

#region PDFHandler
string url_PDF = "https://assets.danfoss.com/documents/368997/AD341159618036en-000102.pdf";
string pdfName = "AD341159618036en-000102";

/*
// - Downloads the .pdf from the given url and then reads text from it
string path = PDFHandler.DownloadPDF(url_PDF, pdfName);
Logger.Log(path);

string[] pagesStrings = PDFHandler.ReadPdf(pdfName);
//foreach (var s in pagesStrings) { Logger.Log(s); }
*/
#endregion

#region End
Console.WriteLine("Program End");
#endregion