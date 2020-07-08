using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Discord;
using Microsoft.VisualBasic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TeaBagBot.DataAccess;
using TeaBagBot.DataAccess.Models;
using Xunit;

namespace TeaBagBot.UnitTests
{
    [BsonCollection("Models")]
    public class TestModel : EntityBase
    {
        public string Name { get; set; }
        public IList<CommandPermission> List { get; set; }
    }

    public class MongoRepositoryTests
    {
        private readonly Mock<MongoRepository<TestModel>> _repositoryMock;

        public MongoRepositoryTests()
        {
            var settingsStub = new Mock<IMongoDbSettings>();
            settingsStub.SetupGet(x => x.DatabaseName).Returns("TestDB");

            var collectionStub = new Mock<IMongoCollection<TestModel>>();

            var databaseStub = new Mock<IMongoDatabase>();
            databaseStub.Setup(x => x.GetCollection<TestModel>("Models", null)).Returns(collectionStub.Object);

            var clientStub = new Mock<IMongoClient>();
            clientStub.Setup(x => x.GetDatabase("TestDB", null)).Returns(databaseStub.Object);

            _repositoryMock = new Mock<MongoRepository<TestModel>>(clientStub.Object, settingsStub.Object);
        }

        [Fact]
        public async Task FindOneAsync_TestModel_ShouldWork()
        {
            var expected = new TestModel
            {
                Name = "test",
                List = new List<CommandPermission>()
                {
                    new CommandPermission() { Command = "test", Permissions = GuildPermission.Administrator }
                }
            };
            var bsonDocument = expected.ToBsonDocument();
            _repositoryMock.Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<TestModel, bool>>>()))
                .Returns(Task.FromResult(BsonSerializer.Deserialize<TestModel>(bsonDocument)));
            var repository = _repositoryMock.Object;
            await repository.InsertOneAsync(expected);
            var actual = await repository.FindOneAsync(x => x.Name == expected.Name);

            Assert.NotNull(actual);
            Assert.Equal(expected.Name, actual.Name);
        }
    }
}
