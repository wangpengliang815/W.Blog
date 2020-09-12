using Microsoft.AspNetCore.Mvc;
using W.Blog.BLL.Implement;

namespace W.Blog.Api.Controllers
{
    public class BaseApiController : ControllerBase
    {
        protected ICategoryBLL _categoryBLL { get; set; }
    }

    public class ApiResult
    {
        public bool Success { get; set; } = false;

        public string Message { get; set; }

        public object Data { get; set; }
    }
}
