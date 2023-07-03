using Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace Infrasturacture.Configs
{
    public class ArticleConfig : IEntityTypeConfiguration<Article>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Article> builder)
        {
            builder.ToTable("T_Articles");
            builder.HasOne<User>(a => a.Author).WithMany();
            builder.HasOne<Category>(a => a.Category).WithMany();
            builder.Property(a => a.Description).HasComment("博客描述");
            builder.Property(a => a.Content).HasColumnType("BLOB");
            builder.HasQueryFilter(a => a.IsDeleted == false);
        }
    }
}
