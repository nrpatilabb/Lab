using Blogging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Blogging.Services.UsersDBService
{
    public interface IUsersService
    {
        Task<string> Login(string email, string password);

        Task<HttpResponseMessage> CreateUser(string userName, string password);

        Task<HttpResponseMessage> GetUser(string userId);
    }
}
