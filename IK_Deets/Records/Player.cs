// Author: Milan Dierick
// Created: 18/09/2021 21:30
// Solution: IK_Deets

using System;
using IK_Deets.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IK_Deets.Records
{
    /// <inheritdoc cref="IK_Deets.Interfaces.IPlayer" />
    [BsonDiscriminator("player")]
    public record Player : IPlayer
    {
        public Player(string name,
                      string alliance,
                      AllianceRank rank,
                      ObjectId databaseID,
                      ushort server = default,
                      uint troopPower = default,
                      uint highestPower = default,
                      uint defeat = default,
                      uint dismantleDurability = default)
        {
            DatabaseID          = databaseID;
            Name                = name;
            Server              = server;
            Alliance            = alliance;
            TroopPower          = troopPower;
            HighestPower        = highestPower;
            Defeat              = defeat;
            DismantleDurability = dismantleDurability;
            Rank                = rank;
        }

        [BsonId]
        public ObjectId DatabaseID
        {
            get;
            set;
        }

        [BsonElement("name")]
        public string Name
        {
            get;
            set;
        }

        [BsonElement("server")]
        public ushort Server
        {
            get;
            set;
        }

        [BsonElement("alliance")]
        public string Alliance
        {
            get;
            set;
        }
        
        [BsonElement("rank")]
        public AllianceRank Rank
        {
            get;
            set;
        }

        [BsonElement("troop_power")]
        public uint TroopPower
        {
            get;
            set;
        }

        [BsonElement("lord_power")]
        public uint HighestPower
        {
            get;
            set;
        }

        [BsonElement("tech_contributions")]
        public uint TechContributions
        {
            get;
            set;
        }

        [BsonElement("defeat")]
        public uint Defeat
        {
            get;
            set;
        }

        [BsonElement("dismantle")]
        public uint DismantleDurability
        {
            get;
            set;
        }

        [BsonElement("time")]
        public DateTime SubmissionDateTime
        {
            get;
            set;
        }
    }
}