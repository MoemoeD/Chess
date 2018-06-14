using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            this.initialPixelX = 100;
            this.initialPixelY = 100;
            this.state = new pieceType[this.boardX, this.boardY];
        }

        public static GobangBoard Instance()
        {
            return _gobangBoard;
        }
    }
}
