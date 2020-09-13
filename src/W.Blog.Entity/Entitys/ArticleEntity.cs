namespace W.Blog.Entity.Entitys
{
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 文章
    /// </summary>
    [Table("T_Article")]
    public class ArticleEntity : BaseEntity
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 类别编号
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// 是否置顶显示
        /// </summary>
        public bool IsImportance { get; set; }

        /// <summary>
        /// 是否完结
        /// </summary>
        public bool IsEnd { get; set; }
    }
}
