using BlogMaster.Model;
using BlogMaster.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace BlogMaster.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogMasterController : ControllerBase
    {
        private readonly IBlogMaster _blog;

        public BlogMasterController(IBlogMaster blog)
        {
            _blog = blog;
        }

        [HttpPost("GetBlogs")]
        public ActionResult<GenericResponse<object>> GetBlogs([FromBody] SearchParams searchParams)
        {
            if (searchParams.Id.HasValue)
            {
                var blog = _blog.GetBlogById(searchParams.Id.Value);
                if (blog == null)
                {
                    return NotFound(new GenericResponse<object>
                    {
                        StatusMessage = "Blog not found",
                        Data = null,
                        StatusCode = 404
                    });
                }

                return Ok(new GenericResponse<object>
                {
                    StatusMessage = "Blog retrieved successfully",
                    Data = blog,
                    StatusCode = 200
                });
            }
            else
            {
                var allBlogs = _blog.GetAllBlogs(searchParams.SearchTerm, searchParams.SortBy, searchParams.Direction);
                var paginatedBlogs = allBlogs.Skip((searchParams.PageNumber - 1) * searchParams.PageSize).Take(searchParams.PageSize).ToList();
                var totalItems = allBlogs.Count();

                var result = new PaginatedResult<Blog> 
                {
                    Items = paginatedBlogs,
                    TotalCount = totalItems,
                    PageSize = searchParams.PageSize,
                    PageNumber = searchParams.PageNumber
                };

                return Ok(new GenericResponse<PaginatedResult<Blog>>
                {
                    StatusMessage = "Blogs retrieved successfully",
                    Data = result,  
                    StatusCode = 200
                });
            }
        }

        [HttpPost("AddOrUpdateBlog")]
        public ActionResult<GenericResponse<object>> AddOrUpdateBlog(Blog blog)
        {
            _blog.AddOrUpdateBlog(blog);
            return Ok(new GenericResponse<object>
            {
                StatusMessage = "Blog Created successfully",
                Data = blog,
                StatusCode = 200
            });
        }

        [HttpDelete("DeleteBlog")]
        public IActionResult DeleteBlog(int id)
        {
            var blog = _blog.GetBlogById(id);

            if (blog == null)
            {
                return NotFound(new GenericResponse<object>
                {
                    StatusMessage = "Blog not found",
                    Data = null,
                    StatusCode = 404
                });
            }

            _blog.RemoveBLog(blog.Id);
            return Ok(new GenericResponse<object>
            {
                StatusMessage = "Blog removed successfully",
                Data = blog,
                StatusCode = 200
            });
        }
    }
}

