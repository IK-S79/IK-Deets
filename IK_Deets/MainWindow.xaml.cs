using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows;
using IK_Deets.Interfaces;
using IK_Deets.Records;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace IK_Deets
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly DataTable                 _dataTable;
        private readonly Database.Database         _database;
        private readonly IMongoCollection<IPlayer> _playerCollection;

        public MainWindow()
        {
            BsonClassMap.RegisterClassMap<Player>();

            _database  = new Database.Database();
            _dataTable = new DataTable("PlayerDataTable");

            _playerCollection = _database.GetCollection<IPlayer>("data", "players");

            List<IPlayer> documents = _playerCollection.Find(FilterDefinition<IPlayer>.Empty).ToList();

            List<ushort> servers = _playerCollection.Distinct<ushort>("server", FilterDefinition<IPlayer>.Empty)
                                                    .ToList();
            List<string> alliances = _playerCollection.Distinct<string>("alliance", FilterDefinition<IPlayer>.Empty)
                                                      .ToList();
            List<string> ranks = _playerCollection.Distinct<string>("rank", FilterDefinition<IPlayer>.Empty).ToList();

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
            _playerCollection.Find(FilterDefinition<IPlayer>.Empty)
                             .ForEachAsync(player => _dataTable.Rows.Add(player.Name, player.Server, player.Name,
                                                                         player.Rank.ToString(), player.TroopPower,
                                                                         player.HighestPower, player.TechContributions,
                                                                         player.Defeat, player.DismantleDurability,
                                                                         player.SubmissionDateTime));

            InitializeComponent();

            DatePicker.SelectedDate = DateTime.Now.AddDays(-1);

            PlayerGrid.DataContext                = _dataTable;
            ServerSelectionComboBox.DataContext   = servers;
            AllianceSelectionComboBox.DataContext = alliances;
            RankSelectionComboBox.DataContext     = ranks;

            ServerSelectionComboBox.SelectedIndex   = 0;
            AllianceSelectionComboBox.SelectedIndex = 0;
            RankSelectionComboBox.SelectedIndex     = 0;

            UpdatePlayerGridFilter();
        }

        private void UpdatePlayerGridFilter()
        {
            // Lock all controls so that there are no issues with filter queries running concurrently
            FilterControlsGrid.IsEnabled = false;

            _dataTable.Rows.Clear();

            FilterDefinitionBuilder<IPlayer> filterBuilder = Builders<IPlayer>.Filter;
            FilterDefinition<IPlayer>        filter        = FilterDefinition<IPlayer>.Empty;

            if (ServerSelectionComboBox.Text != string.Empty && ServerSelectionComboBox.Text != "0")
            {
                filter &= filterBuilder.Eq(player => player.Server, ushort.Parse(ServerSelectionComboBox.Text));
            }

            if (NameSearchTextBox.Text != string.Empty)
            {
                filter &= filterBuilder.Eq(player => player.Name, NameSearchTextBox.Text);
            }

            if (AllianceSelectionComboBox.Text != string.Empty && AllianceSelectionComboBox.Text != "All")
            {
                filter &= filterBuilder.Eq(player => player.Alliance, AllianceSelectionComboBox.Text);
            }

            if (RankSelectionComboBox.Text != string.Empty && RankSelectionComboBox.Text != "All")
            {
                Enum.TryParse(RankSelectionComboBox.Text, out AllianceRank rank);
                filter &= filterBuilder.Eq(player => player.Rank, rank);
            }

            if (TroopPowerSearchTextBox.Text != string.Empty)
            {
                filter &= filterBuilder.Gte(player => player.TroopPower, uint.Parse(TroopPowerSearchTextBox.Text));
            }

            if (HighestPowerSearchTextBox.Text != string.Empty)
            {
                filter &= filterBuilder.Gte(player => player.HighestPower, uint.Parse(HighestPowerSearchTextBox.Text));
            }

            if (TechContributionsSearchTextBox.Text != string.Empty)
            {
                filter &= filterBuilder.Gte(player => player.TechContributions,
                                            uint.Parse(TechContributionsSearchTextBox.Text));
            }

            if (DefeatSearchTextBox.Text != string.Empty)
            {
                filter &= filterBuilder.Gte(player => player.Defeat, uint.Parse(DefeatSearchTextBox.Text));
            }

            if (DismantleDurabilitySearchTextBox.Text != string.Empty)
            {
                filter &= filterBuilder.Gte(player => player.DismantleDurability,
                                            uint.Parse(DismantleDurabilitySearchTextBox.Text));
            }

            // TODO: Figure out why this isn't working
            // if (DatePicker.Text != string.Empty)
            // {
            //     filter &= filterBuilder.Eq(player => player.SubmissionDateTime.ToUniversalTime().Date, DatePicker.SelectedDate!.Value.Date);
            //     filter &= filterBuilder.Where(player => player.SubmissionDateTime == DatePicker.SelectedDate!.Value);
            // }

            List<IPlayer>? list = _playerCollection.Find(filter).ToList();

            if (DatePicker.SelectedDate != null)
            {
                ConcurrentBag<IPlayer> filteredPlayers    = new();
                DateTime               datePickerDateTime = DatePicker.SelectedDate.GetValueOrDefault();

                Parallel.ForEach(list, player =>
                {
                    if (player.SubmissionDateTime.Date == datePickerDateTime)
                    {
                        filteredPlayers.Add(player);
                    }
                });

                foreach (IPlayer player in filteredPlayers)
                {
                    _dataTable.Rows.Add(player.Name, player.Server, player.Name, player.Rank.ToString(),
                                        player.TroopPower, player.HighestPower, player.TechContributions, player.Defeat,
                                        player.DismantleDurability, player.SubmissionDateTime);
                }
            }
            else
            {
                foreach (IPlayer player in list)
                {
                    _dataTable.Rows.Add(player.Name, player.Server, player.Name, player.Rank.ToString(),
                                        player.TroopPower, player.HighestPower, player.TechContributions, player.Defeat,
                                        player.DismantleDurability, player.SubmissionDateTime);
                }
            }

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