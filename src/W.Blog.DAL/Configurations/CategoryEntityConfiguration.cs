namespace W.Blog.Dal.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using W.Blog.Entity.Entitys;

    public class CategoryEntityConfiguration : BaseEntityConfiguration<CategoryEntity>,
        IEntityTypeConfiguration<CategoryEntity>
    {
        public override void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {
            builder.ToTable("T_Category");

            builder.Property(p => p.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(p => p.ParentId)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.Order)
                .IsRequired();
        }
    }
}
