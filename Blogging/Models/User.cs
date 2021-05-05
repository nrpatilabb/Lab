using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blogging.Models
{
    public class UserDetails
    {
        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }
    }
    public class User: UserDetails
    {
        [BsonId]
        public string _id { get; set; }
    }
}
