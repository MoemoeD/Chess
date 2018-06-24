using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Board
{
    public class GobangBoard : BaseBoard
    {
        private static GobangBoard _gobangBoard = new GobangBoard();

        private GobangBoard()
        {
            this.boardX = 19;
            this.boardY = 19;
            this.gapPixel = 40;
            this.initialPixelx = 100;
            this.initialPixely = 80;
            this.mainColor = Color.Black;
            this.bgColor = Color.White;
            this.state = new boardType[this.boardX, this.boardY];
        }

        public static GobangBoard Instance()
        {
            return _gobangBoard;
        }
    }
}
