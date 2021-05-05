using MongoDB.Bson.Serialization.Attributes;

namespace Blogging.Models
{
    public class BlogDetails
    {
        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("blogValue")]
        public string BlogValue { get; set; }
    }
    public class Blog: BlogDetails
    {
        [BsonId]
        public string _id { get; set; }
    }

    public class DisplayBlog
    {
        [BsonElement("_id")]
        public string _id { get; set; }
        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("blogValue")]
        public string BlogValue { get; set; }
    }
}
