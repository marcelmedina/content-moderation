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
        private readonly string _contentModerationUrl = "https://australiaeast.api.cognitive.microsoft.com/contentmoderator/moderate/v1.0/ProcessImage";

        public ContentModerationService(IOptions<Settings> settings)
        {
            _settings = settings;
        }

        public async Task<EvaluateResponse> ProcessImageEvaluate(byte[] byteData, string contentType)
        {
            string jsonResponse;

            using (var client = new HttpClient())
            {
                // Request headers
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _settings.Value.OcpApimSubscriptionKey);

                using (var content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                    var responseMessage = await client.PostAsync($"{_contentModerationUrl}/Evaluate", content);
                    jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                }
            }

            return JsonConvert.DeserializeObject<EvaluateResponse>(jsonResponse);
        }

        public async Task<OcrResponse> ProcessImageOcr(byte[] byteData, string contentType, bool enhanced = false)
        {
            string jsonResponse;

            using (var client = new HttpClient())
            {
                // Request headers
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _settings.Value.OcpApimSubscriptionKey);

                using (var content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                    var responseMessage = await client.PostAsync($"{_contentModerationUrl}/OCR?language=eng&enhanced={enhanced}", content);
                    jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                }
            }

            return JsonConvert.DeserializeObject<OcrResponse>(jsonResponse);
        }
    }
}
