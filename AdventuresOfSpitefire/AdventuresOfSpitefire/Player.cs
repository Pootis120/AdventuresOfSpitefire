using System;
using System.Collections.Generic;
using System.Text;

namespace AdventuresOfSpitefire
{
    public class Player
    {
        public int pos;
        public bool isAlive;

        public Player(int Pos, bool Alive)
        {
            pos = Pos;
            isAlive = Alive;
        }
        public int moveUp()
        {
            int retPos;
            if (pos == 4)
            {
                pos = 565;
                retPos = pos - 1;
                return retPos;
            }
            else
            {
                pos -= 79;
                retPos = pos - 1;
                return retPos;
            }
        }
        public int moveDown()
        {
            int retPos;
            if (pos == 564)
            {
                pos = 4;
                retPos = pos - 1;
                return retPos;
            }
            else
            {
                pos += 80;
                retPos = pos - 1;
                return retPos;
            }
        }
    }
}
