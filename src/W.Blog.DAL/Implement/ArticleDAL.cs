namespace W.Blog.DAL.Implement
{
    using W.Blog.Dal.DbContexts;
    using W.Blog.Entity.Entitys;

    public interface IArticleDAL : IBaseDAL<ArticleEntity, ApplicationDbContext>
    {

    }

    public class ArticleDAL : BaseDAL<ArticleEntity, ApplicationDbContext>
         , IArticleDAL
    {
        public ArticleDAL(ApplicationDbContext dbContext) : base(dbContext) { }
    }
}
