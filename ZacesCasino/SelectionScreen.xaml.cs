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
    /// Interaction logic for SelectionScreen.xaml
    /// </summary>
    public partial class SelectionScreen : UserControl
    {
        public SelectionScreen()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextbox.Text;
            MainWindow window = (MainWindow)Window.GetWindow(this);
            window.SetContent(new Player(name, 500),0);
        }
    }
}
