using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Board;
using Piece;

namespace Chess
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Paint(object sender, PaintEventArgs e)
        {
            GobangBoard board = GobangBoard.Instance();

            Pen pen = new Pen(board.mainColor);

            Graphics graphics = this.CreateGraphics();
            graphics.Clear(board.bgColor);

            for (int i = 0; i < board.boardX; i++)
            {
                Point a = board.GetPoint(i, 0);
                Point b = board.GetPoint(i, board.boardY - 1);
                graphics.DrawLine(pen, a, b);
            }
            for (int i = 0; i < board.boardY; i++)
            {
                Point a = board.GetPoint(0, i);
                Point b = board.GetPoint(board.boardX - 1, i);
                graphics.DrawLine(pen, a, b);
            }
        }
    }
}
