// Author: Milan Dierick
// Created: 18/09/2021 05:15
// Solution: IK_Deets

namespace IK_Deets.Interfaces
{
    public enum AllianceRank
    {
        R6,
        R5,
        R4,
        R3,
        R2,
        R1
    }

    public interface IPlayer
    {
        string DatabaseID
        {
            get;
            set;
        }
        
        string Name
        {
            get;
            set;
        }

        ushort Server
        {
            get;
            set;
        }

        IAlliance Alliance
        {
            get;
            set;
        }

        uint TroopPower
        {
            get;
            set;
        }

        uint HighestPower
        {
            get;
            set;
        }

        uint Defeat
        {
            get;
            set;
        }

        uint DismantleDurability
        {
            get;
            set;
        }

        AllianceRank Rank
        {
            get;
            set;
        }
    }
}