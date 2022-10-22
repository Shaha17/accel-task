using System.Linq;
using System.Threading.Tasks;
using LinkShortener.Context;
using LinkShortener.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LinkShortener.Repository
{
    public class LinksRepository : ILinksRepository
    {
        private readonly AppDbContext _context;

        public LinksRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Link> CreateAsync(Link link)
        {
            await _context.Links.AddAsync(link);
            await _context.SaveChangesAsync();
            return link;
        }

        public async Task<Link> GetByTokenAsync(string token)
        {
            var link = await _context.Links.FirstOrDefaultAsync(l => l.Token.Equals(token));
            return link;
        }

        public async Task<Link> GetByFullUrlAsync(string url)
        {
            var link = await _context.Links.FirstOrDefaultAsync(l => l.Url.Equals(url));
            return link;
        }
    }
}