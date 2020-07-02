using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace TeaBagBot.DataAccess.Models
{
    public abstract class EntityBase : IEntity
    {
        public ObjectId Id { get; set; }
    }
}
