namespace W.Blog.Dal.DbContexts
{
    using Microsoft.EntityFrameworkCore;
    using W.Blog.Dal.Configurations;
    using W.Blog.Entity.Entitys;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {

        }

        public DbSet<CategoryEntity> CategoryEntities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CategoryEntityConfiguration());
        }
    }
}
