using Board;
using System.Drawing;
using System.Windows.Forms;

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

        /// <summary>
        /// 点击坐标转换为棋盘坐标
        /// </summary>
        /// <param name="pixelx"></param>
        /// <param name="pixely"></param>
        /// <param name="Point"></param>
        /// <returns></returns>
        protected abstract bool ConvertxyToXY(int pixelx, int pixely, out Point Point);

        /// <summary>
        /// 绘制棋子
        /// </summary>
        /// <param name="form"></param>
        public abstract void DrawPiece(Form form);

        /// <summary>
        /// 绘制棋子
        /// </summary>
        /// <param name="form"></param>
        /// <param name="point"></param>
        /// <param name="pieceRadius"></param>
        /// <param name="pieceColor"></param>
        /// <param name="pieceFrameColor"></param>
        protected abstract void DrawSetPiece(Form form, Point point, int pieceRadius, Color pieceColor, Color pieceFrameColor);
    }
}
