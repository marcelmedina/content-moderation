using System.IO;
using System.Text;
using System.Threading.Tasks;
using ContentModeration.Controllers;
using ContentModeration.Models;
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

        [SetUp]
        public void Setup()
        {
            _settings = Options.Create<Settings>(new Settings());
            _settings.Value.OcpApimSubscriptionKey = "8ada31fece3e4155bc862e68baf2c5de";

            _data = new Data()
            {
                DataRepresentation = "URL",
                Value = "https://moderatorsampleimages.blob.core.windows.net/samples/sample.jpg"
            };
        }

        [Test]
        public async Task CallJsonMethod_HappyPath()
        {
            await Task.Delay(1000);

            byte[] byteData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(_data));
            
            ImagesController controller = new ImagesController(_settings);
            var result = await controller.ProcessImage(byteData, "application/json");

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

            ImagesController controller = new ImagesController(_settings);
            var result = await controller.ProcessImage(imgdata, contentType);

            Assert.AreEqual(200, ((Microsoft.AspNetCore.Mvc.ObjectResult)result).StatusCode);
            Assert.IsTrue(((Microsoft.AspNetCore.Mvc.ObjectResult)result).Value.GetType() == typeof(Response));
        }
    }
}