using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace webtruyenBackEndAPI.Models
{
	public class OTruyenListResponse
	{
		[JsonPropertyName("status")]
		public string Status { get; set; }

		[JsonPropertyName("message")]
		public string Message { get; set; }

		[JsonPropertyName("data")]
		public OTruyenListData Data { get; set; }
	}

	public class OTruyenListData
	{
		[JsonPropertyName("items")]
		public List<OTruyenComicSummary> Items { get; set; } = new();

		[JsonPropertyName("APP_DOMAIN_CDN_IMAGE")]
		public string ImageCdn { get; set; }
	}

	public class OTruyenComicSummary
	{
		[JsonPropertyName("_id")]
		public string Id { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("slug")]
		public string Slug { get; set; }

		[JsonPropertyName("origin_name")]
		public List<string> OriginName { get; set; } = new();

		[JsonPropertyName("status")]
		public string Status { get; set; }

		[JsonPropertyName("thumb_url")]
		public string ThumbUrl { get; set; }

		[JsonPropertyName("sub_docquyen")]
		public bool SubDocQuyen { get; set; }

		[JsonPropertyName("category")]
		public List<OTruyenCategory> Categories { get; set; } = new();

		[JsonPropertyName("updatedAt")]
		public DateTime? UpdatedAt { get; set; }

		[JsonPropertyName("chaptersLatest")]
		public List<OTruyenLatestChapter> ChaptersLatest { get; set; } = new();
	}

	public class OTruyenCategory
	{
		[JsonPropertyName("id")]
		public string Id { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("slug")]
		public string Slug { get; set; }
	}

	public class OTruyenLatestChapter
	{
		[JsonPropertyName("filename")]
		public string Filename { get; set; }

		[JsonPropertyName("chapter_name")]
		public string ChapterName { get; set; }

		[JsonPropertyName("chapter_title")]
		public string ChapterTitle { get; set; }

		[JsonPropertyName("chapter_api_data")]
		public string ChapterApiData { get; set; }
	}

	public class OTruyenDetailResponse
	{
		[JsonPropertyName("status")]
		public string Status { get; set; }

		[JsonPropertyName("message")]
		public string Message { get; set; }

		[JsonPropertyName("data")]
		public OTruyenDetailData Data { get; set; }
	}

	public class OTruyenDetailData
	{
		[JsonPropertyName("item")]
		public OTruyenComicDetail Item { get; set; }

		[JsonPropertyName("APP_DOMAIN_CDN_IMAGE")]
		public string ImageCdn { get; set; }
	}

	public class OTruyenComicDetail
	{
		[JsonPropertyName("_id")]
		public string Id { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("slug")]
		public string Slug { get; set; }

		[JsonPropertyName("origin_name")]
		public List<string> OriginName { get; set; } = new();

		[JsonPropertyName("content")]
		public string Content { get; set; }

		[JsonPropertyName("status")]
		public string Status { get; set; }

		[JsonPropertyName("thumb_url")]
		public string ThumbUrl { get; set; }

		[JsonPropertyName("sub_docquyen")]
		public bool SubDocQuyen { get; set; }

		[JsonPropertyName("author")]
		public List<string> Authors { get; set; } = new();

		[JsonPropertyName("category")]
		public List<OTruyenCategory> Categories { get; set; } = new();

		[JsonPropertyName("chapters")]
		public List<OTruyenServer> Chapters { get; set; } = new();

		[JsonPropertyName("updatedAt")]
		public DateTime? UpdatedAt { get; set; }
	}

	public class OTruyenServer
	{
		[JsonPropertyName("server_name")]
		public string ServerName { get; set; }

		[JsonPropertyName("server_data")]
		public List<OTruyenChapter> ServerData { get; set; } = new();
	}

	public class OTruyenChapter
	{
		[JsonPropertyName("filename")]
		public string Filename { get; set; }

		[JsonPropertyName("chapter_name")]
		public string ChapterName { get; set; }

		[JsonPropertyName("chapter_title")]
		public string ChapterTitle { get; set; }

		[JsonPropertyName("chapter_api_data")]
		public string ChapterApiData { get; set; }
	}
}
