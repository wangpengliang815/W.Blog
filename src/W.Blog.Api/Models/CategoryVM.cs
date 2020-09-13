namespace W.Blog.Api.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CategoryVM
    {
        public int Id { get; set; }

        [Display(Name = "类别名称")]
        [Required(ErrorMessage = "必须输入类别名称")]
        public string Name { get; set; }

        [Display(Name = "类别所属父级编码")]
        [Required(ErrorMessage = "必须输入类别所属父级编码")]
        public int ParentId { get; set; }
    }
}
