using LMS_Project.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Models.LMS;
using System.Data.Entity;

namespace LMS_Project.DataContexts
{
    public class LMSDb : IdentityDbContext<ApplicationUser>
    {
        public LMSDb()
            : base("DefaultConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("library");
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
    }
}