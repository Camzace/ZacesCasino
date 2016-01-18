using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZacesCasino
{
    class SlotReel
    {
        public enum ReelType
        {
            WILD, HADOUKEN, LAURA, CHUN, MIKA, KARIN, ACE, KING, QUEEN, JACK, TEN
        }

        ReelType type;

        public SlotReel(int i)
        {
            type = (ReelType)i;
        }

        public ReelType GetReel()
        {
            return type;
        }

        public int GetReelNumber()
        {
            return (int)type;
        }

        public override string ToString()
        {
            string output = "";
            switch (type)
            {
                case ReelType.WILD: output += "W"; break;
                case ReelType.HADOUKEN: output += "H"; break;
                case ReelType.LAURA: output += "L"; break;
                case ReelType.CHUN: output += "C"; break;
                case ReelType.MIKA: output += "M"; break;
                case ReelType.KARIN: output += "I"; break;
                case ReelType.ACE: output += "A"; break;
                case ReelType.KING: output += "K"; break;
                case ReelType.QUEEN: output += "Q"; break;
                case ReelType.JACK: output += "J"; break;
                case ReelType.TEN: output += "T"; break;
            }
            return output;
        }
    }
}
