using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Board;

namespace Piece
{
    public class GobangPiece : BasePiece
    {
        GobangBoard gobangBoard = GobangBoard.Instance();

        public GobangPiece(int x, int y)
        {
            this.pieceRadius = 15;
            this.judgeRadius = 15;

            //判断是否点击在规定范围内
            if (!ConvertxyToXY(x, y))
            {
                return;
            }

            if (lastState == null || lastState == pieceType.White)
            {
                this.state = pieceType.Black;
            }
            else if (lastState == pieceType.Black)
            {
                this.state = pieceType.White;
            }

            if (!gobangBoard.SetState(new Point(this.pieceX, this.pieceY), (GobangBoard.boardType)Enum.Parse(typeof(GobangBoard.boardType), Enum.GetName(typeof(pieceType), this.state))))
            {
                return;
            }

            lastState = this.state;
        }

        /// <summary>
        /// 棋子半径
        /// </summary>
        private int pieceRadius { get; set; }

        /// <summary>
        /// 判断半径
        /// </summary>
        private int judgeRadius { get; set; }

        /// <summary>
        /// 点击坐标转换为棋盘坐标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private bool ConvertxyToXY(int x, int y)
        {
            //xy减去基准坐标加上半个间隙除以间隙得到XY
            int X = (x - gobangBoard.initialPixelx + gobangBoard.gapPixel / 2) / gobangBoard.gapPixel;
            int Y = (y - gobangBoard.initialPixely + gobangBoard.gapPixel / 2) / gobangBoard.gapPixel;

            int remainderx = (x - gobangBoard.initialPixelx + gobangBoard.gapPixel / 2) % gobangBoard.gapPixel;
            int remaindery = (y - gobangBoard.initialPixely + gobangBoard.gapPixel / 2) % gobangBoard.gapPixel;

            if (X >= gobangBoard.boardX || Y >= gobangBoard.boardY || remainderx < 0 || remaindery < 0)
            {
                return false;
            }

            Point point = gobangBoard.GetPoint(X, Y);
            if (Math.Pow(point.X - x, 2) + Math.Pow(point.Y - y, 2) > Math.Pow(this.judgeRadius, 2))
            {
                return false;
            }

            this.pieceX = X;
            this.pieceY = Y;
            return true;
        }
    }
}
