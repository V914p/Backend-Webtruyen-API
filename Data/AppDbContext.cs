using Microsoft.EntityFrameworkCore;
using webtruyenBackEndAPI.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace webtruyenBackEndAPI.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		public DbSet<Comic> Comics { get; set; }
		public DbSet<Genre> Genres { get; set; }
		public DbSet<Chapter> Chapters { get; set; }
		public DbSet<ComicGenre> ComicGenres { get; set; }
		public DbSet<Account> Accounts { get; set; }
		public DbSet<Follow> Follows { get; set; }
		public DbSet<ComicFollow> ComicFollows { get; set; }
		public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ComicGenre>()
				.HasOne(cg => cg.Comic)
				.WithMany(c => c.ComicGenres)
				.HasForeignKey(cg => cg.ComicId);

			modelBuilder.Entity<ComicGenre>()
				.HasOne(cg => cg.Genre)
				.WithMany(g => g.ComicGenres)
				.HasForeignKey(cg => cg.GenreId);
		}
	}
}

