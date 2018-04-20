using System;
using System.Text;
using System.Threading.Tasks;
using ContentModeration.Models;
using ContentModeration.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ContentModeration.Controllers.Images
{
    [Route("api/images/[controller]")]
    public class EvaluateController : Controller
    {
        private readonly IContentModerationService _service;

        public EvaluateController(IContentModerationService service)
        {
            _service = service;
        }

        /// <summary>
        /// PUT a json image for content moderation
        /// </summary>
        /// <remarks>
        ///     PUT /Images
        ///     {
        ///         "DataRepresentation":"URL",
        ///         "Value":"https://moderatorsampleimages.blob.core.windows.net/samples/sample.jpg"
        ///     }
        /// </remarks>
        /// <param name="data">Json data</param>
        /// <returns></returns>
        [Produces("application/json")]
        [HttpPut]
        public async Task<IActionResult> Submit([FromBody]Data data)
        {
            try
            {
                // Request body
                byte[] byteData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));

                return Ok(await _service.ProcessImageEvaluate(byteData, "application/json"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        /// <summary>
        /// POST a binary image for content moderation
        /// </summary>
        /// <param name="contentType">Acceptable as image/gif, image/jpeg, image/png, image/bmp</param>
        /// <param name="byteData"></param>
        /// <returns></returns>
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> Post(string contentType, byte[] byteData)
        {
            try
            {
                return Ok(await _service.ProcessImageEvaluate(byteData, contentType));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}