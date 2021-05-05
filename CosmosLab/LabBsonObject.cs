using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CosmosLab
{
    class LabBsonObject
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string _id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("address")]
        public string Address { get; set; }
    }
}
