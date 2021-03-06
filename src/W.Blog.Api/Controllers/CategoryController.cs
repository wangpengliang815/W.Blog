﻿namespace W.Blog.Api.Controllers
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
    public class CategoryController : BaseApiController
    {
        public CategoryController(
            ICategoryBLL categoryBLL,
            IMapper mapper)
        {
            CategoryBLL = categoryBLL;
            Mapper = mapper;
        }

        /// <summary>
        /// 查询集合
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> Get()
        {
            var apiResult = new ApiResult()
            {
                Success = true,
                Data = await CategoryBLL.GetListAsync()
            };
            return apiResult;
        }

        /// <summary>
        /// 根据Id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "CategoryGet")]
        public async Task<ApiResult> Get(int id)
        {
            var entity = await CategoryBLL.FindAsync(id);
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
        public async Task<ApiResult> Post([FromBody] CategoryVM model)
        {
            if (!ModelState.IsValid)
                return new ApiResult
                {
                    Message = JsonConvert.SerializeObject(BadRequest(ModelState))
                };
            else
            {
                var entity = Mapper.Map<CategoryVM, CategoryEntity>(model);
                return new ApiResult
                {
                    Success = true,
                    Data = await CategoryBLL.InsertAsync(entity)
                };
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ApiResult> Put([FromBody] CategoryVM model)
        {
            if (!ModelState.IsValid)
                return new ApiResult
                {
                    Message = JsonConvert.SerializeObject(BadRequest(ModelState))
                };
            else
            {
                var entity = await CategoryBLL.GetByIdAsync(model.Id);
                if (entity != null)
                {
                    entity = Mapper.Map<CategoryEntity>(model);
                    return new ApiResult
                    {
                        Success = true,
                        Data = await CategoryBLL.UpdateAsync(entity)
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

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ApiResult> Delete(int id)
        {
            var entity = await CategoryBLL.FindAsync(id);
            if (entity != null)
            {
                entity.IsDelete = DeleteEnum.Delete;
                return new ApiResult
                {
                    Success = true,
                    Data = await CategoryBLL.UpdateAsync(entity)
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
