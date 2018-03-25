using System.IO;
using System.Text;
using System.Threading.Tasks;
using ContentModeration.Controllers;
using ContentModeration.Models;
using ContentModeration.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NUnit.Framework;

namespace ContentModeration.Tests.Images
{
    [TestFixture]
    public class ImagesController_Tests
    {
        private IOptions<Settings> _settings;
        private Data _data;
        private ContentModerationService _service;

        [SetUp]
        public void Setup()
        {
            _settings = Options.Create<Settings>(new Settings());
            _settings.Value.OcpApimSubscriptionKey = "<your_contentmoderationapi_key>";

            _data = new Data()
            {
                DataRepresentation = "URL",
                Value = "https://moderatorsampleimages.blob.core.windows.net/samples/sample.jpg"
            };

            _service = new ContentModerationService(_settings);
        }

        [Test]
        public async Task CallJsonMethod_HappyPath()
        {
            await Task.Delay(1000);

            byte[] byteData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(_data));
            
            var result = await _service.ProcessImage(byteData, "application/json");

            Assert.AreEqual(200, ((Microsoft.AspNetCore.Mvc.ObjectResult)result).StatusCode);
            Assert.IsTrue(((Microsoft.AspNetCore.Mvc.ObjectResult)result).Value.GetType() == typeof(Response));
        }

        [TestCase("ball.bmp", "image/bmp")]
        [TestCase("ball.jpg", "image/jpeg")]
        [TestCase("ball.gif", "image/gif")]
        [TestCase("ball.png", "image/png")]
        public async Task CallBinaryMethod_HappyPath(string file, string contentType)
        {
            await Task.Delay(1000);

            byte[] imgdata = System.IO.File.ReadAllBytes(Path.GetFullPath($@"Images\{file}"));

            var result = await _service.ProcessImage(imgdata, contentType);

            Assert.AreEqual(200, ((Microsoft.AspNetCore.Mvc.ObjectResult)result).StatusCode);
            Assert.IsTrue(((Microsoft.AspNetCore.Mvc.ObjectResult)result).Value.GetType() == typeof(Response));
        }
    }
}