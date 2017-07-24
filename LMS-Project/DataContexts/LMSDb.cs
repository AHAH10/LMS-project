using LMS_Entities;
using System.Data.Entity;

namespace LMS_Project.DataContexts
{
    public class LMSDb : DbContext
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

        DbSet<Classroom> Classrooms { get; set; }
    }
}