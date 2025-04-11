using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VacationManager.Models;

namespace VacationManager.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<VacationRequest> VacationRequests { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Връзка: Екип -> Лидер
            builder.Entity<Team>()
                .HasOne(t => t.TeamLead)
                .WithMany()
                .HasForeignKey(t => t.TeamLeadId)
                .OnDelete(DeleteBehavior.SetNull);

            // Връзка: Екип -> Разработчици
            builder.Entity<Team>()
                .HasMany(t => t.Developers)
                .WithOne(u => u.Team)
                .HasForeignKey(u => u.TeamId)
                .OnDelete(DeleteBehavior.SetNull);
        }
        public async Task<bool> IsUserOnVacation(string userId, DateTime date)
        {
            return await VacationRequests
                .AnyAsync(r =>
                    r.UserId == userId &&
                    r.IsApproved &&
                    r.FromDate <= date &&
                    r.ToDate >= date);
        }

    }
}
