using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using EmulatorBoardGame;
using Tic_Tac_Toe;
using System.Collections;

namespace EmulatorTournamentUI
{
    /// <summary>
    /// Lógica de interacción para SelectPlayers.xaml
    /// </summary>
    public partial class SelectPlayers : Window 
    {
        string game_name;
        Dictionary<string,List<MyPlayer>> create_players;

        public SelectPlayers(string gameName)
        {
            InitializeComponent();
            game_name = gameName;
            create_players = new Dictionary<string, List<MyPlayer>>();
            TypePlayersComBox.ItemsSource = new List<string>() { "Greedy", "Random" };
            TypePlayersComBox.SelectedIndex = 1;
        }

        private void NextPlayerBut_Click(object sender, RoutedEventArgs e)
        {
            if (PlayerIDTexBox.Text == string.Empty)
                MessageBox.Show("You have to writte the player ID","Warning",MessageBoxButton.OK,MessageBoxImage.Information);

            else if (TeamIDTexBox.Text == string.Empty)
                MessageBox.Show("You have to writte the Team ID", "Warning", MessageBoxButton.OK, MessageBoxImage.Information);

            else if (TypePlayersComBox.Text == string.Empty)
                MessageBox.Show("You have to choose the type of player", "Warning", MessageBoxButton.OK,
                                MessageBoxImage.Information);

            else
            {
                bool exist = false;
                foreach (var item in create_players)
                {
                    foreach (var player in item.Value)
                    {
                        if (player.Name == PlayerIDTexBox.Text)
                        {
                            exist = true;
                            MessageBox.Show("Already exist a player with this ID", "Error", MessageBoxButton.OK,
                                            MessageBoxImage.Error);
                        }
                    }
                }


                if (!exist)
                {
                    var new_player = new MyPlayer(PlayerIDTexBox.Text, TeamIDTexBox.Text, TypePlayersComBox.Text);
                    if (create_players.Keys.Contains(TeamIDTexBox.Text))
                    {
                        create_players[TeamIDTexBox.Text].Add(new_player);
                    }
                    else create_players.Add(TeamIDTexBox.Text, new List<MyPlayer> {new_player});

                    AllPlayersListBox.Items.Add(new_player.ToString());
                }
            }
        }

        private void NextBut2_Click(object sender, RoutedEventArgs e)
        {
            TournamentWindow tourn = new TournamentWindow(create_players, game_name);
            tourn.Show();
            Close();
        }

        private void PreviousBut2_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            Close();
        }
    }

    public class MyPlayer
    {
        public string Name { get; set; }
        public string Team { get; set; }
        public string TypePlayer { get; set; }

        public MyPlayer(string name, string team, string typeplayer) 
        {
            Name = name;
            Team = team;
            TypePlayer = typeplayer;
        }

        public override string ToString()
        {
            return Name + "-" + Team + "-" + TypePlayer;
        }
    }
}

