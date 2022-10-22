using Microsoft.EntityFrameworkCore;

namespace LinkShortener.Models
{
    [Index("Url","Token")]
    public class Link
    {
        public long Id { get; set; }
        public string Url { get; set; }
        public string Token { get; set; }
        public string PathToQr { get; set; }
    }
}