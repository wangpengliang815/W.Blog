namespace W.Blog.BLL.Implement
{
    using W.Blog.Dal.DbContexts;
    using W.Blog.DAL.Implement;
    using W.Blog.Entity.Entitys;

    public interface IArticleBLL : IBaseBLL<ArticleEntity, IArticleDAL, ApplicationDbContext>
    {

    }

    public class ArticleBLL : BaseBLL<ArticleEntity, IArticleDAL, ApplicationDbContext>,
         IArticleBLL
    {
        public ArticleBLL(IArticleDAL dal) : base(dal) { }
    }
}
