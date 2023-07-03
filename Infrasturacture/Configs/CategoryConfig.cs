using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrasturacture.Configs
{
    internal class CategoryConfig : IEntityTypeConfiguration<Domain.Entities.Category>

    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Category> builder)
        {
            builder.ToTable("T_Categories");
        }
    }
}
