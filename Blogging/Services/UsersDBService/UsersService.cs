using Blogging.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Blogging.Services.UsersDBService
{
    public class UsersService: IUsersService
    {
        public UsersService(IDBSettings dBSettings, IConfiguration configuration)
        {
            var client = new MongoClient(dBSettings.ConnectionString);
            var db = client.GetDatabase(dBSettings.DatabaseName);
            _userCollection = db.GetCollection<User>(dBSettings.UsersCollectionName);
            _key = configuration.GetSection("JWTKey").ToString();
        }

        public async Task<string> Login(string email, string password)
        {
            User user;
            try
            {
                var response = await _userCollection.FindAsync(o => o.Email == email && o.Password == password);
                user = await response.FirstOrDefaultAsync();
            }
            catch
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_key);
            var descriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, email)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                            new SymmetricSecurityKey(key),
                            SecurityAlgorithms.HmacSha256Signature
                        )
            };

            var token = tokenHandler.CreateToken(descriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<HttpResponseMessage> CreateUser(string email, string password)
        {
            var user = new User
            {
                _id = Guid.NewGuid().ToString(),
                Email = email,
                Password = password
            };
            try
            {
                await _userCollection.InsertOneAsync(user);
                return new HttpResponseMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(user)),
                    ReasonPhrase = string.Empty,
                    StatusCode = HttpStatusCode.Created
                };
            }
            catch(Exception ex)
            {
                return new HttpResponseMessage
                {
                    Content = new StringContent("Failed to create user"),
                    ReasonPhrase = ex.Message,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }

        public async Task<HttpResponseMessage> GetUser(string userId)
        {
            try
            {
                var user = await _userCollection.FindAsync(o => o._id == userId);
                var result = await user.FirstOrDefaultAsync();
                return new HttpResponseMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(result)),
                    ReasonPhrase = string.Empty,
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage
                {
                    Content = new StringContent("User not found"),
                    ReasonPhrase = ex.Message,
                    StatusCode = HttpStatusCode.NotFound
                };
            }
        }

        private IMongoCollection<User> _userCollection;
        private readonly string _key;
    }
}
