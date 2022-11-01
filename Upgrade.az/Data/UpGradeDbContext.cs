using Microsoft.EntityFrameworkCore;
using Upgrade_az.Models.CourseDetail;
using Upgrade_az.Models.User;

namespace Upgrade_az.Data
{
    public class UpGradeDbContext : DbContext
    {
        public UpGradeDbContext()
        {
        }
        public UpGradeDbContext(DbContextOptions<UpGradeDbContext> options) : base(options)
        {
        }

        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<SlideShow> RoleAccounts { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<LectureParticle> LectureParticles { get; set; }





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SlideShow>(entity =>
            {
                entity.HasKey(e => new { e.RoleId, e.AccountId });

                entity.HasOne(d => d.Account)
                      .WithMany(p => p.RoleAccounts)
                      .HasForeignKey(d => d.AccountId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK_Account_Role_Account");

                entity.HasOne(d => d.Role)
                      .WithMany(p => p.RoleAccounts)
                      .HasForeignKey(d => d.RoleId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK_Account_Role_Role");




            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasMaxLength(250).
                    IsUnicode(false);
            });



            //modelBuilder.Entity<Course>(entity =>
            //{
            //    entity.HasOne(d => d.Category)
            //          .WithMany(p => p.Courses)
            //          .HasForeignKey(d => d.CategoryId)
            //          .OnDelete(DeleteBehavior.ClientSetNull)
            //          .HasConstraintName("FK_Category_Course");

            //});

        }
    }
}

