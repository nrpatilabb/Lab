using Blogging.Models;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Blogging.Services
{
    public class BlogService : IBlogService
    {
        public BlogService(IDBSettings dBSettings)
        {
            var client = new MongoClient(dBSettings.ConnectionString);
            var db = client.GetDatabase(dBSettings.DatabaseName);
            _blogCollection = db.GetCollection<Blog>(dBSettings.BlogCollectionName);
        }

        public async Task<HttpResponseMessage> AddBlog(string title, string blogData, string userId)
        {
            try
            {
                var blog = new Blog
                {
                    _id = Guid.NewGuid().ToString(),
                    Title = title,
                    BlogValue = blogData,
                    UserId = userId
                };

                await _blogCollection.InsertOneAsync(blog);
                var content = JsonConvert.SerializeObject(blog);
                return new HttpResponseMessage
                {
                    Content = new StringContent(content),
                    ReasonPhrase = "Blog created successfully",
                    StatusCode = HttpStatusCode.Created
                };
            }
            catch(Exception ex)
            {
                return new HttpResponseMessage
                {
                    Content = new StringContent("Failed to create blog"),
                    ReasonPhrase = ex.Message,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }

        public async Task<HttpResponseMessage> DeleteBlog(string blogId, string userId)
        {
            try
            {
                var response = await _blogCollection.FindAsync(_ => _._id == blogId);
                var blog = await response.FirstOrDefaultAsync();
                if (blog.UserId != userId)
                {
                    return new HttpResponseMessage
                    {
                        Content = new StringContent("Unauthorised to delete this blog"),
                        ReasonPhrase = string.Empty,
                        StatusCode = HttpStatusCode.Unauthorized
                    };
                }
                await _blogCollection.DeleteOneAsync(o => o._id == blogId);
                return new HttpResponseMessage
                {
                    Content = new StringContent("Blog deleted successfully"),
                    ReasonPhrase = string.Empty,
                    StatusCode = HttpStatusCode.NoContent
                };
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage
                {
                    Content = new StringContent("Failed to delete blog"),
                    ReasonPhrase = ex.Message,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }

        public async Task<HttpResponseMessage> EditBlog(string blogId, string title, string blogData, string userId)
        {
            try
            {
                var response = await _blogCollection.FindAsync(_ => _._id == blogId);
                var oldBlog = await response.FirstOrDefaultAsync();
                if(oldBlog.UserId != userId)
                {
                    return new HttpResponseMessage
                    {
                        Content = new StringContent("Unauthorised to edit this blog"),
                        ReasonPhrase = string.Empty,
                        StatusCode = HttpStatusCode.Unauthorized
                    };
                }
                var blog = new Blog
                {
                    _id = blogId,
                    Title = title,
                    BlogValue = blogData,
                    UserId = oldBlog.UserId
                };
                await _blogCollection.FindOneAndReplaceAsync(o => o._id == blogId, blog);
                return new HttpResponseMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(blog)),
                    ReasonPhrase = "Blog updated successfully",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage
                {
                    Content = new StringContent("Failed to update blog"),
                    ReasonPhrase = ex.Message,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }

        public async Task<HttpResponseMessage> GetAllBlogs()
        {
            try
            {
                var blogs = await _blogCollection.FindAsync(_ => true);
                var result = blogs.ToList();
                var userBlogs = new List<DisplayBlog>();
                foreach (var blog in result)
                {
                    var userBlog = new DisplayBlog
                    {
                        _id = blog._id,
                        Title = blog.Title,
                        BlogValue = blog.BlogValue
                    };
                    userBlogs.Add(userBlog);
                }
                return new HttpResponseMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(userBlogs)),
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch(Exception ex)
            {
                return new HttpResponseMessage
                {
                    Content = new StringContent("Failed to get all blogs"),
                    ReasonPhrase = ex.Message,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }

        private IMongoCollection<Blog> _blogCollection;
    }
}
