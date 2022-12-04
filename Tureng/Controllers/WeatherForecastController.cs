using System.Net;
using System.Xml.Linq;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;

namespace Tureng.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "Translate")]
    public string Translate(string word)
    {
        var content = GetSiteHtml($"https://tureng.com/tr/turkce-ingilizce/{word}");

        var doc = new HtmlDocument();
        doc.LoadHtml(content);

        var tableCollection = doc.DocumentNode.SelectNodes("//*[@id=\"englishResultsTable\"]");

        foreach (HtmlNode table in tableCollection)
        {
            Console.WriteLine("Found: " + table.Id);
            foreach (HtmlNode row in table.SelectNodes("tr"))
            {
                Console.WriteLine("row");
                foreach (HtmlNode cell in row.SelectNodes("th|td"))
                {
                    Console.WriteLine("cell: " + cell.InnerText);
                }
            }
        }

        return string.Empty;
    }

    [Obsolete("Obsolete")]
    private static string GetSiteHtml(string url)
    {
        var webClient = new WebClient();
        webClient.Headers.Add("user-agent", "Only a test!");
        var html = webClient.DownloadString(url);

        return html;
    }
}