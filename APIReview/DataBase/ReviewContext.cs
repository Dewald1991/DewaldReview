using APIReview.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace APIReview.DataBase
{
    public class ReviewContext : DbContext
    {

        public ReviewContext() : base("DefaultConnection")
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmploymentStatus> EmploymentStatus { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}