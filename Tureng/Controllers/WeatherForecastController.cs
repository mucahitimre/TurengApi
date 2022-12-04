using System.Net;
using System.Xml.Linq;
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
        var content = GetSiteHtml($"http://tureng.com/tr/turkce-ingilizce/{word}");

        return string.Empty;
    }

    [Obsolete("Obsolete")]
    private static string GetSiteHtml(string url)
    {
        var wc = new WebClient();
        var html = wc.DownloadString(url);

        return html;
    }
}