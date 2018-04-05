using System.IO;
using System.Text;
using System.Threading.Tasks;
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
            _settings.Value.OcpApimSubscriptionKey = "your_contentmoderationapi_key";

            _data = new Data()
            {
                DataRepresentation = "URL",
                Value = "https://moderatorsampleimages.blob.core.windows.net/samples/sample.jpg"
            };

            _service = new ContentModerationService(_settings);
        }

        [Test]
        public async Task CallEvaluate_JsonMethod_HappyPath()
        {
            await Task.Delay(1000);

            byte[] byteData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(_data));
            
            var result = await _service.ProcessImageEvaluate(byteData, "application/json");

            Assert.IsTrue(result.GetType() == typeof(EvaluateResponse));
        }

        [TestCase("ball.bmp", "image/bmp")]
        [TestCase("ball.jpg", "image/jpeg")]
        [TestCase("ball.gif", "image/gif")]
        [TestCase("ball.png", "image/png")]
        public async Task CallEvaluate_BinaryMethod_HappyPath(string file, string contentType)
        {
            await Task.Delay(1000);

            byte[] imgdata = File.ReadAllBytes(Path.GetFullPath($@"Images\Evaluate\{file}"));

            var result = await _service.ProcessImageEvaluate(imgdata, contentType);

            Assert.IsTrue(result.GetType() == typeof(EvaluateResponse));
        }

        [Test]
        public async Task CallOcr_JsonMethod_HappyPath()
        {
            await Task.Delay(1000);

            byte[] byteData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(_data));

            var result = await _service.ProcessImageOcr(byteData, "application/json", false);

            Assert.IsTrue(result.GetType() == typeof(OcrResponse));
        }

        [Test]
        public async Task CallOcr_JsonMethod_OcrMessage_HappyPath()
        {
            await Task.Delay(1000);

            byte[] byteData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(_data));

            var result = await _service.ProcessImageOcr(byteData, "application/json", false);

            Assert.IsTrue(result.GetType() == typeof(OcrResponse));
        }

        [Test]
        public async Task CallOcr_JsonMethod_OcrMessageEnhanced_HappyPath()
        {
            await Task.Delay(1000);

            byte[] byteData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(_data));

            var result = await _service.ProcessImageOcr(byteData, "application/json", true);

            Assert.IsTrue(result.GetType() == typeof(OcrResponse));
        }

        [TestCase("football.bmp", "image/bmp", false)]
        [TestCase("football.jpg", "image/jpeg", false)]
        [TestCase("football.gif", "image/gif", false)]
        [TestCase("football.png", "image/png", false)]
        public async Task CallOcr_BinaryMethod_HappyPath(string file, string contentType, bool enhanced)
        {
            await Task.Delay(1000);

            var text = "KEEP CALM AND PLAY FOOTBALL";

            byte[] imgdata = File.ReadAllBytes(Path.GetFullPath($@"Images\Ocr\{file}"));

            var result = await _service.ProcessImageOcr(imgdata, contentType);

            Assert.IsTrue(result.GetType() == typeof(OcrResponse));
            Assert.AreEqual(result.Text.Replace("\r\n","").Trim(), text);
        }
    }
}