// Author: Milan Dierick
// Created: 18/09/2021 21:46
// Solution: IK_Deets

using System;
using System.Collections.Concurrent;
using System.Linq;
using IK_Deets.Interfaces;
using MongoDB.Bson;

namespace IK_Deets.Records
{
    public class MemberDistribution : IMemberDistribution
    {
        public MemberDistribution(IAlliance alliance)
        {
            Alliance = alliance;
        }

        public IAlliance Alliance
        {
            get;
        }

        public int R6 => Alliance.Players?.Values.Count(player => player.Rank == AllianceRank.Leader) ?? 0;
        public int R5 => Alliance.Players?.Values.Count(player => player.Rank == AllianceRank.Officer) ?? 0;
        public int R4 => Alliance.Players?.Values.Count(player => player.Rank == AllianceRank.Backbones) ?? 0;
        public int R3 => Alliance.Players?.Values.Count(player => player.Rank == AllianceRank.Elites) ?? 0;
        public int R2 => Alliance.Players?.Values.Count(player => player.Rank == AllianceRank.Members) ?? 0;
        public int R1 => Alliance.Players?.Values.Count(player => player.Rank == AllianceRank.Newbies) ?? 0;
    }

    /// <inheritdoc cref="IK_Deets.Interfaces.IAlliance" />
    public record Alliance : IAlliance
    {
        private string? _tag;

        public Alliance(string name,
                        string? tag,
                        ushort server,
                        ObjectId databaseID = default,
                        ConcurrentDictionary<string, IPlayer> players = null!)
        {
            DatabaseID         = databaseID;
            Name               = name;
            Tag                = tag;
            Server             = server;
            Players            = players;
            MemberDistribution = new MemberDistribution(this);
        }

        public ObjectId DatabaseID
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        // TODO: Check if the in-game tags can contain square brackets
        public string? Tag
        {
            get => _tag;
            set
            {
                if (value == null) return;
                
                if (value.StartsWith("[") || value.EndsWith("]"))
                {
                    _tag = '[' + value + ']';
                }
                else
                {
                    _tag = value;
                }
            }
        }

        public ushort Server
        {
            get;
        }

        public ConcurrentDictionary<string, IPlayer>? Players
        {
            get;
            set;
        }

        public int MemberCount => Players?.Count ?? 0;

        public IMemberDistribution MemberDistribution
        {
            get;
        }
    }
}