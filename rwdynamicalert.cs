using System;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using HtmlAgilityPack;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Extensions.Http;


public static class checkline
{
    [FunctionName("CheckLine")]
        public static IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
        ILogger log)
    {
        string url = "https://www.simplifiedlogistics.com/heartbeattest/heartBeatReport.aspx";  // URL de la página web que deseas revisar
        string textoObjetivo = "HeartBeat Status Good";

        using (var client = new WebClient())
        {
            try
            {
                string htmlContent = client.DownloadString(url);
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(htmlContent);
                string content = htmlDocument.DocumentNode.InnerText;

                if (content.Contains(textoObjetivo))
                {
                    return new OkObjectResult($"The Text '{textoObjetivo}' Was found.");
                }
                else
                {
                    return new OkObjectResult($"The text '{textoObjetivo}' was not found.");
                }
            }
            catch (WebException ex)
            {
                log.LogError($"We cannot access to the page message: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }
    }
}
