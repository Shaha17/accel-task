using System;
using System.Threading.Tasks;
using LinkShortener.Models;
using LinkShortener.Repository;
using Microsoft.AspNetCore.Http;

namespace LinkShortener.Services
{
    public class LinkService : ILinkService
    {
        private const string Alphabet = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890";
        private const int TokenLength = 8;

        private readonly ILinksRepository _linksRepository;
        private readonly IQrCodeService _qrCodeService;

        public LinkService(ILinksRepository linksRepository, IQrCodeService qrCodeService)
        {
            _linksRepository = linksRepository;
            _qrCodeService = qrCodeService;
        }

        private static string GenerateShortToken()
        {
            var rnd = new Random();
            var token = "";
            for (int i = 0; i < TokenLength; i++)
            {
                var pos = rnd.Next(0, Alphabet.Length - 1);
                token += Alphabet[pos];
            }

            return token;
        }

        public async Task<Link> CreateShortLinkAsync(string urlStr)
        {
            if (string.IsNullOrWhiteSpace(urlStr))
            {
                throw new ArgumentException("Url string is empty");
            }

            var url = urlStr.Trim();
            var link = await _linksRepository.GetByFullUrlAsync(url);
            if (link != null)
            {
                return link;
            }

            var token = GenerateShortToken();
            var qrFilePath = await _qrCodeService.CreateAsync(url, token);

            link = new Link()
            {
                Token = token,
                Url = url,
                PathToQr = qrFilePath,
            };

            await _linksRepository.CreateAsync(link);

            return link;
        }

        public async Task<Link> FindByTokenAsync(string token)
        {
            var link = await _linksRepository.GetByTokenAsync(token);
            return link;
        }
    }
}