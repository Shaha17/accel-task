using System.Threading.Tasks;
using LinkShortener.Models;

namespace LinkShortener.Services
{
    public interface ILinkService
    {
        Task<Link> CreateShortLinkAsync(string urlStr);
        Task<Link> FindByTokenAsync(string token);
    }
}