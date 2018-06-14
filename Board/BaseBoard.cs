using System;
using System.Collections.Generic;
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
        protected int initialPixelX { get; set; }

        /// <summary>
        /// 棋盘基准坐标Y
        /// </summary>
        protected int initialPixelY { get; set; }

        /// <summary>
        /// 棋盘点位状态
        /// </summary>
        protected pieceType[,] state;

        /// <summary>
        /// 棋盘点位状态类型
        /// </summary>
        public enum pieceType
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
    }
}
