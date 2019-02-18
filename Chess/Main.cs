using Board;
using Piece;
using System;
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
            try
            {
                BaseBoard board = WeiqiBoard.Instance();
                board.DrawBoard(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Main_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button != MouseButtons.Left)
                {
                    return;
                }

                BasePiece piece = new WeiqiPiece(e.X, e.Y);
                piece.DrawPiece(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
