using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrasturacture.Configs
{
    internal class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {

            builder.ToTable("T_Users").HasComment("基础用户表");
            builder.HasQueryFilter(u => u.IsDeleted == false);
        }
    }
}
