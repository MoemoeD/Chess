using Board;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Piece
{
    public abstract class BasePiece
    {
        protected BaseBoard board { get; set; }

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
        protected bool ConvertxyToXY(int pixelx, int pixely, out Point Point)
        {
            if (!board.GetBoardRangeByRealPoint(pixelx, pixely, out Point))
            {
                return false;
            }

            //超出半径
            Point point = board.GetRealPointByBoardPoint(Point.X, Point.Y);
            if (Math.Pow(point.X - pixelx, 2) + Math.Pow(point.Y - pixely, 2) > Math.Pow(this.judgeRadius, 2))
            {
                return false;
            }

            return true;
        }

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
        protected void DrawSetPiece(Form form, Point point, int pieceRadius, Color pieceColor, Color pieceFrameColor)
        {
            Graphics graphics = form.CreateGraphics();

            SolidBrush brush = new SolidBrush(pieceColor);
            graphics.FillEllipse(brush, point.X - pieceRadius, point.Y - pieceRadius, 2 * pieceRadius, 2 * pieceRadius);

            Pen pen = new Pen(pieceFrameColor);
            graphics.DrawEllipse(pen, point.X - pieceRadius, point.Y - pieceRadius, 2 * pieceRadius, 2 * pieceRadius);

            graphics.Dispose();
        }

        /// <summary>
        /// 移除棋子
        /// </summary>
        /// <param name="form"></param>
        /// <param name="rect"></param>
        /// <param name="lines"></param>
        /// <param name="mainColor"></param>
        /// <param name="bgColor"></param>
        protected void DrawRemovePiece(Form form, Rectangle rect, List<Board.WeiqiBoard.line> lines, Color mainColor, Color bgColor)
        {
            Graphics graphics = form.CreateGraphics();

            SolidBrush brush = new SolidBrush(bgColor);
            graphics.FillRectangle(brush, rect);

            Pen pen = new Pen(mainColor);
            foreach (var line in lines)
            {
                graphics.DrawLine(pen, line.p1, line.p2);
            }

            graphics.Dispose();
        }
    }
}
