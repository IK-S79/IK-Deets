// Author: Milan Dierick
// Created: 18/09/2021 05:15
// Solution: IK_Deets

using System;
using IK_Deets.Records;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;

namespace IK_Deets.Interfaces
{
    public enum AllianceRank
    {
        Leader = 0,
        Officer = 1,
        Backbones = 2,
        Elites = 3,
        Members = 4,
        Newbies = 5
    }

    [BsonSerializer(typeof(ImpliedImplementationInterfaceSerializer<IPlayer, Player>))]
    public interface IPlayer
    {
        [BsonId]
        ObjectId DatabaseID
        {
            get;
            set;
        }
        
        [BsonElement("name")]
        string Name
        {
            get;
            set;
        }

        [BsonElement("server")]
        ushort Server
        {
            get;
            set;
        }

        [BsonElement("alliance")]
        string Alliance
        {
            get;
            set;
        }

        [BsonElement("rank")]
        AllianceRank Rank
        {
            get;
            set;
        }
        
        [BsonElement("troop_power")]
        uint TroopPower
        {
            get;
            set;
        }

        [BsonElement("lord_power")]
        uint HighestPower
        {
            get;
            set;
        }

        [BsonElement("tech_contributions")]
        uint TechContributions
        {
            get;
            set;
        }
        
        [BsonElement("defeat")]
        uint Defeat
        {
            get;
            set;
        }

        
        [BsonElement("dismantle")]
        uint DismantleDurability
        {
            get;
            set;
        }

        [BsonElement("time")]
        DateTime SubmissionDateTime
        {
            get;
            set;
        }
    }
}