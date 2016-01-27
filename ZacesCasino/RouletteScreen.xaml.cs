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
using System.Windows.Threading;

namespace ZacesCasino
{
    /// <summary>
    /// Interaction logic for RouletteScreen.xaml
    /// </summary>
    public partial class RouletteScreen : UserControl
    {
        readonly int GRIDHEIGHT = 36;
        readonly int GRIDWIDTH = 27;

        int[] wheelLookup = new int[]{  0, 23, 6, 35, 4, 19, 10, 31, 16, 27,
                                        18, 14, 33, 12, 25, 2, 21, 8, 29, 3,
                                        24, 5, 28, 17, 20, 7, 36, 11, 32, 30,
                                        15, 26, 1, 22, 9, 34, 13};

        bool[] isBlack = new bool[] {   false, false, true, false, true, false, true, false, true, false, 
                                        true, true, false, true, false, true, false, true, false, false,
                                        true, false, true, false, true, false, true, false, true, true,
                                        false, true, false, true, false, true, false};

        /* 0-36 are individual numbers. 37 = 1+3n. 38 = 2+3n. 39 = 3+3n. 
         * 40 is 1st 12. 41 is 2nd 12. 42 is 3rd 12. 43 is 1st half. 44 is even. 45 is red. 46 is black. 47 is odd. 48 is 2nd half. */
        int[] betArray; 
        Point[] chipPosArray;
        Random rng = new Random();
        DispatcherTimer t;
        int spiralLoop;
        int spin;

        Player player;
        public RouletteScreen(Player p)
        {
            InitializeComponent();
            player = p;
            TableImage.Source = new BitmapImage(new Uri("pack://application:,,,/img/roulettetable.png"));
            WheelImage.Source = new BitmapImage(new Uri("pack://application:,,,/img/roulettewheel.png"));
            betArray = new int[49];
            chipPosArray = new Point[49];
            SetChipPosArray();
            DisplayChips();
        }

        private void HandleBet(Point point)
        {
            int numberOfBet = -1;
            int x = (int)point.X;
            int y = (int)point.Y;
            if (y < GRIDHEIGHT * 3)
            {
                if (x < 27) numberOfBet = 0;
                else numberOfBet = 3 - y / GRIDHEIGHT + 3 * ((x - GRIDWIDTH) / GRIDWIDTH);
            }
            else if (x >= GRIDWIDTH && x < 13 * GRIDWIDTH)
            {
                if (y < GRIDHEIGHT * 4)
                {
                    numberOfBet = 40 + (x - GRIDWIDTH) / (GRIDWIDTH * 4);
                }
                else
                {
                    numberOfBet = 43 + (x - GRIDWIDTH) / (GRIDWIDTH * 2);
                }
            }
            if (numberOfBet != -1 && numberOfBet <= 48)
            {
                int betSize = GetBetSize();
                if (betSize <= player.GetChips())
                {
                    betArray[numberOfBet] += betSize;
                    player.AddChips(-betSize);
                    DisplayBets();
                }
                else
                {
                    Debug.Content = "Not enough chips!";
                }
            }
        }

        private void DisplayBets()
        {
            TableCanvas.Children.Clear();
            for (int i = 0; i < betArray.Length; i++)
            {
                if (betArray[i] > 0)
                {
                    Border border = new Border();
                    border.Height = 18;
                    border.Width = 18;
                    border.Background = Brushes.White;
                    border.BorderThickness = new Thickness(1);
                    border.BorderBrush = Brushes.Black;;
                    border.CornerRadius = new CornerRadius(45);
                    TableCanvas.Children.Add(border);
                    Canvas.SetLeft(border, chipPosArray[i].X);
                    Canvas.SetTop(border, chipPosArray[i].Y);

                    TextBlock chipText = new TextBlock();
                    chipText.Text = "" + betArray[i];
                    chipText.FontSize = 9;
                    chipText.HorizontalAlignment = HorizontalAlignment.Center;
                    chipText.VerticalAlignment = VerticalAlignment.Center;

                    border.Child = chipText;
                }
            }
            DisplayChips();
        }

        private void DisplayChips()
        {
            ChipsDisplay.Content = "Chips: " + player.GetChips();
        }

        public void DrawBallPath()
        {
            spiralLoop = 0;
            t = new DispatcherTimer();
            t.Interval = TimeSpan.FromMilliseconds(50);
            t.Tick += AnimateBall;
            t.Start();
        }

        public void AnimateBall(Object source, EventArgs e)
        {
            double time = 6.0 * Math.PI * spiralLoop / 100.0;
            double a = 2.5 * Math.PI - 2.0 * Math.PI * wheelLookup[spin] / 37.0;
            double x = time * Math.Cos(time + a) * 116 / (6 * Math.PI);
            double y = -1.0 * time * Math.Sin(time + a) * 115 / (6 * Math.PI);
            

            Canvas.SetLeft(Ball, x);
            Canvas.SetTop(Ball, y);

            spiralLoop++;

            if (spiralLoop > 100)
            {
                t.Stop();
                Debug.Content = "Number " + spin + " is the winner!";
                ResolveResult();
            }
        }

        public void ResolveResult()
        {
            int winnings = 0;
            winnings += betArray[spin] * 37; // number correct, pays bet + 36 to 1
            if (spin != 0)
            {
                if (spin % 3 == 1) winnings += betArray[37] * 3; // 1st column, pays bet + 2 to 1
                else if (spin % 3 == 2) winnings += betArray[38] * 3; // 2nd column, pays bet + 2 to 1
                else winnings += betArray[39] * 3; // 3rd column, pays bet + 2 to 1

                if ((spin - 1) / 12 == 0) winnings += betArray[40] * 3; // 1st 12, pays bet + 2 to 1
                else if ((spin - 1) / 12 == 1) winnings += betArray[41] * 3; // 2nd 12, pays bet + 2 to 1
                else winnings += betArray[42] * 3; // 3rd 12, pays bet + 2 to 1

                if ((spin - 1) / 18 == 0) winnings += betArray[43] * 2; // 1st 18, pays double bet
                else winnings += betArray[48] * 2; // 2nd 18, pays double bet

                if (spin % 2 == 0) winnings += betArray[44] * 2; // Even, pays double bet
                else winnings += betArray[47] * 2; // Odd, pays double bet

                if (isBlack[spin]) winnings += betArray[46] * 2; // Black, pays double bet
                else winnings += betArray[45] * 2; // Red, pays double bet
            }
            if (winnings > 0) Debug2.Content = "You won " + winnings + " chips!";
            player.AddChips(winnings);
            DisplayChips();
            ClearTable(false);
            ButtonsEnabled(true);
        }

        private int GetBetSize()
        {
            if (Bet1.IsChecked == true) return 1;
            else if (Bet5.IsChecked == true) return 5;
            else if (Bet10.IsChecked == true) return 10;
            else if (Bet25.IsChecked == true) return 25;
            else return 0;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Window.GetWindow(this);
            window.SetContent(player, 0);
        }

        private void TableCanvas_Clicked(object sender, MouseButtonEventArgs e)
        {
            Debug.Content = "";
            Point p = Mouse.GetPosition(TableCanvas);
            HandleBet(p);
        }

        private void SetChipPosArray()
        {
            int x, y;
            chipPosArray[0] = new Point(GRIDWIDTH / 2 - 8, 3 * GRIDHEIGHT / 2 - 8);
            for (int i = 1; i <= 39; i++)
            {
                x = ((i - 1) / 3) * GRIDWIDTH + 3 * GRIDWIDTH / 2 - 8;
                y = (2 - ((i - 1) % 3)) * GRIDHEIGHT + GRIDHEIGHT / 2 - 8;
                chipPosArray[i] = new Point(x, y);
            }
            for (int i = 40; i <= 42; i++)
            {
                x = (i - 40) * 4 * GRIDWIDTH + 3 * GRIDWIDTH - 8;
                y = 3 * GRIDHEIGHT + GRIDHEIGHT / 2 - 8;
                chipPosArray[i] = new Point(x, y);
            }
            for (int i = 43; i <= 48; i++)
            {
                x = (i - 43) * 2 * GRIDWIDTH + 2 * GRIDWIDTH - 8;
                y = 4 * GRIDHEIGHT + GRIDHEIGHT / 2 - 6;
                chipPosArray[i] = new Point(x, y);
            }
        }
        
        private void ClearTable(bool refund)
        {
            for (int i = 0; i < betArray.Length; i++)
            {
                if (refund) player.AddChips(betArray[i]);
                betArray[i] = 0;
            }
            DisplayBets();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ClearTable(true);
        }

        private void SpinButton_Click(object sender, RoutedEventArgs e)
        {
            Debug.Content = "";
            Debug2.Content = "";
            ButtonsEnabled(false);
            spin = rng.Next(0, 37);
            DrawBallPath();
        }

        private void ButtonsEnabled(bool enabled)
        {
            SpinButton.IsEnabled = enabled;
            ClearButton.IsEnabled = enabled;
            BackButton.IsEnabled = enabled;
        }

    }
}
