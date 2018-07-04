using Board;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Piece
{
    public class GobangPiece : BasePiece
    {
        GobangBoard gobangBoard = GobangBoard.Instance();

        public GobangPiece(int pixelx, int pixely)
        {
            this.pieceRadius = 15;
            this.judgeRadius = 15;
            this.pieceMainColor = Color.Black;
            this.pieceBgColor = Color.White;

            //判断是否点击在规定范围内
            if (!ConvertxyToXY(pixelx, pixely))
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

            if (!gobangBoard.SetState(new Point(this.pieceX, this.pieceY), Enum.GetName(typeof(pieceType), this.state)))
            {
                this.state = null;
                return;
            }

            lastState = this.state;
        }

        /// <summary>
        /// 棋子颜色
        /// </summary>
        private Color pieceMainColor { get; set; }

        /// <summary>
        /// 棋子反色
        /// </summary>
        private Color pieceBgColor { get; set; }

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
        /// <param name="pixelx"></param>
        /// <param name="pixely"></param>
        /// <returns></returns>
        private bool ConvertxyToXY(int pixelx, int pixely)
        {
            Point Point;
            if (!gobangBoard.GetBoardRangeByRealPoint(pixelx, pixely, out Point))
            {
                return false;
            }

            //超出半径
            Point point = gobangBoard.GetRealPointByBoardPoint(Point.X, Point.Y);
            if (Math.Pow(point.X - pixelx, 2) + Math.Pow(point.Y - pixely, 2) > Math.Pow(this.judgeRadius, 2))
            {
                return false;
            }

            this.pieceX = Point.X;
            this.pieceY = Point.Y;
            return true;
        }

        /// <summary>
        /// 绘制棋子
        /// </summary>
        /// <param name="form"></param>
        public void DrawPiece(Form form)
        {
            Point point = gobangBoard.GetRealPointByBoardPoint(this.pieceX, this.pieceY);

            if (this.state == pieceType.Black)
            {
                DrawSetBlack(form, point);
            }
            else if (this.state == pieceType.White)
            {
                DrawSetWhite(form, point);
            }
        }

        /// <summary>
        /// 绘制黑棋落子
        /// </summary>
        /// <param name="form"></param>
        private void DrawSetBlack(Form form, Point point)
        {
            Graphics graphics = form.CreateGraphics();

            SolidBrush brush = new SolidBrush(this.pieceMainColor);
            graphics.FillEllipse(brush, point.X - this.pieceRadius, point.Y - this.pieceRadius, 2 * this.pieceRadius, 2 * this.pieceRadius);

            graphics.Dispose();
        }

        /// <summary>
        /// 绘制白棋落子
        /// </summary>
        /// <param name="form"></param>
        private void DrawSetWhite(Form form, Point point)
        {
            Graphics graphics = form.CreateGraphics();

            SolidBrush brush = new SolidBrush(this.pieceBgColor);
            graphics.FillEllipse(brush, point.X - this.pieceRadius, point.Y - this.pieceRadius, 2 * this.pieceRadius, 2 * this.pieceRadius);

            Pen pen = new Pen(this.pieceMainColor);
            graphics.DrawEllipse(pen, point.X - this.pieceRadius, point.Y - this.pieceRadius, 2 * this.pieceRadius, 2 * this.pieceRadius);

            graphics.Dispose();
        }
    }
}
