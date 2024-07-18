using Finbuckle.MultiTenant.EntityFrameworkCore;
using csumathboy.Domain.PostsAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace csumathboy.Infrastructure.Persistence.Configuration;

public class ClassificationConfig : IEntityTypeConfiguration<Classification>
{
    public void Configure(EntityTypeBuilder<Classification> builder)
    {
        builder
         .ToTable("Classification", SchemaNames.Posts);

        builder.IsMultiTenant();
        builder
            .Property(b => b.Name)
                .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH);
        builder
            .Property(p => p.NickName)
                .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH);
    }
}

public class PostConfig : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder
         .ToTable("Post", SchemaNames.Posts);
        builder.IsMultiTenant();
        builder
            .Property(b => b.Title).IsRequired()
                .HasMaxLength(DataSchemaConstants.DEFAULT_TITLE_LENGTH);
        builder
         .Property(b => b.Description).IsRequired()
             .HasMaxLength(DataSchemaConstants.DEFAULT_DESCRIPTION_LENGTH);
        builder
          .Property(b => b.Author).IsRequired()
             .HasMaxLength(256);
        builder
            .Property(b => b.Picture).IsRequired()
             .HasMaxLength(DataSchemaConstants.DEFAULT_IMAGEURL_LENGTH);
        builder.Property(p => p.PostsStatus)
            .HasConversion(
                p => p.Value,
                p => PostStatus.FromValue(p));

        // one to many
        builder.HasOne(x => x.Classification)
             .WithMany()
             .HasForeignKey(x => x.ClassId)
             .OnDelete(DeleteBehavior.Restrict)
             .IsRequired();
    }
}

public class TagConfig : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder
        .ToTable("Tag", SchemaNames.Posts);

        builder.IsMultiTenant();
        builder
            .Property(b => b.Name).IsRequired()
                .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH);
        builder
            .Property(p => p.NickName)
                .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH);
    }
}

public class PostTagConfig : IEntityTypeConfiguration<PostTag>
{
    public void Configure(EntityTypeBuilder<PostTag> builder)
    {
        builder.ToTable("PostTag", SchemaNames.Posts);
        builder.HasKey(x => new
        {
            x.PostId,
            x.TagId
        });
        builder
            .HasOne(x => x.Post)
            .WithMany(x => x.PostTags)
            .HasForeignKey(x => x.PostId);
        builder
            .HasOne(x => x.Tag)
            .WithMany(x => x.PostTags)
            .HasForeignKey(x => x.TagId);
    }

}

public class CommentConfig : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder
        .ToTable("Comment", SchemaNames.Posts);
        builder.IsMultiTenant();

        builder
            .Property(b => b.Title).IsRequired()
                .HasMaxLength(DataSchemaConstants.DEFAULT_TITLE_LENGTH);
        builder
            .HasOne<Post>().WithMany().HasForeignKey(x => x.PostsId).IsRequired();
        builder
            .Property(p => p.Description)
                .HasMaxLength(DataSchemaConstants.DEFAULT_DESCRIPTION_LENGTH);

        builder
          .Property(b => b.RealName).IsRequired()
              .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH);

        builder
          .Property(b => b.Email).IsRequired()
              .HasMaxLength(256);
    }
}