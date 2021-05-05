using Blogging.Models;
using Blogging.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blogging.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        public BlogController(IBlogService service)
        {
            _service = service;
        }

        [HttpPost("AddNewBlog")]
        public async Task<IActionResult> AddNewBlog([FromBody] BlogDetails blogDetails)
        {
            var response = await _service.AddBlog(blogDetails.Title, blogDetails.BlogValue, blogDetails.UserId);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var blog = JsonConvert.DeserializeObject<Blog>(content);
                return Ok(blog);
            }

            return BadRequest(content);
        }

        [HttpPost("EditBlog")]
        public async Task<IActionResult> EditBlog([FromBody] BlogDetails blogDetails, [FromQuery] string blogId)
        {
            var response = await _service.EditBlog(blogId, blogDetails.Title, blogDetails.BlogValue, blogDetails.UserId);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var blog = JsonConvert.DeserializeObject<Blog>(content);
                return Ok(blog);
            }

            return BadRequest(content);
        }

        [HttpDelete("DeleteBlog")]
        public async Task<IActionResult> DeleteBlog([FromQuery] string blogId, [FromQuery] string userId)
        {
            var response = await _service.DeleteBlog(blogId, userId);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return Ok(content);
            }

            return BadRequest(content);
        }

        [HttpGet("GetAllBlogs")]
        public async Task<IActionResult> GetAllBlogs()
        {
            var response = await _service.GetAllBlogs();
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var blogs = JsonConvert.DeserializeObject<List<DisplayBlog>>(content);
                return Ok(blogs);
            }

            return BadRequest(content);
        }

        private IBlogService _service;
    }
}
