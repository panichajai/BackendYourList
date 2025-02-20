using BackendYourList.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackendYourList.Data
{
    public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<tb_Customer> customers { get; set; }
        public DbSet<tb_Project> projects { get; set; }
        public DbSet<tb_Assignee> assignees { get; set; }
        public DbSet<Models.Entities.tb_Task> tasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) // ตรวจสอบหากยังไม่ได้ถูก Config
            {
                optionsBuilder.UseSqlServer(
                    "Server=JAI\\SQLEXPRESS2024;Database=YourList;Trusted_connection=true;TrustServerCertificate=true;",
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null); // Retry 5 ครั้ง, รอ 10 วิ
                        sqlOptions.CommandTimeout(120); 
                    });
            }
        }
    }
}
