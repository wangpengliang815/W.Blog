namespace W.Blog.Api.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ArticleVM
    {
        public int Id { get; set; }

        [Display(Name = "文章标题")]
        [Required(ErrorMessage = "必须输入文章标题")]
        public string Title { get; set; }

        [Display(Name = "文章内容")]
        [Required(ErrorMessage = "必须输入文章内容")]
        public string Content { get; set; }

        [Display(Name = "类别编号")]
        [Required(ErrorMessage = "必须输入文章所属类别编号")]
        public int CategoryId { get; set; }

        [Display(Name = "是否置顶显示")]
        [Required(ErrorMessage = "必须输入是否置顶显示")]
        public bool IsImportance { get; set; }


        [Display(Name = "是否完结状态")]
        [Required(ErrorMessage = "必须输入是否完结状态")]
        public bool IsEnd { get; set; }
    }
}
