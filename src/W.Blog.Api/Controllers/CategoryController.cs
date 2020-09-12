namespace W.Blog.Api.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using W.Blog.BLL.Implement;

    /// <summary>
    /// 类别相关
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : BaseApiController
    {
        public CategoryController(ICategoryBLL categoryBLL)
        {
            _categoryBLL = categoryBLL;
        }

        [HttpGet]
        public async Task<ApiResult> Get()
        {
            var apiResult = new ApiResult()
            {
                Success = true,
                Data = await _categoryBLL.GetListAsync()
            };
            return apiResult;
        }

        // GET: api/User/5
        [HttpGet("{id}", Name = "CategoryGet")]
        public async Task<ApiResult> Get(int id)
        {
            var entity = await _categoryBLL.FindAsync(id);
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

        ///// <summary>
        ///// 新增
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        ////POST: api/User
        //[HttpPost]
        //public async Task<ApiResult> Post([FromBody] CategoryInputModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return new ApiResult
        //        {
        //            Message = JsonConvert.SerializeObject(BadRequest(ModelState))
        //        };
        //    else
        //    {
        //        var entity = _mapper.Map<CategoryInputModel, CategoryEntity>(model);
        //        return new ApiResult
        //        {
        //            Success = true,
        //            Data = await _categoryBLL.AddOrUpdate(entity)
        //        };
        //    }
        //}

        ///// <summary>
        ///// 更新
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //// PUT: api/User/5
        //[HttpPut("{id}")]
        //public async Task<ApiResult> Put(int id, [FromBody] CategoryInputModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return new ApiResult
        //        {
        //            Message = JsonConvert.SerializeObject(BadRequest(ModelState))
        //        };
        //    else
        //    {
        //        var entity = await _categoryBLL.Get(id);
        //        if (entity != null)
        //        {
        //            entity.Name = model.Name;
        //            return new ApiResult
        //            {
        //                Success = true,
        //                Data = await _categoryBLL.AddOrUpdate(entity)
        //            };
        //        }
        //        else
        //        {
        //            return new ApiResult
        //            {
        //                Message = "信息不存在"
        //            };
        //        }
        //    }
        //}

        ///// <summary>
        ///// 删除
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpDelete("{id}")]
        //public async Task<ApiResult> Delete(int id)
        //{
        //    var entity = await _categoryBLL.Get(id);
        //    if (entity != null)
        //    {
        //        return new ApiResult
        //        {
        //            Success = true,
        //            Data = await _categoryBLL.Delete(id)
        //        };
        //    }
        //    else
        //    {
        //        return new ApiResult
        //        {
        //            Message = "信息不存在"
        //        };
        //    }
        //}
    }
}
