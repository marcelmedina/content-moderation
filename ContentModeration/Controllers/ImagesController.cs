﻿using System;
using System.Text;
using System.Threading.Tasks;
using ContentModeration.Models;
using ContentModeration.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ContentModeration.Controllers
{
    [Route("api/[controller]")]
    public class ImagesController : Controller
    {
        private readonly IContentModerationService _service;

        public ImagesController(IContentModerationService service)
        {
            _service = service;
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
        [HttpPut]
        public async Task<IActionResult> Submit([FromBody]Data data)
        {
            try
            {
                // Request body
                byte[] byteData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));

                return await _service.ProcessImage(byteData, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        /// <summary>
        /// Post a binary image for content moderation
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
                return await _service.ProcessImage(byteData, contentType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}