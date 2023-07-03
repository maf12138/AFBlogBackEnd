using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrasturacture.Configs
{
    internal class CommentConfig : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("T_Comments");
            builder.HasOne<User>(c => c.CommentUser).WithMany();
            builder.HasOne<Article>(c => c.Article).WithMany();
            builder.Property(c => c.Content).IsRequired();
            builder.HasQueryFilter(c => c.IsDeleted == false);
        }
    }
}
