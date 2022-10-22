using System.Threading.Tasks;
using LinkShortener.Models;

namespace LinkShortener.Repository
{
    public interface ILinksRepository
    {
        Task<Link> CreateAsync(Link link);
        Task<Link> GetByTokenAsync(string token);
        Task<Link> GetByFullUrlAsync(string url);
    }
}