using System.Threading.Tasks;
using ContentModeration.Models;

namespace ContentModeration.Services
{
    public interface IContentModerationService
    {
        Task<EvaluateResponse> ProcessImageEvaluate(byte[] byteData, string contentType);
        Task<OcrResponse> ProcessImageOcr(byte[] byteData, string contentType, bool enhanced);
    }
}