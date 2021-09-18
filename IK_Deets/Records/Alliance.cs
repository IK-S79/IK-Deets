// Author: Milan Dierick
// Created: 18/09/2021 21:46
// Solution: IK_Deets

using System;
using System.Collections.Concurrent;
using System.Linq;
using IK_Deets.Interfaces;

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

        public int R6 => Alliance.Players.Values.Count(player => player.Rank == AllianceRank.R6);
        public int R5 => Alliance.Players.Values.Count(player => player.Rank == AllianceRank.R5);
        public int R4 => Alliance.Players.Values.Count(player => player.Rank == AllianceRank.R4);
        public int R3 => Alliance.Players.Values.Count(player => player.Rank == AllianceRank.R3);
        public int R2 => Alliance.Players.Values.Count(player => player.Rank == AllianceRank.R2);
        public int R1 => Alliance.Players.Values.Count(player => player.Rank == AllianceRank.R1);
    }

    /// <inheritdoc cref="IK_Deets.Interfaces.IAlliance" />
    public record Alliance : IAlliance
    {
        private string _tag;

        public Alliance(string name,
                        string tag,
                        ushort server,
                        string databaseID = null,
                        ConcurrentDictionary<string, IPlayer> players = null)
        {
            DatabaseID         = databaseID;
            Name               = name;
            Tag                = tag;
            Server             = server;
            Players            = players ?? new ConcurrentDictionary<string, IPlayer>();
            MemberDistribution = new MemberDistribution(this);
        }

        public string DatabaseID
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
        public string Tag
        {
            get => _tag;
            set
            {
                if (!value.StartsWith("[") || !value.EndsWith("]"))
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

        public ConcurrentDictionary<string, IPlayer> Players
        {
            get;
            set;
        }

        public int MemberCount => Players.Count;

        public IMemberDistribution MemberDistribution
        {
            get;
        }
    }
}