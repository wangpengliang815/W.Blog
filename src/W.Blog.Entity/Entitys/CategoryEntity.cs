namespace W.Blog.Entity.Entitys
{
    /// <summary>
    /// 类别分类
    /// </summary>
    public class CategoryEntity : BaseEntity
    {
        public string Name { get; set; }

        public string ParentId { get; set; }

        public int Order { get; set; }
    }
}
