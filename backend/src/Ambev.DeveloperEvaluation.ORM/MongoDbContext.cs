﻿using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.ORM
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName) => _database.GetCollection<T>(collectionName);
    }
}
