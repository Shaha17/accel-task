using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IronBarCode;
using Microsoft.AspNetCore.Hosting;

namespace LinkShortener.Services
{
    public class QrCodeService : IQrCodeService
    {
        private readonly IWebHostEnvironment _environment;

        public QrCodeService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> CreateAsync(string text, string token)
        {
            var code = QRCodeWriter.CreateQrCode(text, 500, QRCodeWriter.QrErrorCorrectionLevel.Medium);
            var filePath = GetFilePath(token);
            var fileName = Path.Combine("qr", filePath.Split("/").Last());
            await Task.Run(() => { code.SaveAsImage(filePath); });
            return fileName;
        }

        private string GetFilePath(string token)
        {
            var rootPath = _environment.WebRootPath;
            var fileName = $"{token}.png";
            return Path.Combine(rootPath, "qr", fileName);
        }
    }
}