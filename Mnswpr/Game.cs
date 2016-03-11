using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Mnswpr
{
    class Game
    {
        private int x;
        private int y;
        private int size;
        private int totalMines;
        private int openBlocks;
        private int currentFlags;
        private int rows;
        private int columns;
        private List<List<Block>> blocks;
        private int[] dr;
        private int[] dc;

        private bool isGameLost;
        private bool isGameWon;

        private bool isFirstMove;

        public Game(int x, int y, int size, int rows, int columns, int mines)
        {
            this.x = x;
            this.y = y;
            this.size = size;
            totalMines = mines;
            currentFlags = totalMines;
            this.rows = rows;
            this.columns = columns;
            dr = new int[] { -1, -1, -1, 0, 1, 1,  1,  0 };
            dc = new int[] { -1,  0,  1, 1, 1, 0, -1, -1 };
            isFirstMove = true;

            InitializeBlocks();            
        }

        public void LeftClick(int mouseX, int mouseY)
        {
            int row = -1;
            int col = -1;
            if (IndexOf(mouseX, mouseY, ref row, ref col))
            {
                if (!blocks[row][col].IsFlagged)
                {
                    if (isFirstMove)
                    {
                        GenerateMines(row, col);
                        isFirstMove = false;
                        FloodFill(row, col);
                    }
                    else if (blocks[row][col].IsLandMine)
                    {
                        isGameLost = true;
                        isGameWon = false;
                    }
                    else
                    {
                        FloodFill(row, col);
                    }
                }
            }

            if (openBlocks + totalMines == rows * columns)
            {
                isGameWon = true;
                isGameLost = false;
            }
        }

        private void FloodFill(int row, int col)
        {
            if (blocks[row][col].IsHidden && !blocks[row][col].IsFlagged)
            {
                if (blocks[row][col].MinesAround > 0)
                {
                    blocks[row][col].IsHidden = false;
                    openBlocks++;
                }
                else if (blocks[row][col].MinesAround == 0)
                {
                    blocks[row][col].IsHidden = false;
                    openBlocks++;
                    for (int i = 0; i != 8; ++i)
                    {
                        int trow = row + dr[i];
                        int tcol = col + dc[i];
                        if (0 <= trow && trow < rows && 0 <= tcol && tcol < columns)
                        {
                            FloodFill(trow, tcol);
                        }
                    }
                }
            }
        }

        public void RightClick(int mouseX, int mouseY)
        {
            int row = -1;
            int col = -1;
            if (IndexOf(mouseX, mouseY, ref row, ref col))
            {
                if (blocks[row][col].IsHidden)
                {
                    blocks[row][col].IsFlagged = !blocks[row][col].IsFlagged;
                    --currentFlags;
                }
            }
        }

        private bool IndexOf(int mouseX, int mouseY, ref int row, ref int col)
        {
            for (int i = 0; i != rows; ++i)
            {
                for (int j = 0; j != columns; ++j)
                {
                    if (blocks[i][j].IsInRange(mouseX, mouseY))
                    {
                        row = i;
                        col = j;
                        return true;
                    }
                }
            }
            return false;
        }

        private void InitializeBlocks()
        {
            blocks = new List<List<Block>>();
            for (int i = 0; i != rows; ++i)
            {
                List<Block> temp = new List<Block>();
                for (int j = 0; j != columns; ++j)
                {
                    temp.Add(new Block(j * size + x, i * size + y, size, true, false, 0));
                }
                blocks.Add(temp);
            }
        }

        private void GenerateMines(int clickedRow, int clickedCol)
        {
            Random rnd = new Random();
            int counter = 0;
            while (counter != totalMines)
            {
                int row = rnd.Next(rows);
                int col = rnd.Next(columns);
                if (!blocks[row][col].IsLandMine && row != clickedRow && col != clickedCol)
                {
                    blocks[row][col].IsLandMine = true;
                    ++counter;
                }
            }

            for (int row = 0; row != rows; ++row)
            {
                for (int col = 0; col != columns; ++col)
                {
                    if (blocks[row][col].IsLandMine)
                    {
                        for (int k = 0; k != 8; ++k)
                        {
                            int x = row + dr[k];
                            int y = col + dc[k];
                            if (0 <= x && x < rows && 0 <= y && y < columns)
                            {
                                blocks[x][y].MinesAround += 1;
                            }
                        }
                    }
                }
            }
        }

        public void Display(Graphics g, bool showMines)
        {
            for (int i = 0; i != rows; ++i)
            {
                for (int j = 0; j != columns; ++j)
                {
                    if (blocks[i][j].IsLandMine && !blocks[i][j].IsFlagged)
                    {
                        if (showMines)
                        {
                            blocks[i][j].IsHidden = false;
                        }
                        else
                            blocks[i][j].IsHidden = true;
                    }
                    // TO-DO: flag missed case display
                    blocks[i][j].Draw(g);
                }
            }
            
        }

        public bool IsGameLost
        {
            get
            {
                return isGameLost;
            }
        }

        public bool IsGameWon
        {
            get
            {
                return isGameWon;
            }
        }

        public int Flags
        {
            get { return currentFlags; }
        }
    }
}
