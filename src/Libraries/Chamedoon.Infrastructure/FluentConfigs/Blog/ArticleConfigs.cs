using Chamedoon.Domin.Entity.Blogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chamedoon.Infrastructure.FluentConfigs.Blog;

public class ArticleConfigs : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.Property(p => p.ArticleTitle).IsRequired().HasMaxLength(150);
        builder.Property(p => p.Writer).IsRequired().HasMaxLength(150);
        builder.Property(p => p.ArticleDescription).IsRequired();
        builder.Property(p => p.ShortDescription).IsRequired().HasMaxLength(1500);
    }
}