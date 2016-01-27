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
    /// Interaction logic for BlackjackScreen.xaml
    /// </summary>
    public partial class BlackjackScreen : UserControl
    {
        Player player;
        Deck deck;
        BlackjackHand yourHand, dealerHand;
        int bet;
        DispatcherTimer t;

        public BlackjackScreen(Player p)
        {
            InitializeComponent();
            player = p;
            PlayButtonsEnabled(false);
            BetButtonsEnabled(true);
            SetChipDisplay();
        }

        private void StartAHand(int betsize)
        {
            ResetImages();
            BetButtonsEnabled(false);
            PlayButtonsEnabled(true);
            BackButton.IsEnabled = false;

            bet = betsize;

            deck = new Deck();
            yourHand = new BlackjackHand(deck.Draw());
            dealerHand = new BlackjackHand(deck.Draw());

            yourHand.Hit(deck.Draw());
            DisplayCards();
            CheckResult();
        }

        private void CheckResult()
        {
            if (yourHand.IsBust()) Busted();
            else if (yourHand.IsBlackjack()) DealersTurn();
        }

        private void DealersTurn()
        {
            PlayButtonsEnabled(false);
            t = new DispatcherTimer();
            t.Interval = TimeSpan.FromMilliseconds(500);
            t.Tick += TimerReveal;
            t.Start();
        }

        public void TimerReveal(Object source, EventArgs e)
        {
            dealerHand.Hit(deck.Draw());
            DisplayCards();
            if (dealerHand.GetTotal() >= 17)
            {
                t.Stop();
                CheckFinalResult();
            }
        }
        
        private void Busted()
        {
            Result.Content = "Busted! You lose " + bet + " chips.";
            player.AddChips(-bet);
            SetChipDisplay();
            PlayButtonsEnabled(false);
            BetButtonsEnabled(true);
            BackButton.IsEnabled = true;
        }

        private void CheckFinalResult()
        {
            if (yourHand.IsBlackjack())
            {
                if (dealerHand.IsBlackjack()) ResolveHand(0.0);
                else ResolveHand(1.5);
            }
            else if (dealerHand.IsBlackjack()) ResolveHand(-1.0);
            else if (dealerHand.GetTotal() > 21) ResolveHand(1.0);
            else if (yourHand.GetTotal() > dealerHand.GetTotal()) ResolveHand(1.0);
            else if (yourHand.GetTotal() == dealerHand.GetTotal()) ResolveHand(0.0);
            else if (yourHand.GetTotal() < dealerHand.GetTotal()) ResolveHand(-1.0);
            else ResolveHand(0.0);
        }

        private void ResolveHand(double multiplier)
        {
            int winnings = (int)(bet * multiplier);
            player.AddChips(winnings);
            if (multiplier == 1.5) Result.Content = "BLACKJACK! You win " + winnings + " chips!";
            else if (multiplier == 1.0) Result.Content = "You win " + winnings + " chips!";
            else if (multiplier == 0.0) Result.Content = "Push hand. Your bet is returned.";
            else if (multiplier == -1.0) Result.Content = "Dealer wins. You lose " + -winnings + " chips.";
            SetChipDisplay();
            BetButtonsEnabled(true);
            BackButton.IsEnabled = true;
        }

        private void BetButtonsEnabled(bool enabled)
        {
            int chips = player.GetChips();
            if (chips >= 10) Bet10Button.IsEnabled = enabled;
            else Bet10Button.IsEnabled = false;
            if (chips >= 20) Bet20Button.IsEnabled = enabled;
            else Bet20Button.IsEnabled = false;
            if (chips >= 30) Bet30Button.IsEnabled = enabled;
            else Bet30Button.IsEnabled = false;
            if (chips >= 40) Bet40Button.IsEnabled = enabled;
            else Bet40Button.IsEnabled = false;
            if (chips >= 50) Bet50Button.IsEnabled = enabled;
            else Bet50Button.IsEnabled = false;
        }

        private void PlayButtonsEnabled(bool enabled)
        {
            HitButton.IsEnabled = enabled;
            StandButton.IsEnabled = enabled;
            DoubleButton.IsEnabled = enabled;
        }

        private void ResetImages()
        {
            Card1.Source = null;
            Card2.Source = null;
            Card3.Source = null;
            Card4.Source = null;
            Card5.Source = null;
            DCard1.Source = null;
            DCard2.Source = null;
            DCard3.Source = null;
            DCard4.Source = null;
            DCard5.Source = null;
            Result.Content = "";
        }

        private void DisplayCards()
        {
            int[] yourHandArray = yourHand.ToInt();
            int[] dealerHandArray = dealerHand.ToInt();
            for (int i = 0; i < yourHandArray.Length; i++)
            {
                switch (i)
                {
                    case 0: Card1.Source = new BitmapImage(new Uri("pack://application:,,,/img/cards/" + (yourHandArray[i] + 1) + ".png")); break;
                    case 1: Card2.Source = new BitmapImage(new Uri("pack://application:,,,/img/cards/" + (yourHandArray[i] + 1) + ".png")); break;
                    case 2: Card3.Source = new BitmapImage(new Uri("pack://application:,,,/img/cards/" + (yourHandArray[i] + 1) + ".png")); break;
                    case 3: Card4.Source = new BitmapImage(new Uri("pack://application:,,,/img/cards/" + (yourHandArray[i] + 1) + ".png")); break;
                    case 4: Card5.Source = new BitmapImage(new Uri("pack://application:,,,/img/cards/" + (yourHandArray[i] + 1) + ".png")); break;
                    case 5: Card6.Source = new BitmapImage(new Uri("pack://application:,,,/img/cards/" + (yourHandArray[i] + 1) + ".png")); break;
                    case 6: Card7.Source = new BitmapImage(new Uri("pack://application:,,,/img/cards/" + (yourHandArray[i] + 1) + ".png")); break;
                }
            }
            for (int i = 0; i < dealerHandArray.Length; i++)
            {

                switch (i)
                {
                    case 0: DCard1.Source = new BitmapImage(new Uri("pack://application:,,,/img/cards/" + (dealerHandArray[i] + 1) + ".png")); break;
                    case 1: DCard2.Source = new BitmapImage(new Uri("pack://application:,,,/img/cards/" + (dealerHandArray[i] + 1) + ".png")); break;
                    case 2: DCard3.Source = new BitmapImage(new Uri("pack://application:,,,/img/cards/" + (dealerHandArray[i] + 1) + ".png")); break;
                    case 3: DCard4.Source = new BitmapImage(new Uri("pack://application:,,,/img/cards/" + (dealerHandArray[i] + 1) + ".png")); break;
                    case 4: DCard5.Source = new BitmapImage(new Uri("pack://application:,,,/img/cards/" + (dealerHandArray[i] + 1) + ".png")); break;
                    case 5: DCard6.Source = new BitmapImage(new Uri("pack://application:,,,/img/cards/" + (dealerHandArray[i] + 1) + ".png")); break;
                    case 6: DCard7.Source = new BitmapImage(new Uri("pack://application:,,,/img/cards/" + (dealerHandArray[i] + 1) + ".png")); break;
                }
            }
            HandTotal.Content = yourHand.GetTotal();
            DealerHandTotal.Content = dealerHand.GetTotal();
        }

        private void SetChipDisplay()
        {
            ChipsDisplay.Content = "Chips: " +  player.GetChips();
        }

        private void Bet10Button_Click(object sender, RoutedEventArgs e)
        {
            StartAHand(10);
        }

        private void Bet20Button_Click(object sender, RoutedEventArgs e)
        {
            StartAHand(20);
        }

        private void Bet30Button_Click(object sender, RoutedEventArgs e)
        {
            StartAHand(30);
        }

        private void Bet40Button_Click(object sender, RoutedEventArgs e)
        {
            StartAHand(40);
        }

        private void Bet50Button_Click(object sender, RoutedEventArgs e)
        {
            StartAHand(50);
        }

        private void HitButton_Click(object sender, RoutedEventArgs e)
        {
            yourHand.Hit(deck.Draw());
            DisplayCards();
            CheckResult();
        }

        private void StandButton_Click(object sender, RoutedEventArgs e)
        {
            DealersTurn();
        }

        private void DoubleButton_Click(object sender, RoutedEventArgs e)
        {
            bet *= 2;
            yourHand.Hit(deck.Draw());
            DisplayCards();
            if (yourHand.IsBust()) Busted();
            else DealersTurn();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Window.GetWindow(this);
            window.SetContent(player, 0);
        }


    }
}
