using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinkShortener.Models;
using LinkShortener.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LinkShortener.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LinkShortenerController : ControllerBase
    {
        private readonly ILogger<LinkShortenerController> _logger;
        private readonly ILinkService _linkService;

        public LinkShortenerController(ILogger<LinkShortenerController> logger, ILinkService linkService)
        {
            _logger = logger;
            _linkService = linkService;
        }

        [HttpPost]
        public async Task<ActionResult<Link>> Post(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return BadRequest();
            }

            url = url.Trim();
            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
            {
                url = "http://" + url;
            }

            try
            {
                var uri = new Uri(url);
            }
            catch (UriFormatException)
            {
                return BadRequest("Wrong url format");
            }

            var link = await _linkService.CreateShortLinkAsync(url);
            link.PathToQr = HttpContext.Request.Scheme+"://"+HttpContext.Request.Host + "/" + link.PathToQr;

            return link;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return BadRequest();
            }

            token = token.Trim();

            var link = await _linkService.FindByTokenAsync(token);
            if (link == null)
            {
                return NotFound();
            }


            return Redirect(link.Url);
        }
    }
}