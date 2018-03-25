using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ContentModeration.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ContentModeration.Controllers
{
    [Route("api/[controller]")]
    public class ImagesController : Controller
    {
        private readonly IOptions<Settings> _settings;
        private readonly string _contentModerationUrl ="https://australiaeast.api.cognitive.microsoft.com/contentmoderator/moderate/v1.0/ProcessImage/Evaluate";

        public ImagesController(IOptions<Settings> settings)
        {
            _settings = settings;
        }

        /// <summary>
        /// Post a json image for content moderation
        /// </summary>
        /// <remarks>
        ///     GET /Images
        ///     {
        ///         "DataRepresentation":"URL",
        ///         "Value":"https://moderatorsampleimages.blob.core.windows.net/samples/sample.jpg"
        ///     }
        /// </remarks>
        /// <param name="data">Json data</param>
        /// <returns></returns>
        [Produces("application/json")]
        [HttpGet]
        public async Task<IActionResult> Get([FromBody]Data data)
        {
            try
            {
                // Request body
                byte[] byteData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));

                return await ProcessImage(byteData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        /// <summary>
        /// Post a binary image for content moderation
        /// </summary>
        /// <param name="contentType">Acceptable as application/json, image/gif, image/jpeg, image/png, image/bmp, image/tiff</param>
        /// <param name="byteData"></param>
        /// <returns></returns>
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> Post(string contentType, byte[] byteData)
        {
            try
            {
                return await ProcessImage(byteData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        private async Task<IActionResult> ProcessImage(byte[] byteData)
        {
            HttpResponseMessage responseMessage;
            string jsonResponse;

            using (var client = new HttpClient())
            {
                // Request headers
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _settings.Value.OcpApimSubscriptionKey);

                using (var content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    responseMessage = await client.PostAsync(_contentModerationUrl, content);
                    jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                }
            }

            return responseMessage.StatusCode == HttpStatusCode.OK
                ? Ok(JsonConvert.DeserializeObject<Response>(jsonResponse))
                : StatusCode((int)responseMessage.StatusCode, jsonResponse);
        }
    }
}