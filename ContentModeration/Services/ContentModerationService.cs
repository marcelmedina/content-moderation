using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ContentModeration.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ContentModeration.Services
{
    public class ContentModerationService : IContentModerationService
    {
        private readonly IOptions<Settings> _settings;
        private readonly string _contentModerationUrl = "https://australiaeast.api.cognitive.microsoft.com/contentmoderator/moderate/v1.0/ProcessImage/Evaluate";

        public ContentModerationService(IOptions<Settings> settings)
        {
            _settings = settings;
        }

        public async Task<IActionResult> ProcessImage(byte[] byteData, string contentType)
        {
            HttpResponseMessage responseMessage;
            string jsonResponse;

            using (var client = new HttpClient())
            {
                // Request headers
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _settings.Value.OcpApimSubscriptionKey);

                using (var content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                    responseMessage = await client.PostAsync(_contentModerationUrl, content);
                    jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                }
            }

            return responseMessage.StatusCode == HttpStatusCode.OK
                ? new OkObjectResult(JsonConvert.DeserializeObject<Response>(jsonResponse))
                : new ObjectResult(jsonResponse) { StatusCode = (int) responseMessage.StatusCode };
        }
    }
}
