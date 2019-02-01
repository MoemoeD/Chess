using Board;
using System.Drawing;

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
        protected BaseBoard.boardType state { get; set; }

        /// <summary>
        /// 上一枚棋子状态
        /// </summary>
        protected static BaseBoard.boardType lastState { get; set; }

        /// <summary>
        /// 棋子边框颜色
        /// </summary>
        protected Color pieceFrameColor { get; set; }

        /// <summary>
        /// 棋子半径
        /// </summary>
        protected int pieceRadius { get; set; }

        /// <summary>
        /// 判定半径
        /// </summary>
        protected int judgeRadius { get; set; }
    }
}
