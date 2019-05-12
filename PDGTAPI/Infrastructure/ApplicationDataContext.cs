using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDGTAPI.Infrastructure
{
	public class ApplicationDataContext : IdentityDbContext<User, IdentityRole, string>
	{
        public virtual DbSet<Exercise> Exercise { get; set; }
        public virtual DbSet<RandomisationGroup> RandomisationGroup { get; set; }
		public virtual DbSet<TimeRangeHasExercise> TimeRangeHasExecise { get; set; }
        public virtual DbSet<Session> Session { get; set; }
        public virtual DbSet<TimeRange> TimeRange { get; set; }

		public ApplicationDataContext(DbContextOptions options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<TimeRangeHasExercise>(entity =>
			{
				entity.HasKey(e => new { e.ExerciseId, e.TimeRangeId });

				entity.Property(e => e.ExerciseId).HasColumnName("ExerciseID");

				entity.Property(e => e.TimeRangeId).HasColumnName("TimeRangeID");

				entity.HasOne(d => d.Exercise)
					.WithMany(p => p.TimeRangeHasExercises)
					.HasForeignKey(d => d.ExerciseId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK__TimeRang__HasE__73BA3083");

				entity.HasOne(d => d.TimeRange)
					.WithMany(p => p.TimeRangeHasExercises)
					.HasForeignKey(d => d.TimeRangeId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK__TimeRang__HasE__72C60C4A");
			});

			modelBuilder.Entity<Exercise>(entity =>
			{
				entity.Property(e => e.Id).HasColumnName("ID");

				entity.Property(e => e.ExerciseName)
					.IsRequired()
					.HasMaxLength(255);
			});

			modelBuilder.Entity<RandomisationGroup>(entity =>
			{
				entity.Property(e => e.Id).HasColumnName("ID");

				entity.Property(e => e.GroupName)
					.IsRequired()
					.HasMaxLength(1)
					.IsUnicode(false);
			});

			modelBuilder.Entity<Session>(entity =>
			{
				entity.Property(e => e.Id).HasColumnName("ID");

				entity.Property(e => e.CompletionTime).HasColumnType("datetime");

				entity.Property(e => e.UserId).IsRequired();
			});

			modelBuilder.Entity<TimeRange>(entity =>
			{
				entity.Property(e => e.Id).HasColumnName("ID");
			});
		}
	}
}
