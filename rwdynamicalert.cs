using System;
using System.Net.Http;
using HtmlAgilityPack;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace rw
{
    public static class rwdynamicalert
    {
        [FunctionName("rwdynamicalert")]
        public static async Task Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            string aspxPageUrl = "https://www.simplifiedlogistics.com/heartbeattest/heartBeatReport.aspx";
            
            using (HttpClient client = new HttpClient())
            {
                string PageHtml =await client.GetStringAsync(aspxPageUrl);

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(PageHtml);

                 HtmlNode spanElemnt = doc.DocumentNode.SelectSingleNode("//span[contains(@class, HeartBeat Status Good)]");

                 if (spanElemnt != null)
                 {
                    string spanContent = spanElemnt.InnerText;
                    log.LogInformation($"Span Content: {spanContent}");

                 }
                 else
                 {
                    log.LogInformation("The span was not found on the ASPX page.");
                 }
            }
        }
    }
}
