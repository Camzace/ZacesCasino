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
    /// Interaction logic for VideoPokerScreen.xaml
    /// </summary>
    public partial class VideoPokerScreen : UserControl
    {
        Player player;
        Deck deck;
        Card[] cards;
        int bet;
        string[] finalHands = new string[] { "Royal Flush", "Straight Flush", "Four of a Kind", "Full House", "Flush", "Straight", "Three of a Kind", "Two Pair", "Jacks or Better", "Nothing"};
        int[] payouts = new int[] { 800, 50, 25, 9, 6, 4, 3, 2, 1, 0 };

        public VideoPokerScreen(Player p)
        {
            InitializeComponent();
            player = p;
            PlayButtonsEnabled(false);
            BetButtonsEnabled(true);
            UpdateChips();
            
        }

        public void StartAHand(int bet)
        {
            this.bet = bet;
            ResetImages();
            BetButtonsEnabled(false);
            PlayButtonsEnabled(true);
            BackButton.IsEnabled = false;

            deck = new Deck();
            cards = new Card[5];
            for (int i = 0; i < 5; i++)
            {
                cards[i] = deck.Draw();
            }

            DisplayCards();
        }

        public void EvaluateHand()
        {
            SortHand();
            bool flush = IsAFlush();
            bool straight = IsAStraight();
            int[] handInfo = GetHandHistogram();

            if (flush && straight)
            {
                if (cards[0].IsAnAce()) HandResult(0); //Royal Flush
                else HandResult(1); //Straight Flush
            }
            else if (handInfo[0] == 4) HandResult(2); //if 4oak, Four of a Kind
            else if (handInfo[0] == 3 && handInfo[2] == 2) HandResult(3); //if 3oak + pair, Full House
            else if (flush) HandResult(4); //Flush
            else if (straight) HandResult(5); //Straight
            else if (handInfo[0] == 3) HandResult(6); //if 3oak, Three of a Kind
            else if (handInfo[0] == 2 && handInfo[2] == 2) HandResult(7); // if pair + pair, Two Pair
            else if (handInfo[0] == 2 && handInfo[1] >= 9) HandResult(8); // if pair jack+, Jacks or Better
            else HandResult(9);
        }

        public void HandResult(int id)
        {
            int winnings = bet * payouts[id];
            if (id == 9) WinningsDisplay.Content = "";
            else WinningsDisplay.Content = finalHands[id] + "! You win " + winnings + " chips!";
            player.AddChips(winnings - bet);
            UpdateChips();
            BackButton.IsEnabled = true;
            BetButtonsEnabled(true);
            PlayButtonsEnabled(false);
        }

        public void SortHand()
        {
            for (int i = 0; i < 5; i++)
            {
                int max = i;
                for (int j = i+1; j < 5; j++)
                {
                    if ((int)cards[j].GetValue() > (int)cards[max].GetValue())
                    {
                        Card temp = cards[j];
                        cards[j] = cards[max];
                        cards[max] = temp;
                    }
                }
            }
        }

        public bool IsAFlush()
        {
            Card.Suit suit = cards[0].GetSuit();
            for (int i = 1; i < 5; i++)
            {
                if (cards[i].GetSuit() != suit) return false;
            }
            return true;
        }

        public bool IsAStraight()
        {
            for (int i = 1; i < 5; i++)
            {
                if ((int)cards[i].GetValue() != (int)cards[i - 1].GetValue() - 1)
                {
                    if (i == 4 && cards[i - 1].GetValue() == Card.Value.TWO && cards[i].GetValue() == Card.Value.ACE) return true;
                    else return false;
                }
            }
            return true;
        }

        public int[] GetHandHistogram()
        {
            int[] cardCount = new int[13];
            for (int i = 0; i < 5; i++)
            {
                cardCount[(int)cards[i].GetValue()]++;
            }
            int[] handHistogram = new int[4];
            int max = 0;
            int index = 0;
            for (int i = 0; i < 13; i++)
            {
                if (cardCount[i] >= max)
                {
                    max = cardCount[i];
                    index = i;
                }
            }
            handHistogram[0] = max;
            handHistogram[1] = index;
            max = 0; index = 0;
            for (int i = 0; i < 13; i++)
            {
                if (i != handHistogram[1] && cardCount[i] >= max)
                {
                    max = cardCount[i];
                    index = i;
                }
            }
            handHistogram[2] = max;
            handHistogram[3] = index;
            return handHistogram;
        }

        private void DisplayCards()
        {
            Card1.Source = new BitmapImage(new Uri("pack://application:,,,/img/cards/" + (cards[0].IntegerValue() + 1) + ".png"));
            Card2.Source = new BitmapImage(new Uri("pack://application:,,,/img/cards/" + (cards[1].IntegerValue() + 1) + ".png"));
            Card3.Source = new BitmapImage(new Uri("pack://application:,,,/img/cards/" + (cards[2].IntegerValue() + 1) + ".png"));
            Card4.Source = new BitmapImage(new Uri("pack://application:,,,/img/cards/" + (cards[3].IntegerValue() + 1) + ".png"));
            Card5.Source = new BitmapImage(new Uri("pack://application:,,,/img/cards/" + (cards[4].IntegerValue() + 1) + ".png"));
        }

        public void BetButtonsEnabled(bool enabled)
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

        public void PlayButtonsEnabled(bool enabled)
        {
            Hold1Button.IsEnabled = enabled;
            Hold2Button.IsEnabled = enabled;
            Hold3Button.IsEnabled = enabled;
            Hold4Button.IsEnabled = enabled;
            Hold5Button.IsEnabled = enabled;
            DealButton.IsEnabled = enabled;
        }

        public void ResetImages()
        {
            Card1.Source = null;
            Card2.Source = null;
            Card3.Source = null;
            Card4.Source = null;
            Card5.Source = null;
            Hold1Button.Content = "Hold";
            Hold2Button.Content = "Hold";
            Hold3Button.Content = "Hold";
            Hold4Button.Content = "Hold";
            Hold5Button.Content = "Hold";
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

        private void Held1Button_Click(object sender, RoutedEventArgs e)
        {
            if ((string)Hold1Button.Content != "Discard") Hold1Button.Content = "Discard";
            else Hold1Button.Content = "Hold";
        }

        private void Held2Button_Click(object sender, RoutedEventArgs e)
        {
            if ((string)Hold2Button.Content != "Discard") Hold2Button.Content = "Discard";
            else Hold2Button.Content = "Hold";
        }

        private void Held3Button_Click(object sender, RoutedEventArgs e)
        {
            if ((string)Hold3Button.Content != "Discard") Hold3Button.Content = "Discard";
            else Hold3Button.Content = "Hold";
        }

        private void Held4Button_Click(object sender, RoutedEventArgs e)
        {
            if ((string)Hold4Button.Content != "Discard") Hold4Button.Content = "Discard";
            else Hold4Button.Content = "Hold";
        }

        private void Held5Button_Click(object sender, RoutedEventArgs e)
        {
            if ((string)(Hold5Button.Content) != "Discard") Hold5Button.Content = "Discard";
            else Hold5Button.Content = "Hold";
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Window.GetWindow(this);
            window.SetContent(player, 0);
        }

        private void DealButton_Click(object sender, RoutedEventArgs e)
        {
            if ((string)Hold1Button.Content != "Hold") cards[0] = deck.Draw();
            if ((string)Hold2Button.Content != "Hold") cards[1] = deck.Draw();
            if ((string)Hold3Button.Content != "Hold") cards[2] = deck.Draw();
            if ((string)Hold4Button.Content != "Hold") cards[3] = deck.Draw();
            if ((string)Hold5Button.Content != "Hold") cards[4] = deck.Draw();
            DisplayCards();
            EvaluateHand();
        }

        private void UpdateChips()
        {
            ChipsDisplay.Content = "Chips: " + player.GetChips();
        }
    }
}
