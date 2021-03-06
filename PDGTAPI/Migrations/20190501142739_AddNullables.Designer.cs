﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PDGTAPI.Infrastructure;

namespace PDGTAPI.Migrations
{
    [DbContext(typeof(ApplicationDataContext))]
    [Migration("20190501142739_AddNullables")]
    partial class AddNullables
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.8-servicing-32085")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("PDGTAPI.Infrastructure.Exercise", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ExerciseName")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<int>("GuideId")
                        .HasColumnName("GuideID");

                    b.HasKey("Id");

                    b.HasIndex("GuideId");

                    b.ToTable("Exercise");
                });

            modelBuilder.Entity("PDGTAPI.Infrastructure.GroupHasExerciseInTimeRange", b =>
                {
                    b.Property<int>("GroupId")
                        .HasColumnName("GroupID");

                    b.Property<int>("ExerciseId")
                        .HasColumnName("ExerciseID");

                    b.Property<int>("TimeRangeId")
                        .HasColumnName("TimeRangeID");

                    b.Property<string>("UserId");

                    b.HasKey("GroupId", "ExerciseId", "TimeRangeId");

                    b.HasIndex("ExerciseId");

                    b.HasIndex("TimeRangeId");

                    b.HasIndex("UserId");

                    b.ToTable("GroupHasExerciseInTimeRange");
                });

            modelBuilder.Entity("PDGTAPI.Infrastructure.Guide", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("GuideDescription")
                        .IsRequired()
                        .HasMaxLength(2048);

                    b.Property<byte[]>("GuideImage")
                        .IsRequired()
                        .HasColumnType("image");

                    b.HasKey("Id");

                    b.ToTable("Guide");
                });

            modelBuilder.Entity("PDGTAPI.Infrastructure.RandomisationGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("GroupName")
                        .IsRequired()
                        .HasMaxLength(1)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.ToTable("RandomisationGroup");
                });

            modelBuilder.Entity("PDGTAPI.Infrastructure.Session", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CompletionTime")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.ToTable("Session");
                });

            modelBuilder.Entity("PDGTAPI.Infrastructure.TimeRange", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte>("EndWeek");

                    b.Property<byte>("RepsAmount");

                    b.Property<byte>("SetsAmount");

                    b.Property<byte>("StartWeek");

                    b.HasKey("Id");

                    b.ToTable("TimeRange");
                });

            modelBuilder.Entity("PDGTAPI.Infrastructure.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<int?>("RandomisationGroupID");

                    b.Property<DateTime>("RedCapBaseline");

                    b.Property<int?>("RedCapRecordId");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("RandomisationGroupID");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("PDGTAPI.Infrastructure.UserHasExerciseWeightInTimeRange", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnName("UserID");

                    b.Property<int>("ExerciseId")
                        .HasColumnName("ExerciseID");

                    b.Property<int>("TimeRangeId")
                        .HasColumnName("TimeRangeID");

                    b.Property<byte>("UserExerciseWeight");

                    b.HasKey("UserId", "ExerciseId", "TimeRangeId");

                    b.HasIndex("ExerciseId");

                    b.HasIndex("TimeRangeId");

                    b.ToTable("UserHasExerciseWeightInTimeRange");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("PDGTAPI.Infrastructure.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("PDGTAPI.Infrastructure.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PDGTAPI.Infrastructure.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("PDGTAPI.Infrastructure.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PDGTAPI.Infrastructure.Exercise", b =>
                {
                    b.HasOne("PDGTAPI.Infrastructure.Guide", "Guide")
                        .WithMany("Exercise")
                        .HasForeignKey("GuideId")
                        .HasConstraintName("FK__Exercise__GuideI__619B8048");
                });

            modelBuilder.Entity("PDGTAPI.Infrastructure.GroupHasExerciseInTimeRange", b =>
                {
                    b.HasOne("PDGTAPI.Infrastructure.Exercise", "Exercise")
                        .WithMany("GroupHasExerciseInTimeRange")
                        .HasForeignKey("ExerciseId")
                        .HasConstraintName("FK__GroupHasE__Exerc__73BA3083");

                    b.HasOne("PDGTAPI.Infrastructure.RandomisationGroup", "Group")
                        .WithMany("GroupHasExerciseInTimeRange")
                        .HasForeignKey("GroupId")
                        .HasConstraintName("FK__GroupHasE__Group__72C60C4A");

                    b.HasOne("PDGTAPI.Infrastructure.TimeRange", "TimeRange")
                        .WithMany("GroupHasExerciseInTimeRange")
                        .HasForeignKey("TimeRangeId")
                        .HasConstraintName("FK__GroupHasE__TimeR__74AE54BC");

                    b.HasOne("PDGTAPI.Infrastructure.User")
                        .WithMany("GroupHasExerciseInTimeRange")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("PDGTAPI.Infrastructure.User", b =>
                {
                    b.HasOne("PDGTAPI.Infrastructure.RandomisationGroup")
                        .WithMany("Users")
                        .HasForeignKey("RandomisationGroupID");
                });

            modelBuilder.Entity("PDGTAPI.Infrastructure.UserHasExerciseWeightInTimeRange", b =>
                {
                    b.HasOne("PDGTAPI.Infrastructure.Exercise", "Exercise")
                        .WithMany("UserHasExerciseWeightInTimeRange")
                        .HasForeignKey("ExerciseId")
                        .HasConstraintName("FK__UserHasEx__Exerc__6EF57B66");

                    b.HasOne("PDGTAPI.Infrastructure.TimeRange", "TimeRange")
                        .WithMany("UserHasExerciseWeightInTimeRange")
                        .HasForeignKey("TimeRangeId")
                        .HasConstraintName("FK__UserHasEx__TimeR__6FE99F9F");

                    b.HasOne("PDGTAPI.Infrastructure.User", "User")
                        .WithMany("UserHasExerciseWeightInTimeRange")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK__UserHasEx__UserI__6E01572D");
                });
#pragma warning restore 612, 618
        }
    }
}
