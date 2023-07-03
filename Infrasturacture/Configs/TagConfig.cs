using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrasturacture.Configs
{
    internal class TagConfig : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable("T_Tags");
            builder.HasKey(x => x.Id);
            builder.HasMany<Article>(t => t.Articles).WithMany(a => a.Tags).UsingEntity(j => j.ToTable("T_ArticleTags")); //其实用配置多对多,导航属性配置好了直接识别出来
        }
    }
}
