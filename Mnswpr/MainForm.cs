using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Text;
using System.IO;

namespace Mnswpr
{
    public partial class MainForm : Form
    {
        int leftX;
        int leftY;
        int size;
        int rows;
        int columns;
        int totalMines;
        int seconds;

        Game game;
        bool showMines;
        GameButton gameButton;
        Scoreboard flags;
        Scoreboard timer;
        //PrivateFontCollection pfc;        

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            leftX = 8;
            leftY = 72;
            size = 32;

            SetNewbieLevel();
            NewGame();

            showMines = false;
            flags = new Scoreboard(leftX, leftY - size, 64, 22, 16);
            timer = new Scoreboard(leftX + size * (columns - 2), leftY - size, 64, 22, 16);
        }

        //private void LoadFont()
        //{
        //    pfc = new PrivateFontCollection();
        //    Stream fontStream = this.GetType().Assembly.GetManifestResourceStream(@"res\DS-DIGI.TTF");

        //    byte[] fontdata = new byte[fontStream.Length];
        //    fontStream.Read(fontdata, 0, (int)fontStream.Length);
        //    fontStream.Close();
        //    unsafe
        //    {
        //        fixed (byte* pFontData = fontdata)
        //        {
        //            pfc.AddMemoryFont((System.IntPtr)pFontData, fontdata.Length);
        //        }
        //    }
        //}

        private void SetNewbieLevel()
        {
            rows = 9;
            columns = 9;
            totalMines = 10;
        }

        private void SetNormalLevel()
        {
            rows = 16;
            columns = 16;
            totalMines = 40;
        }

        private void SetExpertLevel()
        {
            rows = 16;
            columns = 30;
            totalMines = 99;
        }

        private void NewGame()
        {
            game = new Game(leftX, leftY, size, rows, columns, totalMines);
            timer1.Enabled = false;
            timer = new Scoreboard(leftX + size * (columns - 2), leftY - size, 64, 22, 16);
            gameButton = new GameButton(leftX + size * ((columns) / 2), leftY - (int)(1.2 * size), size);
            showMines = false;
            seconds = 0;
            
            RecalculateClientSize();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            game.Display(e.Graphics, showMines);
            gameButton.Display(e.Graphics);
            flags.Display(e.Graphics, game.Flags);
            timer.Display(e.Graphics, seconds);
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (!timer1.Enabled) 
                timer1.Enabled = true;

            if (!game.IsGameLost && !game.IsGameWon)
                gameButton.SetState(GameButton.State.Click);
            
            if (e.Button == MouseButtons.Right)
            {
                if (!game.IsGameLost && !game.IsGameWon)
                    game.RightClick(e.X, e.Y);
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (gameButton.IsInRange(e.X, e.Y))
                    NewGame();
                else
                {
                    if (!game.IsGameLost && !game.IsGameWon)
                        game.LeftClick(e.X, e.Y);
                }
            }
            Invalidate();
        }

        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (game.IsGameWon)
            {
                gameButton.SetState(GameButton.State.Win);
                timer1.Enabled = false;
            }
            else if (game.IsGameLost)
            {
                gameButton.SetState(GameButton.State.Loose);
                showMines = true;
                timer1.Enabled = false;
            }
            else if (!game.IsGameLost && !game.IsGameWon)
                gameButton.SetState(GameButton.State.Smile);

            Invalidate();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.X)
            {
                showMines = !showMines;
                Invalidate();
            }
        }

        private void RecalculateClientSize()
        {
            this.SetClientSizeCore(columns * size + leftX * 2, rows * size + (int)(leftY * 1.2));
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            // This is the only line needed for anti-aliasing to be turned on.
            pe.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // the next two lines of code (not comments) are needed to get the highest 
            // possible quiality of anti-aliasing. Remove them if you want the image to render faster.
            pe.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            pe.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            // this line is needed for .net to draw the contents.
            base.OnPaint(pe);
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame();
            Invalidate();
        }

        private void newbieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetNewbieLevel(); 
            NewGame();
            Invalidate();
        }

        private void normalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetNormalLevel();
            NewGame(); 
            Invalidate();
        }

        private void expertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetExpertLevel();
            NewGame();
            Invalidate();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            seconds += 1;
            Invalidate();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
