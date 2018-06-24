using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piece
{
    public abstract class BasePiece
    {
        /// <summary>
        /// 棋子X轴
        /// </summary>
        protected int pieceX { get; set; }

        /// <summary>
        /// 棋子Y轴
        /// </summary>
        protected int pieceY { get; set; }

        /// <summary>
        /// 棋子状态
        /// </summary>
        protected pieceType? state { get; set; }

        /// <summary>
        /// 上一枚棋子状态
        /// </summary>
        protected static pieceType? lastState { get; set; }

        /// <summary>
        /// 棋子状态类型
        /// </summary>
        public enum pieceType
        {
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
