using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Mnswpr
{
    class Block
    {
        private int x;
        private int y;
        private int size;
        private SolidBrush brush;
        private Pen pen;
        private Font font;

        private bool isHidden;
        private bool isLandMine;
        private bool isFlagged;
        private int minesAround;

        public Block(int x, int y, int size, bool isHidden, bool isLandMine, int minesAround)
        {
            this.x = x;
            this.y = y;
            this.size = size;
            this.isHidden = isHidden;
            this.isLandMine = isLandMine;
            this.minesAround = minesAround;
            
            font = new Font("Arial", size / 2, FontStyle.Bold);
        }

        public bool IsInRange(int mouseX, int mouseY)
        {
            return x <= mouseX && mouseX <= x + size && y <= mouseY && mouseY <= y + size;
        }

        private void FillHiddenBlock(Graphics g)
        {
            brush = new SolidBrush(Color.LightGray);
            g.FillRectangle(brush, x, y, size, size);
        }

        private void DrawFlag(Graphics g)
        {
            brush = new SolidBrush(Color.Red);
            g.DrawImage(Resources.Flag, x, y, size, size);
        }

        private void FillOpenBlock(Graphics g)
        {
            brush = new SolidBrush(Color.FromArgb(189, 189, 189));
            g.FillRectangle(brush, x, y, size, size);
        }

        private void DrawLandMine(Graphics g)
        {
            brush = new SolidBrush(Color.Red);
            g.FillRectangle(brush, x, y, size, size);
            brush = new SolidBrush(Color.DarkRed);
            //g.DrawString("M", font, brush, x + size / 8, y + size / 8);
            g.DrawImage(Resources.Landmine, x, y, size, size);
        }

        private void DrawHExterior(Graphics g)
        {
            pen = new Pen(Color.WhiteSmoke);
            g.DrawLine(pen, x, y, x + size, y);
            g.DrawLine(pen, x, y, x, y + size);
            pen = new Pen(Color.Gray);
            g.DrawLine(pen, x, y + size - 1, x + size - 1, y + size - 1);
            g.DrawLine(pen, x + size - 1, y, x + size - 1, y + size - 1);
        }

        private void DrawNumberOfMinesAround(Graphics g)
        {
            if (minesAround != 0)
            {
                switch (minesAround)
                {
                    case 1:
                        brush = new SolidBrush(Color.Blue);
                        break;
                    case 2:
                        brush = new SolidBrush(Color.DarkGreen);
                        break;
                    case 3:
                        brush = new SolidBrush(Color.Yellow);
                        break;
                    case 4:
                        brush = new SolidBrush(Color.DarkBlue);
                        break;
                    default:
                        brush = new SolidBrush(Color.DarkOrchid);
                        break;
                }
                g.DrawString(minesAround.ToString(), font, brush, x + size / 8, y + size / 8);
            }
        }
        

        public void Draw(Graphics g)
        {
            if (isHidden)
            {
                FillHiddenBlock(g);

                if (isFlagged)
                {
                    DrawFlag(g);
                }
                DrawHExterior(g);
            }
            else
            {
                if (isLandMine)
                {
                    DrawLandMine(g);
                }
                else
                {
                    FillOpenBlock(g);
                    DrawNumberOfMinesAround(g);
                }
                pen = new Pen(Color.Gray);
                g.DrawRectangle(pen, x, y, size, size);
            }
            
        }

        public bool IsHidden
        {
            get
            {
                return isHidden;
            }
            set
            {
                isHidden = value;
            }
        }

        public bool IsLandMine
        {
            get
            {
                return isLandMine;
            }
            set
            {
                isLandMine = value;
            }
        }

        public int MinesAround
        {
            get
            {
                return minesAround;
            }
            set
            {
                minesAround = value;
            }
        }

        public bool IsFlagged
        {
            get { return isFlagged; }
            set { isFlagged = value; }
        }
    }
}
