namespace W.Blog.DAL.Implement
{
    using W.Blog.Dal.DbContexts;
    using W.Blog.Entity.Entitys;

    public interface ICategoryDAL : IBaseDAL<CategoryEntity, ApplicationDbContext>
    {

    }

    public class CategoryDAL : BaseDAL<CategoryEntity, ApplicationDbContext>
         , ICategoryDAL
    {
        public CategoryDAL(ApplicationDbContext dbContext) : base(dbContext) { }
    }
}
