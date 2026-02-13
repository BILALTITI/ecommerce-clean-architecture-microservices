using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Core.Entites
{
    public class productType: BaseEntity
    {
        //[BsonElement("name")] //to map the property to the "name" field in MongoDB will be stored as "name"
        public string Name { get; set; }
    }
}