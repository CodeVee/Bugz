using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityRole<Guid>> builder)
        {
            builder.HasData(
            new IdentityRole<Guid>
            {
                Id = Guid.Parse("a08cc95f-bdae-4284-8f56-c7df179c1c77"),
                Name = "Project Manager",
                NormalizedName = "PROJECT MANAGER"
            },
            new IdentityRole<Guid>
            {
                Id = Guid.Parse("83513221-c2ac-4924-8c00-5dc553e98878"),
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR"
            },
            new IdentityRole<Guid>
            {
                Id = Guid.Parse("75e8d32e-23a6-4f9b-b52d-c8542171d9c4"),
                Name = "Developer",
                NormalizedName = "DEVELOPER"
            },
            new IdentityRole<Guid>
            {
                Id = Guid.Parse("68dd560f-a167-457c-b572-ee542eb56fa3"),
                Name = "Submitter",
                NormalizedName = "SUBMITTER"
            });
            
        }
    }
}