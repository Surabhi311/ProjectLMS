using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ProjectLMS.Models
{
    public partial class LMS_dbContext : DbContext
    {
        public LMS_dbContext()
        {
        }

        public LMS_dbContext(DbContextOptions<LMS_dbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<LendRequest> LendRequests { get; set; }
        public virtual DbSet<Publisher> Publishers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost;Database=LMS_db;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.Password).IsRequired();

                entity.Property(e => e.Role).IsRequired();

                entity.Property(e => e.Username).IsRequired();
            });

            modelBuilder.Entity<Author>(entity =>
            {
                entity.Property(e => e.AuthorName).IsRequired();
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasIndex(e => e.AuthorId, "IX_Books_AuthorId");

                entity.HasIndex(e => e.PublisherId, "IX_Books_PublisherId");

                entity.Property(e => e.BookTitle).IsRequired();

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.AuthorId);

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.PublisherId);
            });

            modelBuilder.Entity<LendRequest>(entity =>
            {
                entity.HasKey(e => e.LendId);

                entity.HasIndex(e => e.BookId, "IX_LendRequests_BookId");

                entity.HasIndex(e => e.UserId, "IX_LendRequests_UserId");

                entity.Property(e => e.LendStatus).IsRequired();

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.LendRequests)
                    .HasForeignKey(d => d.BookId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.LendRequests)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Publisher>(entity =>
            {
                entity.Property(e => e.PublisherName).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
