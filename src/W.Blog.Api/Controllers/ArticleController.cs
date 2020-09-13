namespace W.Blog.Api.Controllers
{
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using W.Blog.Api.Models;
    using W.Blog.BLL.Implement;
    using W.Blog.Entity.Entitys;
    using W.Blog.Entity.Enums;

    /// <summary>
    /// 类别相关
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ArticleController : BaseApiController
    {
        public ArticleController(
            ICategoryBLL categoryBLL,
            IArticleBLL articleBLL,
            IMapper mapper)
        {
            CategoryBLL = categoryBLL;
            ArticleBLL = articleBLL;
            Mapper = mapper;
        }

        /// <summary>
        /// 查询集合
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> Get()
        {
            ApiResult apiResult = new ApiResult()
            {
                Success = true,
                Data = await ArticleBLL.GetListAsync()
            };
            return apiResult;
        }

        /// <summary>
        /// 根据Id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "ArticleGet")]
        public async Task<ApiResult> Get(int id)
        {
            ArticleEntity entity = await ArticleBLL.FindAsync(id);
            if (entity != null)
            {
                return new ApiResult
                {
                    Success = true,
                    Data = entity
                };
            }
            else
            {
                return new ApiResult
                {
                    Message = "信息不存在"
                };
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> Post([FromBody] ArticleVM model)
        {
            if (!ModelState.IsValid)
                return new ApiResult
                {
                    Message = JsonConvert.SerializeObject(BadRequest(ModelState))
                };
            else
            {
                CategoryEntity category = await CategoryBLL.FindAsync(model.CategoryId);
                if (category != null)
                {
                    ArticleEntity entity = Mapper.Map<ArticleVM, ArticleEntity>(model);
                    return new ApiResult
                    {
                        Success = true,
                        Data = await ArticleBLL.InsertAsync(entity)
                    };
                }
                else
                {
                    return new ApiResult
                    {
                        Message = "类别信息不存在"
                    };
                }
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ApiResult> Put([FromBody] ArticleVM model)
        {
            ApiResult apiResult = new ApiResult();
            if (!ModelState.IsValid)
                apiResult.Message = JsonConvert.SerializeObject(BadRequest(ModelState));
            else
            {
                CategoryEntity category = await CategoryBLL.FindAsync(model.CategoryId);
                if (category != null)
                {
                    ArticleEntity entity = await ArticleBLL.GetByIdAsync(model.Id);
                    if (entity != null)
                    {
                        entity = Mapper.Map<ArticleEntity>(model);
                        apiResult.Success = true;
                        apiResult.Data = await ArticleBLL.UpdateAsync(entity);
                    }
                }
                else
                {
                    apiResult.Message = "类别信息不存在";
                }
            }
            return apiResult;
        }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ApiResult> Delete(int id)
        {
            ArticleEntity entity = await ArticleBLL.FindAsync(id);
            if (entity != null)
            {
                entity.IsDelete = DeleteEnum.Delete;
                return new ApiResult
                {
                    Success = true,
                    Data = await ArticleBLL.UpdateAsync(entity)
                };
            }
            else
            {
                return new ApiResult
                {
                    Message = "信息不存在"
                };
            }
        }
    }
}
