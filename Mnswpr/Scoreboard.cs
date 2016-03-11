using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Mnswpr
{
    class Scoreboard
    {
        private int x;
        private int y;
        private int width;
        private int height;
        private SolidBrush brush;
        private Font font;

        public Scoreboard(int x, int y, int width, int height, int fontSize)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            font = new Font("Arial", fontSize, FontStyle.Bold);
        }

        public void Display(Graphics g, int value)
        {
            brush = new SolidBrush(Color.Black);
            g.FillRectangle(brush, x, y, width, height);
            brush = new SolidBrush(Color.Green);
            g.DrawString(value.ToString(), font, brush, x + width / 3, y);
        }
    }
}
