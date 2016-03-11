using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Mnswpr
{
    class GameButton
    {
        int x;
        int y;
        int size;

        public enum State { Smile, Click, Loose, Win, Cheater };

        State state;
        Pen pen;

        public GameButton(int x, int y, int size)
        {
            this.x = x;
            this.y = y;
            this.size = size;
            state = State.Smile;
            pen = new Pen(Color.Gray);
        }

        public void SetState(State state)
        {
            this.state = state;
        }

        public void Display(Graphics g)
        {
            switch (state)
            {
                case State.Smile:
                    g.DrawImage(Resources.Smile, x, y, size, size);
                    break;
                case State.Click:
                    g.DrawImage(Resources.Click, x, y, size, size);
                    break;
                case State.Loose:
                    g.DrawImage(Resources.Loose, x, y, size, size);
                    break;
                case State.Win:
                    g.DrawImage(Resources.Win, x, y, size, size);
                    break;
                case State.Cheater:
                    
                    g.DrawImage(Resources.Cheater, x, y, size, size);
                    break;
                default:
                    break;
            }
            //g.DrawRectangle(pen, x, y, size, size);
        }

        public bool IsInRange(int mouseX, int mouseY)
        {
            return x <= mouseX && mouseX <= x + size && y <= mouseY && mouseY <= y + size;
        }
    }
}
