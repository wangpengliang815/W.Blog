namespace W.Blog.BLL.Implement
{
    using W.Blog.Dal.DbContexts;
    using W.Blog.DAL.Implement;
    using W.Blog.Entity.Entitys;

    public interface ICategoryBLL : IBaseBLL<CategoryEntity, ICategoryDAL, ApplicationDbContext>
    {

    }

    class CategoryBLL: BaseBLL<CategoryEntity, ICategoryDAL, ApplicationDbContext>,
        ICategoryBLL
    {
        public CategoryBLL(ICategoryDAL dal) : base(dal) { }
    }
}
