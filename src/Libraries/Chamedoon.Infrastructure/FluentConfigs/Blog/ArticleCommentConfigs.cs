using Chamedoon.Domin.Entity.Blogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chamedoon.Infrastructure.FluentConfigs.Blog;

public class ArticleCommentConfigs : IEntityTypeConfiguration<ArticleComment>
{
    public void Configure(EntityTypeBuilder<ArticleComment> builder)
    {
        builder.Property(a=>a.Message).IsRequired().HasMaxLength(500);
    }
}
