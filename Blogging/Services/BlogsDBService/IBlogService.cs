using Blogging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Blogging.Services
{
    public interface IBlogService
    {
        Task<HttpResponseMessage> GetAllBlogs();

        Task<HttpResponseMessage> AddBlog(string title, string blogData, string userId);

        Task<HttpResponseMessage> EditBlog(string blogId, string title, string blogData, string userId);

        Task<HttpResponseMessage> DeleteBlog(string blogId, string userId);
    }
}
