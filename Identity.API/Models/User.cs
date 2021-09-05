using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Identity.API.Models
{
    public class User
    {
        [BsonId]
        public Guid Id { get; set; }
        
        public string Email { get; set; }
        
        public string? GoogleId { get; set; }
    }
}