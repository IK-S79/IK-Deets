using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using IK_Deets.Interfaces;
using IK_Deets.Records;

namespace IK_Deets
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly DataTable _dataTable = new DataTable("PlayerDataTable");

        public MainWindow()
        {
            Alliance alliance = new("Test Alliance", "TEST", 79);
            Player   player1  = new("Test Player1", alliance, AllianceRank.R1);
            Player   player2  = new("Test Player2", alliance, AllianceRank.R2);
            Player   player3  = new("Test Player3", alliance, AllianceRank.R3);
            Player   player4  = new("Test Player4", alliance, AllianceRank.R4);
            Player   player5  = new("Test Player5", alliance, AllianceRank.R5);
            Player   player6  = new("Test Player6", alliance, AllianceRank.R6);

            alliance.Players.AddOrUpdate(player1.Name, player1, (_, _) => player1);
            alliance.Players.AddOrUpdate(player2.Name, player2, (_, _) => player2);
            alliance.Players.AddOrUpdate(player3.Name, player3, (_, _) => player3);
            alliance.Players.AddOrUpdate(player4.Name, player4, (_, _) => player4);
            alliance.Players.AddOrUpdate(player5.Name, player5, (_, _) => player5);
            alliance.Players.AddOrUpdate(player6.Name, player6, (_, _) => player6);

            _dataTable.Columns.Add("ID");
            _dataTable.Columns.Add("Name");
            _dataTable.Columns.Add("Server");
            _dataTable.Columns.Add("Alliance");
            _dataTable.Columns.Add("Troop Power");
            _dataTable.Columns.Add("Highest Power");
            _dataTable.Columns.Add("Defeat");
            _dataTable.Columns.Add("Dismantle Durability");
            _dataTable.Columns.Add("Rank");

            foreach (IPlayer player in alliance.Players.Values)
            {
                _dataTable.Rows.Add(player.DatabaseID, player.Name, player.Server, player.Alliance.Name,
                                    player.TroopPower, player.HighestPower, player.Defeat, player.DismantleDurability,
                                    player.Rank);
            }

            InitializeComponent();

            PlayerGrid.DataContext = _dataTable;
        }
    }
}