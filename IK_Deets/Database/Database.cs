// Author: Milan Dierick
// Created: 19/09/2021 05:28
// Solution: IK_Deets

using System;
using System.Data.SqlClient;
using IK_Deets.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace IK_Deets.Database
{
    public class Database : IDatabase
    {
        private readonly MongoClient _client;

        public Database()
        {
            _client
                = new
                    MongoClient("mongodb+srv://dbUser:fallenover@serverlessinstance0.5d4xq.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");
        }

        public void Connect()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public IMongoDatabase GetDatabase(string name)
        {
            return _client.GetDatabase(name);
        }

        public IMongoCollection<T> GetCollection<T>(string database, string collection)
        {
            return _client.GetDatabase(database).GetCollection<T>(collection);
        }
    }
}