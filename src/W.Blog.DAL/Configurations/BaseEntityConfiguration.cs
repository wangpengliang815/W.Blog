namespace W.Blog.Dal.Configurations
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using W.Blog.Entity.Entitys;
    using W.Blog.Entity.Enums;

    public class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T>
        where T : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.IsDelete)
                .IsRequired()
                .HasDefaultValue(DeleteEnum.Normal);

            builder.Property(p => p.CreateTime)
                .IsRequired()
                .HasDefaultValue(DateTime.Now);
        }
    }
}
