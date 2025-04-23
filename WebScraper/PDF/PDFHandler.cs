using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;
using UglyToad.PdfPig;
using WebScraperProject.Filehandling;
using WebScraperProject.Logging;

namespace WebScraperProject.PDF
{
    class PDFHandler
    {
        #region Private Variables
        private static readonly HttpClient _httpClient = new();
        #endregion

        #region Read PDF
        public static string[] ReadPdf(string pdfName)
        {
            List<string> pageStrings = [];

            string? path = AssetManager.GetAssetPath(pdfName);
            if (path == null) return [];

            using var pdf = PdfDocument.Open(path);
            foreach (var page in pdf.GetPages())
            {
                var text = string.Join(" ", page.GetWords());
                pageStrings.Add(text);
            }

            return [.. pageStrings];
        }
        #endregion

        #region Download PDF
        public static string DownloadPDF(string url, string pdfName)
        {
            using Stream stream = _httpClient.GetStreamAsync(url).Result;
            return AssetManager.SaveAsset(pdfName, AssetType.PDF, stream);
        }
        #endregion
    }
}
