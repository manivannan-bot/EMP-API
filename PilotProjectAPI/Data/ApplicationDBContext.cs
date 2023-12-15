using Microsoft.EntityFrameworkCore;
using PilotProjectAPI.Models;

namespace PilotProjectAPI.Data
{
    public class ApplicationDBContext:DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { 
        }
       
        public DbSet<Member> Members { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<EmployeeMaster> EmployeeMaster { get; set; }
        public DbSet<EmployeeContacts> EmployeeContacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ignore the Department navigation property in EmployeeMaster
            modelBuilder.Entity<EmployeeMaster>()
                .Ignore(e => e.Department);

            base.OnModelCreating(modelBuilder);
        }


    }
}
