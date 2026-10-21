using Cis.Infrastructure.MySql.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cis.Infrastructure.MySql.Context;

public class CisDbContext(DbContextOptions<CisDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Topic> Topics => Set<Topic>();
    public DbSet<Idea> Ideas => Set<Idea>();
    public DbSet<Vote> Votes => Set<Vote>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasColumnType("bigint unsigned")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.SourceId)
                .HasColumnName("source_id")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(e => e.Login)
                .HasColumnName("login")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(e => e.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamp")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Topic>(entity =>
        {
            entity.ToTable("topics");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasColumnType("bigint unsigned")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Title)
                .HasColumnName("title")
                .HasMaxLength(255)
                .IsRequired();

            entity.HasIndex(e => e.Title).IsUnique();

            entity.Property(e => e.Description)
                .HasColumnName("description")
                .HasColumnType("text")
                .IsRequired();

            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasMaxLength(50)
                .HasDefaultValue("open")
                .IsRequired();

            entity.Property(e => e.UserId)
                .HasColumnName("user_id")
                .HasColumnType("bigint unsigned");

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(e => e.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamp")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(e => e.User)
                .WithMany(e => e.Topics)
                .HasForeignKey(e => e.UserId)
                .HasConstraintName("fk_topics_user");
        });

        modelBuilder.Entity<Idea>(entity =>
        {
            entity.ToTable("ideas");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasColumnType("bigint unsigned")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Title)
                .HasColumnName("title")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(e => e.Description)
                .HasColumnName("description")
                .HasColumnType("text")
                .IsRequired();

            entity.Property(e => e.UserId)
                .HasColumnName("user_id")
                .HasColumnType("bigint unsigned");

            entity.Property(e => e.TopicId)
                .HasColumnName("topic_id")
                .HasColumnType("bigint unsigned");

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(e => e.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamp")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(e => e.User)
                .WithMany(e => e.Ideas)
                .HasForeignKey(e => e.UserId)
                .HasConstraintName("fk_ideas_user");

            entity.HasOne(e => e.Topic)
                .WithMany(e => e.Ideas)
                .HasForeignKey(e => e.TopicId)
                .HasConstraintName("fk_ideas_topic");
        });

        modelBuilder.Entity<Vote>(entity =>
        {
            entity.ToTable("votes");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasColumnType("bigint unsigned")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.UserId)
                .HasColumnName("user_id")
                .HasColumnType("bigint unsigned");

            entity.Property(e => e.TopicId)
                .HasColumnName("topic_id")
                .HasColumnType("bigint unsigned");

            entity.Property(e => e.IdeaId)
                .HasColumnName("idea_id")
                .HasColumnType("bigint unsigned");

            entity.Property(e => e.IsActive)
                .HasColumnName("is_active")
                .HasColumnType("tinyint(1)")
                .HasDefaultValue(true);

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(e => e.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamp")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(e => e.User)
                .WithMany(e => e.Votes)
                .HasForeignKey(e => e.UserId)
                .HasConstraintName("fk_votes_user");

            entity.HasOne(e => e.Topic)
                .WithMany(e => e.Votes)
                .HasForeignKey(e => e.TopicId)
                .HasConstraintName("fk_votes_topic");

            entity.HasOne(e => e.Idea)
                .WithMany(e => e.Votes)
                .HasForeignKey(e => e.IdeaId)
                .HasConstraintName("fk_votes_idea");
        });
    }
}