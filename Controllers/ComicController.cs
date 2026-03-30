using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webtruyenBackEndAPI.Data;
using webtruyenBackEndAPI.Models;


namespace webtruyenBackEndAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ComicsController : ControllerBase
	{
		private readonly AppDbContext _context;
		public ComicsController(AppDbContext context)
		{
			_context = context;
		}

		// 1️⃣ GET: Danh sách truyện (20 truyện / trang)
		[HttpGet("page")]
		public async Task<ActionResult<IEnumerable<Comic>>> GetComicsByPage([FromQuery] int page = 1)
		{
			int pageSize = 20;
			var comics = await _context.Comics
				.OrderByDescending(c => c.UpdatedAt)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			return Ok(comics);
		}

		// 2️⃣ GET: Tìm kiếm truyện theo tên
		[HttpGet("search")]
		public async Task<ActionResult<IEnumerable<Comic>>> SearchComics([FromQuery] string keyword)
		{
			var comics = await _context.Comics
				.Where(c => c.Name.Contains(keyword))
				.ToListAsync();

			return Ok(comics);
		}

		// 3️⃣ GET: Chi tiết 1 truyện
		[HttpGet("{comicid}")]
		public async Task<ActionResult<Comic>> GetComicDetail(string comicid)
		{
			var comic = await _context.Comics
		.Include(c => c.ComicGenres)
			.ThenInclude(cg => cg.Genre)
		.FirstOrDefaultAsync(c => c.ComicId == comicid);

			if (comic == null)
				return NotFound();

			// 📌 Lấy chapters từ DB và sắp xếp trực tiếp trong query SQL
			comic.Chapters = await _context.Chapters
				.Where(ch => ch.ComicId == comicid)
				.OrderBy(ch => ch.ChapterIndex)
				.ToListAsync();

			return Ok(comic);
		}

		// 4️⃣ GET: Lấy truyện theo thể loại
		[HttpGet("genre/{genreId}")]
		public async Task<ActionResult<IEnumerable<Comic>>> GetComicsByGenre(string genreId)
		{
			var comics = await _context.ComicGenres
				.Where(cg => cg.GenreId == genreId)
				.Select(cg => cg.Comic)
				.ToListAsync();

			return Ok(comics);
		}
	}
}
