﻿using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Etdb.ServiceBase.DocumentRepository.Abstractions.Context
{
    public abstract class DocumentDbContext
    {
        private const string CamelCase = "CamelCase";

        protected DocumentDbContext(IOptions<DocumentDbContextOptions> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            
            this.Database = client.GetDatabase(options.Value.DatabaseName);
        }

        public abstract void Configure();

        protected bool CollectionExists(string collectionName)
        {
            var filter = new BsonDocument("name", collectionName);

            var collections = this.Database.ListCollections(new ListCollectionsOptions
            {
                Filter = filter
            });

            return collections.Any();
        }

        protected void CreateCollection(string collectionName, CreateCollectionOptions options = null)
        {
            this.Database.CreateCollection(collectionName, options);
        }

        protected static void UseCamelCaseConvention()
        {
            ConventionRegistry.Register(DocumentDbContext.CamelCase, 
                new ConventionPack { new CamelCaseElementNameConvention() }, 
                type => true);
        }

        protected static CreateCollectionOptions AutoIndexIdCollectionOptions()
        {
            return new CreateCollectionOptions
            {
                AutoIndexId = true
            };
        }
            
        
        public IMongoDatabase Database { get; }
    }
}