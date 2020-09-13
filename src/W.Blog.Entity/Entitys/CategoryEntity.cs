namespace W.Blog.Entity.Entitys
{
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 类别分类
    /// </summary>
    [Table("T_Category")]
    public class CategoryEntity : BaseEntity
    {
        public string Name { get; set; }

        public string ParentId { get; set; }

        public int Order { get; set; }
    }
}
