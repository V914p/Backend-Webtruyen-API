using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using webtruyenBackEndAPI.Crawl_comic;

namespace webtruyenBackEndAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CrawlController : ControllerBase
	{
		private readonly IComicCrawler _crawler;

		public CrawlController(IComicCrawler crawler)
		{
			_crawler = crawler;
		}

		[HttpPost("latest")]
		public async Task<IActionResult> CrawlLatestAsync([FromQuery] int page = 1, CancellationToken cancellationToken = default)
		{
			var summary = await _crawler.CrawlLatestAsync(page, cancellationToken);
			return Ok(summary);
		}
	}
}
