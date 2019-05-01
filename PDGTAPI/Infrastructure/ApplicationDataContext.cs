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
        public virtual DbSet<GroupHasExerciseInTimeRange> GroupHasExerciseInTimeRange { get; set; }
        public virtual DbSet<Guide> Guide { get; set; }
        public virtual DbSet<RandomisationGroup> RandomisationGroup { get; set; }
        public virtual DbSet<Session> Session { get; set; }
        public virtual DbSet<TimeRange> TimeRange { get; set; }
        public virtual DbSet<UserHasExerciseWeightInTimeRange> UserHasExerciseWeightInTimeRange { get; set; }

		public ApplicationDataContext(DbContextOptions options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Exercise>(entity =>
			{
				entity.Property(e => e.Id).HasColumnName("ID");

				entity.Property(e => e.ExerciseName)
					.IsRequired()
					.HasMaxLength(255);

				entity.Property(e => e.GuideId).HasColumnName("GuideID");

				entity.HasOne(d => d.Guide)
					.WithMany(p => p.Exercise)
					.HasForeignKey(d => d.GuideId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK__Exercise__GuideI__619B8048");
			});

			modelBuilder.Entity<GroupHasExerciseInTimeRange>(entity =>
			{
				entity.HasKey(e => new { e.GroupId, e.ExerciseId, e.TimeRangeId });

				entity.Property(e => e.GroupId).HasColumnName("GroupID");

				entity.Property(e => e.ExerciseId).HasColumnName("ExerciseID");

				entity.Property(e => e.TimeRangeId).HasColumnName("TimeRangeID");

				entity.HasOne(d => d.Exercise)
					.WithMany(p => p.GroupHasExerciseInTimeRange)
					.HasForeignKey(d => d.ExerciseId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK__GroupHasE__Exerc__73BA3083");

				entity.HasOne(d => d.Group)
					.WithMany(p => p.GroupHasExerciseInTimeRange)
					.HasForeignKey(d => d.GroupId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK__GroupHasE__Group__72C60C4A");

				entity.HasOne(d => d.TimeRange)
					.WithMany(p => p.GroupHasExerciseInTimeRange)
					.HasForeignKey(d => d.TimeRangeId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK__GroupHasE__TimeR__74AE54BC");
			});

			modelBuilder.Entity<Guide>(entity =>
			{
				entity.Property(e => e.Id).HasColumnName("ID");

				entity.Property(e => e.GuideDescription)
					.IsRequired()
					.HasMaxLength(2048);

				entity.Property(e => e.GuideImage)
					.IsRequired()
					.HasColumnType("image");
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
			});

			modelBuilder.Entity<TimeRange>(entity =>
			{
				entity.Property(e => e.Id).HasColumnName("ID");
			});

			modelBuilder.Entity<UserHasExerciseWeightInTimeRange>(entity =>
			{
				entity.HasKey(e => new { e.UserId, e.ExerciseId, e.TimeRangeId });

				entity.Property(e => e.UserId).HasColumnName("UserID");

				entity.Property(e => e.ExerciseId).HasColumnName("ExerciseID");

				entity.Property(e => e.TimeRangeId).HasColumnName("TimeRangeID");

				entity.HasOne(d => d.Exercise)
					.WithMany(p => p.UserHasExerciseWeightInTimeRange)
					.HasForeignKey(d => d.ExerciseId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK__UserHasEx__Exerc__6EF57B66");

				entity.HasOne(d => d.TimeRange)
					.WithMany(p => p.UserHasExerciseWeightInTimeRange)
					.HasForeignKey(d => d.TimeRangeId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK__UserHasEx__TimeR__6FE99F9F");

				entity.HasOne(d => d.User)
					.WithMany(p => p.UserHasExerciseWeightInTimeRange)
					.HasForeignKey(d => d.UserId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK__UserHasEx__UserI__6E01572D");
			});
		}
	}
}
