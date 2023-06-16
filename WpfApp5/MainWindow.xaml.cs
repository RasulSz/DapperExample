using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp5.Entities;

namespace WpfApp5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //GetAllCaller();
            var player = GetById(1);
            player.Score = 50;
            player.Name = "New Gamer";
            Update(player);
            GetAllCaller();
            //MyDataGrid.ItemsSource = new List<Player> { player };
        }
         
        public async void GetAllCaller()
        {
            var players = await GetAllAsync();
            MyDataGrid.ItemsSource = players;

        }

        public async Task<List<Player>> GetAllAsync()
        {
            List<Player> players = new List<Player>();
            var conn = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
            using (var connection = new SqlConnection(conn))
            {
                var data = await connection.QueryAsync<Player>("SELECT Id,Name,Score,IsStar FROM Players");
                players = data.ToList();
            }
            return players;
        }

        public Player GetById(int id)
        {
            var conn = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
            using (var connection = new SqlConnection(conn))
            {
                var player = connection
                    .QueryFirstOrDefault<Player>("SELECT * FROM Players WHERE Id = @PId", new { PId = id });
                return player;
            }
        }

        public void Update(Player player)
        {
            var conn = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
            using(var connection = new SqlConnection(conn))
            {
                connection.Execute(@"
                 UPDATE Players
                 SET Name = @PName,Score = @PScore,IsStar=@PIsStar
                 Where Id = @PId
                 ", new {PName= player.Name,PScore = player.Score,PIsStar = player.IsStar,PId = player.Id });
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
