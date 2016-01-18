using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZacesCasino
{
    public class Player
    {
        string name;
        int chips;

        public Player(string n, int c)
        {
            name = n;
            chips = c;
        }

        public string GetName()
        {
            return name;
        }

        public int GetChips()
        {
            return chips;
        }

        public void SetName(string n)
        {
            name = n;
        }

        public void SetChips(int c)
        {
            chips = c;
        }

        public void AddChips(int c)
        {
            chips += c;
        }
    }
}
