namespace GraphQL.Core.Entities
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string CategoryId { get; set; }
    }
}
