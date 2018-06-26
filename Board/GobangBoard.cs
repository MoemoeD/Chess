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
            this.records = new List<boardType[,]>();
            this.records.Add(new boardType[this.boardX, this.boardY]);
        }

        public static GobangBoard Instance()
        {
            return _gobangBoard;
        }

        /// <summary>
        /// 下棋
        /// </summary>
        /// <param name="p"></param>
        public bool SetState(Point p, boardType bt)
        {
            //已被占用返回false
            if (this.state[p.X, p.Y] != boardType.Blank)
            {
                return false;
            }

            this.state[p.X, p.Y] = bt;
            boardType[,] _state = this.state;
            this.records.Add(_state);

            return true;
        }
    }
}
