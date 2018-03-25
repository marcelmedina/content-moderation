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
        /// Post an image for content moderation
        /// </summary>
        /// <remarks>
        ///     *only for application/json*
        ///     POST /Images
        ///     {
        ///         "DataRepresentation":"URL",
        ///         "Value":"https://moderatorsampleimages.blob.core.windows.net/samples/sample.jpg"
        ///     }
        /// </remarks>
        /// <param name="data"></param>
        /// <returns></returns>
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Data data)
        {
            // <param name="contentType">Acceptable as application/json, image/gif, image/jpeg, image/png, image/bmp, image/tiff</param>
            try
            {
                // Request body
                byte[] byteData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));

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
                    : StatusCode((int) responseMessage.StatusCode, jsonResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}