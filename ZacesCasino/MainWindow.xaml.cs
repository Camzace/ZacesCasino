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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ContentHolder.Content = new SelectionScreen();
        }
        
        public void SetContent(Player p, int contentId)
        {
            switch (contentId) 
            {
                case 0: ContentHolder.Content = new GamePicker(p); break;
                case 1: ContentHolder.Content = new BlackjackScreen(p); break;
                case 2: ContentHolder.Content = new SlotsScreen(p); break;
                case 3: ContentHolder.Content = new VideoPokerScreen(p); break;
                case 4: ContentHolder.Content = new RouletteScreen(p); break;
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.SizeToContent = SizeToContent.WidthAndHeight;
        }
    }
}
