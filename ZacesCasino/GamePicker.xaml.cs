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

namespace ZacesCasino
{
    /// <summary>
    /// Interaction logic for GamePicker.xaml
    /// </summary>
    public partial class GamePicker : UserControl
    {
        Player player;
        public GamePicker(Player p)
        {
            InitializeComponent();
            player = p;
            WelcomeLabel.Content = "Welcome " + p.GetName() + "! Which game would you like to play?";
            ChipsLabel.Content = "You have " + p.GetChips() + " chips.";
        }

        private void BlackjackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Window.GetWindow(this);
            window.SetContent(player, 1);
        }

        private void SlotsButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Window.GetWindow(this);
            window.SetContent(player, 2);
        }

        private void VideoPokerButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Window.GetWindow(this);
            window.SetContent(player, 3);
        }

        private void RouletteButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Window.GetWindow(this);
            window.SetContent(player, 4); 
        }

        private void ReloadButton_Click(object sender, RoutedEventArgs e)
        {
            player.SetChips(500);
            ChipsLabel.Content = "You have " + player.GetChips() + " chips.";
        }

    }
}
