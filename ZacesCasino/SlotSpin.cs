using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZacesCasino
{
    class SlotSpin
    {
        /* 0  1  2  3  4
        * 5  6  7  8  9
        * 10 11 12 13 14
        */
        SlotReel[] reelArray;

        double[] probArray = new double[] { 0.04, 0.07, 0.14, 0.21, 0.28, 0.35, 0.48, 0.61, 0.74, 0.87, 1.0 };
        double[] luckyArray = new double[] { 0.24, 0.27, 0.39, 0.51, 0.63, 0.75, 0.80, 0.85, 0.90, 0.95, 1.0 };

        int[,] combinations = new int[,] {{5, 6, 7, 8, 9}, {0, 1, 2, 3, 4}, {10, 11, 12, 13, 14}, {0, 6, 12, 8, 4}, {10, 6, 2, 8, 14},
                                          {5, 1, 2, 3, 9}, {5, 11, 12, 13, 9}, {0, 1, 7, 13, 14}, {10, 11, 7, 3, 4}, {5, 11, 7, 3, 9},
                                          {5, 1, 7, 13, 9}, {0, 6, 7, 8, 4}, {10, 6, 7, 8, 14}, {0, 6, 2, 8, 4}, {10, 6, 12, 8, 14},
                                          {5, 6, 2, 8, 9}, {5, 6, 12, 8, 9}, {0, 1, 12, 3, 4}, {10, 11, 2, 13, 14}, {0, 11, 12, 13, 4},
                                          {10, 1, 2, 3, 14}, {5, 11, 2, 13, 4}, {5, 1, 12, 3, 9}, {0, 11, 2, 13, 4}, {10, 1, 12, 3, 14}};

        //                                  WILD           HADOUKEN            LAURA                  CHUN                   MIKA                 KARIN
        int[,] payouts = new int[,] {{0, 0, 0, 0, 5000}, {0, 0, 0, 0, 0}, {0, 0, 75, 250, 500}, {0, 0, 60, 250, 600}, {0, 0, 50, 200, 400}, {0, 0, 30, 150, 1000},
                                     {0, 0, 10, 50, 125}, {0, 0, 10, 50, 125}, {0, 0, 10, 50, 125}, {0, 0, 10, 50, 125}, {0, 0, 10, 50, 125}}; // ACE KING QUEEN JACK TEN

        public SlotSpin(Random rng)
        {
            reelArray = new SlotReel[15];
            for (int i = 0; i < 15; i++)
            {
                double[] array;
                if (rng.NextDouble() <= 0.01) array = luckyArray;
                else array = probArray;
                double num = rng.NextDouble();
                int j;
                for (j = 0; j < 13; j++)
                {
                    if (num <= array[j])
                    {
                        break;
                    }
                }
                reelArray[i] = new SlotReel(j);
            }
        }

        public int[] GetReelArray()
        {
            int[] array = new int[15];
            for (int i = 0; i < 15; i++)
            {
                array[i] = reelArray[i].GetReelNumber();
            }
            return array;
        }

        public int[] GetCombination(int combo)
        {
            return new int[] {combinations[combo, 0], combinations[combo, 1], combinations[combo, 2], combinations[combo, 3], combinations[combo, 4]};
        }

        public override string ToString()
        {
            string output = "";
            for (int i = 0; i < 15; i++)
            {
                output += reelArray[i].ToString() + " ";
                if (i % 5 == 4) output += "\n\n";
            }
            return output;
        }

        public bool CheckForBonus()
        {
            int hadoukens = 0;
            for (int i = 0; i < 15; i++)
            {
                if (reelArray[i].GetReel() == SlotReel.ReelType.HADOUKEN) hadoukens++;
            }
            if (hadoukens >= 3) return true;
            else return false;
        }

        public int CheckForWin(int lines)
        {
            int winnings = 0;
            for (int i = 0; i < lines; i++)
            {
                winnings += CheckCombination(new SlotReel[] { reelArray[combinations[i, 0]], reelArray[combinations[i, 1]], reelArray[combinations[i, 2]], reelArray[combinations[i, 3]], reelArray[combinations[i, 4]] }, i);
            }
            return winnings;
        }

        public bool[] CheckWinningCombinations(int lines)
        {
            bool[] combos = new bool[25];
            for (int i = 0; i < 25; i++)
            {
                if (CheckCombination(new SlotReel[] { reelArray[combinations[i, 0]], reelArray[combinations[i, 1]], reelArray[combinations[i, 2]], reelArray[combinations[i, 3]], reelArray[combinations[i, 4]] }, i)
                            > 0) combos[i] = true;
            }
            return combos;
        }

        public int CheckNumberOfWinningLines(int lines)
        {
            int accum = 0;
            for (int i = 0; i < 25; i++)
            {
                if (CheckCombination(new SlotReel[] { reelArray[combinations[i, 0]], reelArray[combinations[i, 1]], reelArray[combinations[i, 2]], reelArray[combinations[i, 3]], reelArray[combinations[i, 4]] }, i)
                            > 0) accum++;
            }
            return accum;
        }


        public int CheckCombination(SlotReel[] combo, int num)
        {
            SlotReel.ReelType currentType = SlotReel.ReelType.WILD;
            SlotReel.ReelType newType;
            int i;
            for (i = 0; i < 5; i++)
            {
                newType = combo[i].GetReel();
                if (newType != currentType)
                {
                    if (currentType == SlotReel.ReelType.WILD) currentType = newType;
                    else if (newType == SlotReel.ReelType.WILD) { } //do nothing
                    else break;
                }
            }
            return payouts[(int)currentType, i - 1];
        }
    }
}
