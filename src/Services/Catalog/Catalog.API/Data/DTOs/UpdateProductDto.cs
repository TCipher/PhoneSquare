using Catalog.API.Data.DTOs.Common;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.API.Data.DTOs
{
    public class UpdateProductDto : BaseDto
    {
        [BsonElement("Name")]
        public string Name { get; set; }
        public string Category { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string ImageFile { get; set; }
        public decimal Price { get; set; }
    }
}
