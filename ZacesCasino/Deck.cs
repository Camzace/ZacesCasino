using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZacesCasino
{
    class Deck
    {
        List<Card> cards;

        public Deck()
        {
            cards = new List<Card>();
            for (int i = 0; i < 52; i++)
            {
                cards.Add(new Card(i));
            }
            Shuffle();
        }

        public void Shuffle()
        {
            Random rng = new Random();
            for (int i = 0; i < cards.Count; i++)
            {
                Swap(i, rng.Next(i, cards.Count));
            }
        }

        private void Swap(int i, int j)
        {
            var temp = cards[i];
            cards[i] = cards[j];
            cards[j] = temp;
        }

        public Card Draw()
        {
            Card output = cards[0];
            cards.RemoveAt(0);
            return output;
        }
    }
}
