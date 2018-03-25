using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ContentModeration.Services
{
    public interface IContentModerationService
    {
        Task<IActionResult> ProcessImage(byte[] byteData, string contentType);
    }
}