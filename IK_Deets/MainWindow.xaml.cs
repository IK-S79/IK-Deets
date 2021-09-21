using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using IK_Deets.Interfaces;
using IK_Deets.Records;
using MongoDB.Bson;
using MongoDB.Driver;

namespace IK_Deets
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly DataTable                      _dataTable = new("PlayerDataTable");
        private readonly Database.Database              _database  = new();
        private readonly IMongoCollection<BsonDocument> _playerCollection;

        public MainWindow()
        {
            _playerCollection = _database.GetCollection<BsonDocument>("data", "players");
            List<BsonDocument> documents = _playerCollection.Find(new BsonDocument()).ToList();

            List<int>    servers   = _playerCollection.Distinct<int>("server", new BsonDocument()).ToList();
            List<string> alliances = _playerCollection.Distinct<string>("alliance", new BsonDocument()).ToList();
            List<string> ranks     = _playerCollection.Distinct<string>("rank", new BsonDocument()).ToList();

            servers.Insert(0, 0);
            alliances.Insert(0, "All");
            ranks.Insert(0, "All");

            _dataTable.Columns.Add("Name");
            _dataTable.Columns.Add("Server");
            _dataTable.Columns.Add("Alliance");
            _dataTable.Columns.Add("Rank");
            _dataTable.Columns.Add("Troop Power");
            _dataTable.Columns.Add("Highest Power");
            _dataTable.Columns.Add("Tech Contributions");
            _dataTable.Columns.Add("Defeat");
            _dataTable.Columns.Add("Dismantle Durability");
            _dataTable.Columns.Add("Time");

            // TODO: Create POCO mapping
            // foreach (BsonDocument document in documents)
            // {
            //     _dataTable.Rows.Add(document[1], document[2], document[3], document[4], document[5], document[6],
            //                         document[7], document[8], document[9], document[10]);
            // }

            InitializeComponent();

            PlayerGrid.DataContext                = _dataTable;
            ServerSelectionComboBox.DataContext   = servers;
            AllianceSelectionComboBox.DataContext = alliances;
            RankSelectionComboBox.DataContext     = ranks;

            ServerSelectionComboBox.SelectedIndex   = 0;
            AllianceSelectionComboBox.SelectedIndex = 0;
            RankSelectionComboBox.SelectedIndex     = 0;
            
            UpdatePlayerGridFilter();
        }

        private async void UpdatePlayerGridFilter()
        {
            // Lock all controls so that there are no issues with filter queries running concurrently
            FilterControlsGrid.IsEnabled = false;
            
            _dataTable.Rows.Clear();

            FilterDefinitionBuilder<BsonDocument> filterBuilder = Builders<BsonDocument>.Filter;
            FilterDefinition<BsonDocument>        filter        = FilterDefinition<BsonDocument>.Empty;

            if (ServerSelectionComboBox.Text != string.Empty && ServerSelectionComboBox.Text != "0")
            {
                filter &= filterBuilder.Eq("server", int.Parse(ServerSelectionComboBox.Text));
            }

            if (NameSearchTextBox.Text != string.Empty)
            {
                filter &= filterBuilder.Eq("name", NameSearchTextBox.Text);
            }

            if (AllianceSelectionComboBox.Text != string.Empty && AllianceSelectionComboBox.Text != "All")
            {
                filter &= filterBuilder.Eq("alliance", AllianceSelectionComboBox.Text);
            }
            
            if (RankSelectionComboBox.Text != string.Empty && RankSelectionComboBox.Text != "All")
            {
                filter &= filterBuilder.Eq("rank", RankSelectionComboBox.Text);
            }
            
            if (TroopPowerSearchTextBox.Text != string.Empty)
            {
                filter &= filterBuilder.Gte("troop_power", int.Parse(TroopPowerSearchTextBox.Text));
            }
            
            if (HighestPowerSearchTextBox.Text != string.Empty)
            {
                filter &= filterBuilder.Gte("lord_power", int.Parse(HighestPowerSearchTextBox.Text));
            }
            
            if (TechContributionsSearchTextBox.Text != string.Empty)
            {
                filter &= filterBuilder.Gte("tech_contributions", int.Parse(TechContributionsSearchTextBox.Text));
            }
            
            if (DefeatSearchTextBox.Text != string.Empty)
            {
                filter &= filterBuilder.Gte("defeat", int.Parse(DefeatSearchTextBox.Text));
            }
            
            if (DismantleDurabilitySearchTextBox.Text != string.Empty)
            {
                filter &= filterBuilder.Gte("dismantle", int.Parse(DismantleDurabilitySearchTextBox.Text));
            }

            await _playerCollection.Find(filter)
                                   .ForEachAsync(document => _dataTable.Rows.Add(document[1], document[2], document[3],
                                                     document[4], document[5], document[6], document[7],
                                                     document[8], document[9], document[10]));

            _dataTable.AcceptChanges();

            // Unlock all controls
            FilterControlsGrid.IsEnabled = true;
        }

        private void FilterButton_OnClick(object sender, RoutedEventArgs e)
        {
            UpdatePlayerGridFilter();
        }
    }
}