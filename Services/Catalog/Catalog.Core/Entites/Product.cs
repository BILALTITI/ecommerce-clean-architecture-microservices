using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Core.Entites
{
 public class Product: BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        [MongoDB.Bson.Serialization.Attributes.BsonRepresentation(MongoDB.Bson.BsonType.Decimal128)]
        public decimal Price { get; set; } 

        public productBrand Type { get; set; }
        public productType Brand { get; set; }
        public string ImageFile{ get; set; }

    }
}
