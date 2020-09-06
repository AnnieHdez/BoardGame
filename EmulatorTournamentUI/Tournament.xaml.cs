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
using System.Windows.Threading;
using EmulatorBoardGame;
using Tic_Tac_Toe;
using FourinlineGame;
using System.Threading;

namespace EmulatorTournamentUI
{
    /// <summary>
    /// Lógica de interacción para Tournament.xaml
    /// </summary>
    public partial class TournamentWindow : Window
    {
        Dictionary<string,List<MyPlayer>> createdPlayers;
        string gameName;

        ITournament current_tournament;
		
		public TournamentWindow(Dictionary<string, List<MyPlayer>> createdPlayers, string gameName)
        {
            InitializeComponent();
            this.createdPlayers = createdPlayers;
            this.gameName = gameName;

            ComBoxTournaments.ItemsSource = new List<string>{ "Title", "Individual Classification", "TwoByTwo" };
            ComBoxTournaments.SelectedIndex = 0;
        }

        private void startBut_Click(object sender, RoutedEventArgs e)
        {
             string typeTournament = ComBoxTournaments.Text;
             int int_index = 0;
             if (ComBoxTournaments.Text == "Title")
                {
                    string index = TitleTeamIndexTexB.Text;
                    if (!int.TryParse(index,out int_index))
                    {
                        MessageBox.Show(
                                    "You have to choose the Title Team, for this kind of tourament", "Warning",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                }

             if (gameName == "Tic Tac Toe")
             {
                 if (!(createdPlayers.Count >= FourInLineGame.ValidNumberOfPlayers))
                     {
                      MessageBox.Show("The number of players is not valid, get back to the previus window and fixed", "Error", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                       return;
                     }
                    
             }
             else if (gameName == "Four in Line")
             {
                 if (!(createdPlayers.Count >= FourInLineGame.ValidNumberOfPlayers))
                 {
                     MessageBox.Show("The number of players is not valid, get back to the previus window and fixed", "Error", MessageBoxButton.OK,
                       MessageBoxImage.Error);
                 return;
                 }
             }

            Log.StartTournament += LogOn;
            Log.EndTournament += LogOn;

            if (MatchCheckB.IsChecked == true)
            {
                Log.EndMatch += LogOn;
                Log.StartMatch += LogOn;
            }

            if (PlaysCheckB.IsChecked == true)
                Log.Play += LogOn;

            if (GamesCheckB.IsChecked == true)
            {
                Log.StartGame += LogOn;
                Log.EndGame += LogOn;
            }

            if (ComBoxTournaments.SelectedValue != string.Empty)
            {
                AutomaticBut.IsEnabled = true;
                GameBut.IsEnabled = true;
                TournamentBut.IsEnabled = true;
                matchBut.IsEnabled = true;
                PlayBut.IsEnabled = true;

                TitelLab.IsEnabled = false;
                TitleTeamIndexTexB.IsEnabled = false;

                GamesCheckB.IsEnabled = false;
                PlaysCheckB.IsEnabled = false;
                MatchCheckB.IsEnabled = false;

                PreviousBut.IsEnabled = false;
                ComBoxTournaments.IsEnabled = false;
                startBut.IsEnabled = false;
                
                if (gameName == "Tic Tac Toe")
                {
                    List<Team<TicTacToe>> teams = CreateTeams<TicTacToe>();
                   
                        if (ComBoxTournaments.Text == "Title")
                        {
                            current_tournament = new Title<TictactoeMatch, TicTacToeGame, TicTacToe>(int_index,teams);
                        }
                        if (ComBoxTournaments.Text == "TwoByTwo")
                            current_tournament = new TwoByTwo<TictactoeMatch, TicTacToeGame, TicTacToe>(teams);

                        if (ComBoxTournaments.Text == "Individual Classification")
                            current_tournament = new IndividualClassification<TictactoeMatch, TicTacToeGame, TicTacToe>(teams);

                        current_tournament.Start();
                    
                }
                else if (gameName == "Four in Line")
                {

                    List<Team<FourInLine>> teamsFourinline = CreateTeams<FourInLine>();

                    if (ComBoxTournaments.Text == "Title")
                        {
                                current_tournament = new Title<FourInLineMatch, FourInLineGame, FourInLine>(
                                    int_index, teamsFourinline);
                        }

                       if (ComBoxTournaments.Text == "TwoByTwo")
                            current_tournament = new TwoByTwo<FourInLineMatch, FourInLineGame, FourInLine>(teamsFourinline);

                         if (ComBoxTournaments.Text == "Individual Classification")
                            current_tournament = new IndividualClassification<FourInLineMatch, FourInLineGame, FourInLine>(teamsFourinline);

                    
                    current_tournament.Start();

                }
                else
                {
                    MessageBox.Show("The selected game is not available", "Error", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }

        }
        

        private List<Team<T>> CreateTeams<T>() where T: IGameKind
        {
            var res = new List<Team<T>>();

            foreach (var team in createdPlayers)
            {
                var players = new List<Player<T>>();

                foreach (var myPlayer in team.Value)
                {
                    if (myPlayer.TypePlayer == "Greedy")
                        players.Add(new GreedyPlayer<T>(myPlayer.Name));
                    if (myPlayer.TypePlayer == "Random")
                        players.Add(new RandomPlayer<T>(myPlayer.Name));

                }

                res.Add(new Team<T>(team.Key,players));
            }
            return res;
        }

        private void LogOn(object sender, LogArg logArg)
        {
            TournamentListB.Items.Add(logArg.Message);
        }

        private void TournamentBut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                current_tournament.Finish();
            }
            catch (Exception)
            {
                MessageBox.Show("This Tournament is ended", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void PreviousBut_Click(object sender, RoutedEventArgs e)
        {
            SelectPlayers sp = new SelectPlayers(gameName);
            sp.Show();
            Close();                           
        }

        private void matchBut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(!current_tournament.StartNewMatch())
                    MessageBox.Show("This Match is ended", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                TournamentListB.SelectedIndex = TournamentListB.Items.Count - 1;
                TournamentListB.ScrollIntoView(TournamentListB.SelectedItem);
            }
            catch (Exception)
            {
                MessageBox.Show("This Match is ended", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void GameBut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(!current_tournament.StartNewGame())
                    MessageBox.Show("This Game is ended", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                TournamentListB.SelectedIndex = TournamentListB.Items.Count - 1;
                TournamentListB.ScrollIntoView(TournamentListB.SelectedItem);
            }
            catch (Exception)
            {
                MessageBox.Show("This Game is ended", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void PlayBut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(!current_tournament.DoNextPlay())
                    MessageBox.Show("This Game is ended", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                TournamentListB.SelectedIndex = TournamentListB.Items.Count - 1;
                TournamentListB.ScrollIntoView(TournamentListB.SelectedItem);
            }
            catch (Exception)
            {
                MessageBox.Show("This Game is ended", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
            
        }

        private void AutomaticBut_Click(object sender, RoutedEventArgs e)
        {
           
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            timer.Tick += TimerOnTick;
            timer.Start();
            
        }

        private void TimerOnTick(object sender, EventArgs eventArgs)
        {
            var play_res = current_tournament.DoNextPlay();
            TournamentListB.SelectedIndex = TournamentListB.Items.Count - 1;
            TournamentListB.ScrollIntoView(TournamentListB.SelectedItem);
            if (!play_res)
                {
                    ((DispatcherTimer)sender).Stop();
                    MessageBox.Show("This Tournament is ended", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
        }

        private void ComBoxTournaments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((string)ComBoxTournaments.SelectedValue == "Title")
            {
                TitelLab.Visibility = Visibility.Visible;
                TitleTeamIndexTexB.Visibility = Visibility.Visible;
            }
            else
            {
                TitelLab.Visibility = Visibility.Hidden;
                TitleTeamIndexTexB.Visibility = Visibility.Hidden;
            }
        }
    }
}
