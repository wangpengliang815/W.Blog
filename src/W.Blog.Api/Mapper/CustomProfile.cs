namespace W.Blog.Api.Mapper
{
    using W.Blog.Api.Models;
    using W.Blog.Entity.Entitys;
    using AutoMapper;

    public class CustomProfile : Profile
    {
        public CustomProfile()
        {
            CreateMap<CategoryVM, CategoryEntity>().ReverseMap();

            CreateMap<ArticleVM, ArticleEntity>().ReverseMap();
        }
    }
}
