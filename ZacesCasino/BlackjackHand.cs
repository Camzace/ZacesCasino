using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZacesCasino
{
    class BlackjackHand
    {
        List<Card> hand;

        public BlackjackHand(Card c)
        {
            hand = new List<Card>();
            hand.Add(c);
        }

        public void Hit(Card c)
        {
            hand.Add(c);
        }

        public int CardCount()
        {
            return hand.Count;
        }

        public int GetTotal()
        {
            int numberOfAces = 0;
            int total = 0;
            for (int i = 0; i < hand.Count; i++)
            {
                total += hand[i].BlackjackValue();
                if (hand[i].IsAnAce()) numberOfAces++;
            }
            while (numberOfAces > 0 && total > 21)
            {
                total -= 10;
                numberOfAces--;
            }
            return total;
        }

        public bool IsBlackjack()
        {
            return (GetTotal() == 21 && hand.Count == 2);
        }

        public bool IsBust()
        {
            return GetTotal() > 21;
        }

        public override string ToString()
        {
            string outputString = "";
            for (int i = 0; i < hand.Count; i++)
            {
                outputString += hand[i].ToString();
                if (i != hand.Count - 1) outputString += " ";
            }
            return outputString;
        }

        public int[] ToInt()
        {
            int[] output = new int[hand.Count];
            for (int i = 0; i < output.Length; i++) output[i] = hand[i].IntegerValue();
            return output;
        }
    }
}
