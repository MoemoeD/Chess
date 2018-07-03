using Board;
using Piece;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            board.DrawBoard(this);
        }

        private void Main_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            GobangPiece piece = new GobangPiece(e.X, e.Y);
            piece.DrawPiece(this);
        }
    }
}
