using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DynamicFormPresentation.Models;

public partial class SDirectContext : DbContext
{
    public SDirectContext()
    {
    }

    public SDirectContext(DbContextOptions<SDirectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AnswerMaster> AnswerMasters { get; set; }

    public virtual DbSet<AnswerOption> AnswerOptions { get; set; }

    public virtual DbSet<AnswerType> AnswerTypes { get; set; }

    public virtual DbSet<FormsTable> FormsTables { get; set; }

    public virtual DbSet<QuestionBank> QuestionBanks { get; set; }

    public virtual DbSet<Response> Responses { get; set; }

    public virtual DbSet<SectionQuestionMapping> SectionQuestionMappings { get; set; }

    public virtual DbSet<SectionTable> SectionTables { get; set; }

    public virtual DbSet<UserCredential> UserCredentials { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AnswerMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AnswerMa__3214EC2707CA8CFD");

            entity.ToTable("AnswerMaster");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AnswerOptionId).HasColumnName("AnswerOptionID");
            entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

            entity.HasOne(d => d.AnswerOption).WithMany(p => p.AnswerMasters)
                .HasForeignKey(d => d.AnswerOptionId)
                .HasConstraintName("FK__AnswerMas__Answe__73B00EE2");

            entity.HasOne(d => d.Question).WithMany(p => p.AnswerMasters)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK__AnswerMas__Quest__72BBEAA9");
        });

        modelBuilder.Entity<AnswerOption>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AnswerOp__3214EC27922DC775");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.AnswerTypeId).HasColumnName("AnswerTypeID");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DeletedDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.OptionValue)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.AnswerType).WithMany(p => p.AnswerOptions)
                .HasForeignKey(d => d.AnswerTypeId)
                .HasConstraintName("FK__AnswerOpt__Answe__6FDF7DFE");
        });

        modelBuilder.Entity<AnswerType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AnswerTy__3214EC277B7B29F9");

            entity.ToTable("AnswerType");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DeletedDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.TypeName)
                .HasMaxLength(25)
                .IsUnicode(false);
        });

        modelBuilder.Entity<FormsTable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FormsTab__3214EC276FCF5E00");

            entity.ToTable("FormsTable");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.Comments)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DeletedDate).HasColumnType("datetime");
            entity.Property(e => e.FormName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.FormsTables)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__FormsTabl__UserI__5DC0CDC3");
        });

        modelBuilder.Entity<QuestionBank>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3214EC27349A07D7");

            entity.ToTable("QuestionBank");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.ConstraintValue)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Constraints)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DataType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DeletedDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.Questions)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.TextBoxSize)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WarningMessage)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Response>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Response__3214EC270E00DDE7");

            entity.ToTable("Response");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.AnswerMasterId).HasColumnName("AnswerMasterID");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DeletedDate).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.FormId).HasColumnName("FormID");
            entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.Response1)
                .IsUnicode(false)
                .HasColumnName("Response");

            entity.HasOne(d => d.AnswerMaster).WithMany(p => p.Responses)
                .HasForeignKey(d => d.AnswerMasterId)
                .HasConstraintName("FK__Response__Answer__7874C3FF");

            entity.HasOne(d => d.Form).WithMany(p => p.Responses)
                .HasForeignKey(d => d.FormId)
                .HasConstraintName("FK__Response__FormID__77809FC6");
        });

        modelBuilder.Entity<SectionQuestionMapping>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SectionQ__3214EC2766763B2A");

            entity.ToTable("SectionQuestionMapping");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.QuestionId).HasColumnName("QuestionID");
            entity.Property(e => e.SectionId).HasColumnName("SectionID");

            entity.HasOne(d => d.Question).WithMany(p => p.SectionQuestionMappings)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK__SectionQu__Quest__7C4554E3");

            entity.HasOne(d => d.Section).WithMany(p => p.SectionQuestionMappings)
                .HasForeignKey(d => d.SectionId)
                .HasConstraintName("FK__SectionQu__Secti__7D39791C");
        });

        modelBuilder.Entity<SectionTable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SectionT__3214EC27C9C584CE");

            entity.ToTable("SectionTable");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DeletedDate).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.FormId).HasColumnName("FormID");
            entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.SectionName)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.Form).WithMany(p => p.SectionTables)
                .HasForeignKey(d => d.FormId)
                .HasConstraintName("FK__SectionTa__Modif__61915EA7");
        });

        modelBuilder.Entity<UserCredential>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserCred__3214EC27C401987C");

            entity.ToTable("UserCredential");

            entity.HasIndex(e => e.Email, "UQ__UserCred__A9D1053485570BBB").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DeletedDate).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
