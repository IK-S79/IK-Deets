// Author: Milan Dierick
// Created: 18/09/2021 05:15
// Solution: IK_Deets

using System.Collections.Concurrent;
using MongoDB.Bson;

namespace IK_Deets.Interfaces
{
    /// <summary>
    /// Base member distribution interface, exposes properties to retrieve member distributions of an alliance
    /// </summary>
    public interface IMemberDistribution
    {
        IAlliance Alliance
        {
            get;
        }
        
        int R6
        {
            get;
        }

        int R5
        {
            get;
        }

        int R4
        {
            get;
        }

        int R3
        {
            get;
        }

        int R2
        {
            get;
        }

        int R1
        {
            get;
        }
    }

    /// <summary>
    /// Base alliance interface
    /// </summary>
    public interface IAlliance
    {
        ObjectId DatabaseID
        {
            get;
            set;
        }
        
        string Name
        {
            get;
            set;
        }

        string? Tag
        {
            get;
            set;
        }

        ushort Server
        {
            get;
        }

        /// <summary>
        /// All players that belong to this alliance
        /// </summary>
        /// <remarks>
        /// Accessible by multiple threads concurrently
        /// </remarks>
        ConcurrentDictionary<string, IPlayer>? Players
        {
            get;
            set;
        }
        
        int MemberCount
        {
            get;
        }

        IMemberDistribution MemberDistribution
        {
            get;
        }
    }
}