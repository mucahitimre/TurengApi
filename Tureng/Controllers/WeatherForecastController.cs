using System.Net;
using System.Web;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;

namespace Tureng.Controllers;

[ApiController]
[Route("[controller]")]
public class TurengController : ControllerBase
{
    [HttpGet(Name = "Translate")]
    public List<TurengDictionary> Translate(string word)
    {
        var content = GetSiteHtml($"https://tureng.com/tr/turkce-ingilizce/{word}");
        
        var doc = new HtmlDocument();
        doc.LoadHtml(content);
        var tableCollection = doc.DocumentNode.SelectNodes("//*[@id=\"englishResultsTable\"]");

        return (from table in tableCollection
            from row in table.SelectNodes("tr")
            where row != null && row.ChildNodes.Any() && row.ChildNodes.Count >= 8 && row.ChildNodes[5].ChildNodes.Count >= 4
            let category = row.ChildNodes[5].ChildNodes[0]
            let type = row.ChildNodes[5].ChildNodes[2]
            select new TurengDictionary()
            {
                Category = row.ChildNodes[3].InnerText,
                Source = category.InnerText + " " + type.InnerText.Trim(),
                Translate = HttpUtility.HtmlDecode(row.ChildNodes[7].ChildNodes[0].InnerText)
            }).ToList();
    }

    [Obsolete("Obsolete")]
    private static string GetSiteHtml(string url)
    {
        var webClient = new WebClient();
        webClient.Headers.Add("user-agent", "test");
        var html = webClient.DownloadString(url);

        return html;
    }
}

public class TurengDictionary
{
    public string Category { get; set; }
    public string Source { get; set; }
    public string Translate { get; set; }
}