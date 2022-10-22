using System.Threading.Tasks;

namespace LinkShortener.Services
{
    public interface IQrCodeService
    {
        Task<string> CreateAsync(string text, string token);
    }
}