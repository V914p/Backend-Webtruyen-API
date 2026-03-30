namespace webtruyenBackEndAPI.DTOs
{
	public record FollowRequest(
		string AccountId,
		string ComicId
	);

	public class FollowedComicResponse
	{
		public string ComicId { get; set; }
		public string Name { get; set; }
		public string ThumbUrl { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
