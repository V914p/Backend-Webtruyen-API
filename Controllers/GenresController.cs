using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webtruyenBackEndAPI.Data;
using webtruyenBackEndAPI.Models;

namespace webtruyenBackEndAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class GenresController : ControllerBase
	{
		private readonly AppDbContext _context;
		public GenresController(AppDbContext context)
		{
			_context = context;
		}

		// 5️⃣ GET: Danh sách thể loại
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Genre>>> GetGenres()
		{
			return await _context.Genres.ToListAsync();
		}
	}
}
