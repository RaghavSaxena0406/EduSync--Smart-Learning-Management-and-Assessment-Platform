﻿using System;
using System.Collections.Generic;
using EduSyncWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EduSyncWebApi.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Assessment> Assessments { get; set; }
    public virtual DbSet<AssessmentResult> AssessmentResults { get; set; }
    public virtual DbSet<Course> Courses { get; set; }
    public virtual DbSet<Result> Results { get; set; }
    public virtual DbSet<UserModel> UserModels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Assessment>(entity =>
        {
            entity.HasKey(e => e.AssessmentId).HasName("PK__Assessme__3D2BF81E2E1CFF82");

            entity.ToTable("Assessment");

            entity.Property(e => e.AssessmentId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.Course).WithMany(p => p.Assessments)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__Assessmen__Cours__3E52440B");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Course__C92D71A72D95F113");

            entity.ToTable("Course");

            entity.Property(e => e.CourseId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.MediaUrl)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Instructor).WithMany(p => p.Courses)
                .HasForeignKey(d => d.InstructorId)
                .HasConstraintName("FK_Course_UserModel");
        });

        modelBuilder.Entity<Result>(entity =>
        {
            entity.ToTable("Result");

            entity.Property(e => e.ResultId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.AttemptDate).HasColumnType("datetime");

            entity.HasOne(d => d.Assessment).WithMany(p => p.Results)
                .HasForeignKey(d => d.AssessmentId)
                .HasConstraintName("FK_Result_Assessment");

            entity.HasOne(d => d.User).WithMany(p => p.Results)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Result_UserModel");
        });

        modelBuilder.Entity<UserModel>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__UserMode__1788CC4C26566554");

            entity.ToTable("UserModel");

            entity.HasIndex(e => e.Email, "UQ_UserModels_Email").IsUnique();

            //entity.HasIndex(e => e.Email, "UQ__UserMode__A9D10534F75F0D55").IsUnique();

            entity.Property(e => e.UserId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(225)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AssessmentResult>(entity =>
        {
            entity.HasKey(e => e.ResultId).HasName("PK_AssessmentResults");

            entity.ToTable("AssessmentResult");

            entity.Property(e => e.ResultId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.SubmissionDate).HasColumnType("datetime");
            entity.Property(e => e.Answers).IsUnicode(false);

            entity.HasOne(d => d.Assessment)
                .WithMany()
                .HasForeignKey(d => d.AssessmentId)
                .HasConstraintName("FK_AssessmentResult_Assessment");

            entity.HasOne(d => d.Student)
                .WithMany()
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK_AssessmentResult_UserModel");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
