using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZacesCasino
{
    class Card
    {
        public enum Suit
        {
            SPADES, HEARTS, DIAMONDS, CLUBS
        }
        public enum Value
        {
            TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE, TEN, JACK, QUEEN, KING, ACE
        }

        Suit suit;
        Value value;
        int integerValue;

        public Card(int input)
        {
            integerValue = input;
            int tempValue = input % 13;
            int tempSuit = input / 13;

            switch (tempValue)
            {
                case 0: value = Value.ACE; break;
                case 1: value = Value.TWO; break;
                case 2: value = Value.THREE; break;
                case 3: value = Value.FOUR; break;
                case 4: value = Value.FIVE; break;
                case 5: value = Value.SIX; break;
                case 6: value = Value.SEVEN; break;
                case 7: value = Value.EIGHT; break;
                case 8: value = Value.NINE; break;
                case 9: value = Value.TEN; break;
                case 10: value = Value.JACK; break;
                case 11: value = Value.QUEEN; break;
                case 12: value = Value.KING; break;
            }

            switch (tempSuit)
            {
                case 0: suit = Suit.SPADES; break;
                case 1: suit = Suit.HEARTS; break;
                case 2: suit = Suit.DIAMONDS; break;
                case 3: suit = Suit.CLUBS; break;
            }
        }

        public override string ToString()
        {
            string returnString = "";
            switch (value)
            {
                case Value.TWO: returnString += "2"; break;
                case Value.THREE: returnString += "3"; break;
                case Value.FOUR: returnString += "4"; break;
                case Value.FIVE: returnString += "5"; break;
                case Value.SIX: returnString += "6"; break;
                case Value.SEVEN: returnString += "7"; break;
                case Value.EIGHT: returnString += "8"; break;
                case Value.NINE: returnString += "9"; break;
                case Value.TEN: returnString += "T"; break;
                case Value.JACK: returnString += "J"; break;
                case Value.QUEEN: returnString += "Q"; break;
                case Value.KING: returnString += "K"; break;
                case Value.ACE: returnString += "A"; break;
            }

            switch (suit)
            {
                case Suit.SPADES: returnString += "S"; break;
                case Suit.HEARTS: returnString += "H"; break;
                case Suit.DIAMONDS: returnString += "D"; break;
                case Suit.CLUBS: returnString += "C"; break;
            }
            return returnString;
        }

        public int BlackjackValue()
        {
            int returnValue = 0;
            switch (value)
            {
                case Value.TWO: returnValue = 2; break;
                case Value.THREE: returnValue = 3; break;
                case Value.FOUR: returnValue = 4; break;
                case Value.FIVE: returnValue = 5; break;
                case Value.SIX: returnValue = 6; break;
                case Value.SEVEN: returnValue = 7; break;
                case Value.EIGHT: returnValue = 8; break;
                case Value.NINE: returnValue = 9; break;
                case Value.TEN: returnValue = 10; break;
                case Value.JACK: returnValue = 10; break;
                case Value.QUEEN: returnValue = 10; break;
                case Value.KING: returnValue = 10; break;
                case Value.ACE: returnValue = 11; break;
            }
            return returnValue;
        }

        public int IntegerValue()
        {
            return integerValue;
        }

        public Value GetValue()
        {
            return value;
        }

        public Suit GetSuit()
        {
            return suit;
        }

        public bool IsAnAce()
        {
            return (value == Value.ACE);
        }
    }
}
