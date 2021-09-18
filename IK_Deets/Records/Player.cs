// Author: Milan Dierick
// Created: 18/09/2021 21:30
// Solution: IK_Deets

using System;
using IK_Deets.Interfaces;

namespace IK_Deets.Records
{
    /// <inheritdoc cref="IK_Deets.Interfaces.IPlayer" />
    public record Player : IPlayer
    {
        public Player(string name,
                      IAlliance alliance,
                      AllianceRank rank,
                      string databaseID = null,
                      uint troopPower = default,
                      uint highestPower = default,
                      uint defeat = default,
                      uint dismantleDurability = default)
        {
            DatabaseID          = databaseID;
            Name                = name ?? throw new ArgumentNullException(nameof(name));
            Alliance            = alliance ?? throw new ArgumentNullException(nameof(alliance));
            Server              = Alliance.Server;
            TroopPower          = troopPower;
            HighestPower        = highestPower;
            Defeat              = defeat;
            DismantleDurability = dismantleDurability;
            Rank                = rank;
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

        public ushort Server
        {
            get;
            set;
        }

        public IAlliance Alliance
        {
            get;
            set;
        }

        public uint TroopPower
        {
            get;
            set;
        }

        public uint HighestPower
        {
            get;
            set;
        }

        public uint Defeat
        {
            get;
            set;
        }

        public uint DismantleDurability
        {
            get;
            set;
        }

        public AllianceRank Rank
        {
            get;
            set;
        }
    }
}