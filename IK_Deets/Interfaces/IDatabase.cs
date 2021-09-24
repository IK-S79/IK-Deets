// Author: Milan Dierick
// Created: 19/09/2021 05:22
// Solution: IK_Deets

using System.Data.SqlClient;
using MongoDB.Bson;
using MongoDB.Driver;

namespace IK_Deets.Interfaces
{
    public interface IDatabase
    {
        public void Connect();

        public void Close();

        public IMongoDatabase GetDatabase(string database);

        public IMongoCollection<T> GetCollection<T>(string database, string collection);
    }
}