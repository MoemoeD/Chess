using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Board
{
    public abstract class BaseBoard
    {
        /// <summary>
        /// 棋盘X轴
        /// </summary>
        public int boardX { get; set; }

        /// <summary>
        /// 棋盘Y轴
        /// </summary>
        public int boardY { get; set; }

        /// <summary>
        /// 间隙
        /// </summary>
        public int gapPixel { get; set; }

        /// <summary>
        /// 棋盘基准坐标X
        /// </summary>
        public int initialPixelx { get; set; }

        /// <summary>
        /// 棋盘基准坐标Y
        /// </summary>
        public int initialPixely { get; set; }

        /// <summary>
        /// 棋盘颜色
        /// </summary>
        public Color mainColor { get; set; }

        /// <summary>
        /// 棋盘背景颜色
        /// </summary>
        public Color bgColor { get; set; }

        /// <summary>
        /// 棋盘点位状态
        /// </summary>
        protected boardType[,] state { get; set; }

        /// <summary>
        /// 棋盘点位状态类型
        /// </summary>
        public enum boardType
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
        /// 获得点的具体坐标
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        public Point GetPoint(int X, int Y)
        {
            int x = this.initialPixelx + X * this.gapPixel;
            int y = this.initialPixely + Y * this.gapPixel;
            return new Point(x, y);
        }
    }
}
