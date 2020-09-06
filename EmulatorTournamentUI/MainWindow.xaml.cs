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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EmulatorTournamentUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();            
            gameSelectorCombo.ItemsSource = new List<string>() { "Tic Tac Toe","Four in Line" };
            gameSelectorCombo.SelectedIndex = 0;
        }

        private void ButNex_Click_1(object sender, RoutedEventArgs e)
        {
            string game_name = gameSelectorCombo.Text;
            if (game_name != string.Empty)
            {
                SelectPlayers sp = new SelectPlayers(game_name);
                sp.Show();
                Close();                           
            }
            else
            {
                MessageBox.Show("You have to choose the kind of game", "Error", MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }
    }
}

