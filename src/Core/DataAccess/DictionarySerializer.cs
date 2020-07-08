using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TeaBagBot.DataAccess
{
    public class DictionarySerializer<TDictionary, KeySerializer, ValueSerializer> : DictionarySerializerBase<TDictionary>
    where TDictionary : class, IDictionary, new()
    where KeySerializer : IBsonSerializer, new()
    where ValueSerializer : IBsonSerializer, new()
    {
        public DictionarySerializer() : base(DictionaryRepresentation.ArrayOfDocuments, new KeySerializer(), new ValueSerializer())
        {
        }

        protected override TDictionary CreateInstance()
        {
            return new TDictionary();
        }
    }

    public class EnumStringSerializer<TEnum> : EnumSerializer<TEnum>
        where TEnum : struct
    {
        public EnumStringSerializer() : base(BsonType.String) { }
    }
}
