using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Board
{
    public abstract class BaseBoard
    {
        /// <summary>
        /// 棋盘X轴
        /// </summary>
        protected int boardX { get; set; }

        /// <summary>
        /// 棋盘Y轴
        /// </summary>
        protected int boardY { get; set; }

        /// <summary>
        /// 间隙
        /// </summary>
        protected int gapPixel { get; set; }

        /// <summary>
        /// 棋盘基准坐标X
        /// </summary>
        protected int initialPixelx { get; set; }

        /// <summary>
        /// 棋盘基准坐标Y
        /// </summary>
        protected int initialPixely { get; set; }

        /// <summary>
        /// 棋盘颜色
        /// </summary>
        protected Color mainColor { get; set; }

        /// <summary>
        /// 棋盘背景颜色
        /// </summary>
        protected Color bgColor { get; set; }

        /// <summary>
        /// 棋盘点位状态
        /// </summary>
        protected boardType[,] state { get; set; }

        /// <summary>
        /// 棋谱
        /// </summary>
        protected List<boardType[,]> records { get; set; }

        /// <summary>
        /// 棋盘点位状态类型
        /// </summary>
        protected enum boardType
        {
            /// <summary>
            /// 空
            /// </summary>
            Blank,

            /// <summary>
            /// 黑
            /// </summary>
            Black,

            /// <summary>
            /// 白
            /// </summary>
            White,
        }

        /// <summary>
        /// 获得棋盘点的具体坐标
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        public Point GetRealPointByBoardPoint(int X, int Y)
        {
            int pixelx = this.initialPixelx + X * this.gapPixel;
            int pixely = this.initialPixely + Y * this.gapPixel;
            return new Point(pixelx, pixely);
        }

        /// <summary>
        /// 获取坐标所在范围的棋盘点
        /// </summary>
        /// <param name="pixelx"></param>
        /// <param name="pixely"></param>
        /// <param name="Point"></param>
        /// <returns></returns>
        public bool GetBoardRangeByRealPoint(int pixelx, int pixely, out Point Point)
        {
            //xy减去基准坐标加上半个间隙除以间隙得到XY
            int X = (pixelx - this.initialPixelx + this.gapPixel / 2) / this.gapPixel;
            int Y = (pixely - this.initialPixely + this.gapPixel / 2) / this.gapPixel;

            int remainderx = (pixelx - this.initialPixelx + this.gapPixel / 2) % this.gapPixel;
            int remaindery = (pixely - this.initialPixely + this.gapPixel / 2) % this.gapPixel;

            Point = new Point(X, Y);

            //超出边界
            if (X >= this.boardX || Y >= this.boardY || remainderx < 0 || remaindery < 0)
            {
                return false;
            }

            return true;
        }
    }
}
