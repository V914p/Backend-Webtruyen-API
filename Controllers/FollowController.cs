using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webtruyenBackEndAPI.Data;
using webtruyenBackEndAPI.DTOs;
using webtruyenBackEndAPI.Models;

[ApiController]
[Route("api/[controller]")]
public class FollowController : ControllerBase
{
	private readonly AppDbContext _context;

	public FollowController(AppDbContext context)
	{
		_context = context;
	}

	[Authorize]
	[HttpPost]
	public async Task<IActionResult> FollowComic(FollowRequest req)
	{
		var follow = _context.Follows.FirstOrDefault(f => f.AccountId == req.AccountId);
		if (follow == null)
		{
			follow = new Follow { AccountId = req.AccountId };
			_context.Follows.Add(follow);
			await _context.SaveChangesAsync();
		}

		bool already = _context.ComicFollows.Any(cf => cf.FollowId == follow.FollowId && cf.ComicId == req.ComicId);
		if (already)
			return BadRequest("Đã theo dõi truyện này");

		_context.ComicFollows.Add(new ComicFollow
		{
			ComicId = req.ComicId,
			FollowId = follow.FollowId
		});

		await _context.SaveChangesAsync();
		return Ok("Đã theo dõi truyện");
	}

	[Authorize]
	[HttpDelete("{accountId}/{comicId}")]
	public async Task<IActionResult> Unfollow(string accountId, string comicId)
	{
		var follow = _context.Follows.FirstOrDefault(f => f.AccountId == accountId);
		if (follow == null) return NotFound();

		var item = _context.ComicFollows.FirstOrDefault(cf => cf.FollowId == follow.FollowId && cf.ComicId == comicId);
		if (item == null) return NotFound();

		_context.ComicFollows.Remove(item);
		await _context.SaveChangesAsync();
		return Ok("Đã hủy theo dõi");
	}

	[Authorize]
	[HttpGet("{accountId}")]
	public IActionResult GetFollowedComics(string accountId)
	{
		var comics = from f in _context.Follows
					 join cf in _context.ComicFollows on f.FollowId equals cf.FollowId
					 join c in _context.Comics on cf.ComicId equals c.ComicId
					 where f.AccountId == accountId
					 select new
					 {
						 c.ComicId,
						 c.Name,
						 c.ThumbUrl,
						 cf.CreatedAt
					 };

		return Ok(comics);
	}
}
