using System;
using Entities.Configurations;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class RepositoryContext : IdentityDbContext<User, IdentityRole<Guid>, Guid, 
        IdentityUserClaim<Guid>, IdentityUserRole<Guid>, IdentityUserLogin<Guid>, 
        IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public RepositoryContext(DbContextOptions options) : base(options) {  }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Issue> Issue { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<History> Activities { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Task>(options => 
            {
                options.HasOne(t => t.Assignee)
                    .WithMany(u => u.AssignedTask)
                    .HasForeignKey(t => t.AssigneeId);

                options.HasOne(t => t.Creator)
                    .WithMany(u => u.CreatedTask)
                    .HasForeignKey(t => t.CreatorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Issue>(options => 
            {
                options.HasOne(t => t.Assignee)
                    .WithMany(u => u.AssignedIssue)
                    .HasForeignKey(t => t.AssigneeId);

                options.HasOne(t => t.Reporter)
                    .WithMany(u => u.ReportedIssue)
                    .HasForeignKey(t => t.ReporterId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<History>(options =>
            {
                options.HasOne(h => h.User)
                    .WithMany(u => u.Activities)
                    .HasForeignKey(h => h.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Comment>(options =>
            {
                options.HasOne(c => c.User)
                    .WithMany(u => u.Comments)
                    .HasForeignKey(h => h.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<UserProject>()
                .HasKey(up => new { up.UserId, up.ProjectId});

            builder.ApplyConfiguration(new RoleConfiguration());
                
        }
    }
}