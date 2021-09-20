using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        private          Task                           _gridUpdateTask;
        private          CancellationTokenSource        _tokenSource;

        public MainWindow()
        {
            _tokenSource = new CancellationTokenSource();

            _playerCollection = _database.GetCollection<BsonDocument>("data", "players");
            List<BsonDocument> documents = _playerCollection.Find(new BsonDocument()).ToList();

            List<int> servers = _playerCollection.Distinct<int>("server", new BsonDocument()).ToList();

            // _dataTable.Columns.Add("ID");
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

            // _dataTable.Columns.Add("Time");
            // _dataTable.Columns.Add("Name");
            // _dataTable.Columns.Add("Lord Power");
            // _dataTable.Columns.Add("Troop Power");

            // TODO: Create POCO mapping
            foreach (BsonDocument document in documents)
            {
                _dataTable.Rows.Add(document[1], document[2], document[3], document[4], document[5], document[6],
                                    document[7], document[8], document[9], document[10]);
            }

            InitializeComponent();

            PlayerGrid.DataContext              = _dataTable;
            ServerSelectionComboBox.DataContext = servers;
        }

        private async void UpdatePlayerGridFilter()
        {
            // Lock all controls so that there are no issues with filter queries running concurrently
            
            _dataTable.Rows.Clear();

            FilterDefinitionBuilder<BsonDocument> filterBuilder = Builders<BsonDocument>.Filter;
            FilterDefinition<BsonDocument>        filter        = FilterDefinition<BsonDocument>.Empty;

            if (ServerSelectionComboBox.Text != string.Empty)
            {
                filter = filterBuilder.Eq("server", int.Parse(ServerSelectionComboBox.Text));
            }

            if (NameSearchTextBox.Text != string.Empty)
            {
                filter &= filterBuilder.Eq("name", NameSearchTextBox.Text);
            }

            await _playerCollection.Find(filter)
                                   .ForEachAsync(document => _dataTable.Rows.Add(document[1], document[2], document[3],
                                                     document[4], document[5], document[6], document[7],
                                                     document[8], document[9], document[10]));

            _dataTable.AcceptChanges();
            
            // Unlock all controls
        }

        private void ServerSelectionComboBox_OnDropDownClosed(object sender, EventArgs e)
        {
            UpdatePlayerGridFilter();
        }
    }
}