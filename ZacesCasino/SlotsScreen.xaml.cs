using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
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
    /// Interaction logic for SlotsScreen.xaml
    /// </summary>
    public partial class SlotsScreen : UserControl
    {
        int[,] combinations = new int[,] {{5, 6, 7, 8, 9}, {0, 1, 2, 3, 4}, {10, 11, 12, 13, 14}, {0, 6, 12, 8, 4}, {10, 6, 2, 8, 14},
                                          {5, 1, 2, 3, 9}, {5, 11, 12, 13, 9}, {0, 1, 7, 13, 14}, {10, 11, 7, 3, 4}, {5, 11, 7, 3, 9},
                                          {5, 1, 7, 13, 9}, {0, 6, 7, 8, 4}, {10, 6, 7, 8, 14}, {0, 6, 2, 8, 4}, {10, 6, 12, 8, 14},
                                          {5, 6, 2, 8, 9}, {5, 6, 12, 8, 9}, {0, 1, 12, 3, 4}, {10, 11, 2, 13, 14}, {0, 11, 12, 13, 4},
                                          {10, 1, 2, 3, 14}, {5, 11, 2, 13, 9}, {5, 1, 12, 3, 9}, {0, 11, 2, 13, 4}, {10, 1, 12, 3, 14}};
        string[] colourHex = new string[] {"#BD0A40","#14E85C","#9554EA","#1C7096","#595203","#B9F2BF","#ED8D0A","#64216C","#EFCCE5",
                                            "#F935BC","#1D2A27","#C5FB35","#ECDE72","#0451B4","#5D8E04","#36947F","#F5709A","#4BFAF2",
                                            "#B73212","#C785ED","#801D3A","#E4BAAA","#33CB69","#43BF15","#F47B3A"};
        BitmapImage[] reelImages;
        BitmapImage waitImage;
        Player player;
        Random rng;
        int looper;
        DispatcherTimer t;
        SlotSpin spin;
        int lines;
        public SlotsScreen(Player p)
        {
            InitializeComponent();
            player = p;
            reelImages = new BitmapImage[] {new BitmapImage(new Uri("pack://application:,,,/img/wild.png")),
                                            new BitmapImage(new Uri("pack://application:,,,/img/hadouken.png")),
                                            new BitmapImage(new Uri("pack://application:,,,/img/laura.png")),
                                            new BitmapImage(new Uri("pack://application:,,,/img/chun.png")),
                                            new BitmapImage(new Uri("pack://application:,,,/img/mika.png")),
                                            new BitmapImage(new Uri("pack://application:,,,/img/karin.png")),
                                            new BitmapImage(new Uri("pack://application:,,,/img/ace.png")),
                                            new BitmapImage(new Uri("pack://application:,,,/img/king.png")),
                                            new BitmapImage(new Uri("pack://application:,,,/img/queen.png")),
                                            new BitmapImage(new Uri("pack://application:,,,/img/jack.png")),
                                            new BitmapImage(new Uri("pack://application:,,,/img/ten.png"))};
            waitImage = new BitmapImage(new Uri("pack://application:,,,/img/Wait.bmp"));
            rng = new Random();
            SetInitialImages();
        }

        public void SetInitialImages()
        {
            Reel0.Source = reelImages[0];
            Reel1.Source = reelImages[0];
            Reel2.Source = reelImages[0];
            Reel3.Source = reelImages[0];
            Reel4.Source = reelImages[0];
            Reel5.Source = reelImages[0];
            Reel6.Source = reelImages[0];
            Reel7.Source = reelImages[0];
            Reel8.Source = reelImages[0];
            Reel9.Source = reelImages[0];
            Reel10.Source = reelImages[0];
            Reel11.Source = reelImages[0];
            Reel12.Source = reelImages[0];
            Reel13.Source = reelImages[0];
            Reel14.Source = reelImages[0];
            SetChipDisplay();
        }

        public void SetImages()
        {

            looper = 0;
            Reel0.Source = waitImage;
            Reel1.Source = waitImage;
            Reel2.Source = waitImage;
            Reel3.Source = waitImage;
            Reel4.Source = waitImage;
            Reel5.Source = waitImage;
            Reel6.Source = waitImage;
            Reel7.Source = waitImage;
            Reel8.Source = waitImage;
            Reel9.Source = waitImage;
            Reel10.Source = waitImage;
            Reel11.Source = waitImage;
            Reel12.Source = waitImage;
            Reel13.Source = waitImage;
            Reel14.Source = waitImage;
            
            t = new DispatcherTimer();
            t.Interval = TimeSpan.FromMilliseconds(500);
            t.Tick += TimerReveal;
            t.Start();
        }

        public void TimerReveal(Object source, EventArgs e)
        {
            RevealLine(looper);
            looper++;
            if (looper == 5) {

                t.Stop();
                int winnings = spin.CheckForWin(lines);
                if (spin.CheckForBonus()) winnings += lines * 5;
                player.AddChips(winnings);
                SetChipDisplay();
                SetWinningsDisplay(winnings);
                DrawWinCombos(spin.CheckWinningCombinations(lines), spin.CheckNumberOfWinningLines(lines));
                SetButtonsActive(true);
                UpdateButtons();
            }
        }

        public void RevealLine(int number)
        {
            int[] array = spin.GetReelArray();
            switch (number)
            {
                case 0:
                    Reel0.Source = reelImages[array[0]];
                    Reel5.Source = reelImages[array[5]];
                    Reel10.Source = reelImages[array[10]];
                    break;
                case 1:
                    Reel1.Source = reelImages[array[1]];
                    Reel6.Source = reelImages[array[6]];
                    Reel11.Source = reelImages[array[11]];
                    break;
                case 2:
                    Reel2.Source = reelImages[array[2]];
                    Reel7.Source = reelImages[array[7]];
                    Reel12.Source = reelImages[array[12]];
                    break;
                case 3:
                    Reel3.Source = reelImages[array[3]];
                    Reel8.Source = reelImages[array[8]];
                    Reel13.Source = reelImages[array[13]];
                    break;
                case 4:
                    Reel4.Source = reelImages[array[4]];
                    Reel9.Source = reelImages[array[9]];
                    Reel14.Source = reelImages[array[14]];
                    break;
            }
        }

        private void Spin(int lines)
        {
            WinningsDisplay.Content = "";
            player.AddChips(-lines);
            SetChipDisplay();
            SetButtonsActive(false);
            LineContainer.Children.Clear();
            spin = new SlotSpin(rng);
            this.lines = lines;
            SetImages();
            
        }

        private void SetChipDisplay() {
            ChipsDisplay.Content = "Chips: " + player.GetChips();
        }

        private void SetWinningsDisplay(int winnings)
        {
            if (winnings > 0) WinningsDisplay.Content = "You won " + winnings + " chips!";
        }

        private void Spin1Button_Click(object sender, RoutedEventArgs e)
        {
            Spin(1);
        }

        private void Spin5Button_Click(object sender, RoutedEventArgs e)
        {
            Spin(5);
        }

        private void Spin10Button_Click(object sender, RoutedEventArgs e)
        {
            Spin(10);
        }

        private void Spin20Button_Click(object sender, RoutedEventArgs e)
        {
            Spin(20);
        }

        private void Spin25Button_Click(object sender, RoutedEventArgs e)
        {
            Spin(25);
        }

        private void SetButtonsActive(bool active)
        {
            Spin1Button.IsEnabled = active;
            Spin5Button.IsEnabled = active;
            Spin10Button.IsEnabled = active;
            Spin20Button.IsEnabled = active;
            Spin25Button.IsEnabled = active;
            BackButton.IsEnabled = active;
        }

        private void UpdateButtons()
        {
            int currentChips = player.GetChips();
            if (currentChips < 25) Spin25Button.IsEnabled = false;
            if (currentChips < 20) Spin20Button.IsEnabled = false;
            if (currentChips < 10) Spin10Button.IsEnabled = false;
            if (currentChips < 5) Spin5Button.IsEnabled = false;
            if (currentChips < 1) Spin1Button.IsEnabled = false;
        }

        private void DrawWinCombos(bool[] combos, int winners)
        {
            int reelSize = 100;
            
            int theseWinners = 0;
            for (int i = 0; i < lines; i++)
            {
                if (combos[i])
                {
                    theseWinners++;
                    double centre = (winners + 1) / 2.0;
                    int yOffset = (int)((theseWinners - centre) * 5);
                    Line line;

                    for (int j = -1; j < 5; j++)
                    {
                        line = new Line();
                        line.Stroke = (SolidColorBrush)new BrushConverter().ConvertFromString(colourHex[i]);
                        line.StrokeThickness = 4;
                        if (j == -1)
                        {
                            line.X1 = 0;
                            line.X2 = reelSize / 2;
                            line.Y1 = reelSize / 2 + yOffset + reelSize * (combinations[i, 0] / 5);
                            line.Y2 = line.Y1;
                        }
                        else if (j == 4)
                        {
                            line.X1 = 9 * reelSize / 2;
                            line.X2 = 5 * reelSize;
                            line.Y1 = reelSize / 2 + yOffset + reelSize * (combinations[i, 4] / 5);
                            line.Y2 = line.Y1;
                        }
                        else
                        {
                            line.X1 = reelSize / 2 + reelSize * (combinations[i, j] % 5);
                            line.X2 = reelSize / 2 + reelSize * (combinations[i, j + 1] % 5);
                            line.Y1 = reelSize / 2 + yOffset + reelSize * (combinations[i, j] / 5);
                            line.Y2 = reelSize / 2 + yOffset + reelSize * (combinations[i, j + 1] / 5);
                        }
                        LineContainer.Children.Add(line);
                    }
                   
                  
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Window.GetWindow(this);
            window.SetContent(player, 0);
        }
    }
}
