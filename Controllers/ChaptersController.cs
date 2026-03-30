using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webtruyenBackEndAPI.Data;
using webtruyenBackEndAPI.Models;

namespace webtruyenBackEndAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ChaptersController : ControllerBase
	{
		private readonly AppDbContext _context;
		public ChaptersController(AppDbContext context)
		{
			_context = context;
		}

		// 6️⃣ GET: Lấy chapters của 1 truyện
		[HttpGet("comic/{comicId}")]
		public async Task<ActionResult<IEnumerable<Chapter>>> GetChaptersByComic(string comicId)
		{
			var chapters = await _context.Chapters
				.Where(c => c.ComicId == comicId)
				.OrderBy(c => c.ChapterIndex)
				.ToListAsync();

			return Ok(chapters);
		}
	}
}
