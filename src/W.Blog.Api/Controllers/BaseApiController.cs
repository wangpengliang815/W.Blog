namespace W.Blog.Api.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using W.Blog.BLL.Implement;

    public class BaseApiController : ControllerBase
    {
        protected IMapper Mapper { get; set; }

        protected ICategoryBLL CategoryBLL { get; set; }

        protected IArticleBLL ArticleBLL { get; set; }
    }

    public class ApiResult
    {
        public bool Success { get; set; } = false;

        public string Message { get; set; }

        public object Data { get; set; }
    }
}
