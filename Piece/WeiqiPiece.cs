using Board;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Piece
{
    public class WeiqiPiece : BasePiece
    {
        WeiqiBoard weiqiBoard = WeiqiBoard.Instance();

        public WeiqiPiece(int pixelx, int pixely)
        {
            this.pieceRadius = 15;
            this.judgeRadius = 15;
            this.pieceFrameColor = Color.Black;

            Point Point = new Point();
            //判断是否点击在规定范围内
            if (!ConvertxyToXY(pixelx, pixely, out Point))
            {
                return;
            }
            this.pieceX = Point.X;
            this.pieceY = Point.Y;

            //判断棋子状态类型
            if (lastState == BaseBoard.boardType.Blank || lastState == BaseBoard.boardType.White)
            {
                this.state = BaseBoard.boardType.Black;
            }
            else if (lastState == BaseBoard.boardType.Black)
            {
                this.state = BaseBoard.boardType.White;
            }
            else
            {
                return;
            }

            //下棋
            if (!weiqiBoard.SetState(this.pieceX, this.pieceY, this.state))
            {
                this.state = BaseBoard.boardType.Blank;
                return;
            }

            lastState = this.state;
        }

        /// <summary>
        /// 点击坐标转换为棋盘坐标
        /// </summary>
        /// <param name="pixelx"></param>
        /// <param name="pixely"></param>
        /// <returns></returns>
        private bool ConvertxyToXY(int pixelx, int pixely, out Point Point)
        {
            if (!weiqiBoard.GetBoardRangeByRealPoint(pixelx, pixely, out Point))
            {
                return false;
            }

            //超出半径
            Point point = weiqiBoard.GetRealPointByBoardPoint(Point.X, Point.Y);
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
        public void DrawPiece(Form form)
        {
            //暂时通过是否拥有状态来判断是否初始化成功
            if (this.state != BaseBoard.boardType.Blank)
            {
                DrawSetPiece(form, weiqiBoard.GetRealPointByBoardPoint(this.pieceX, this.pieceY), this.pieceRadius, Color.FromName(Enum.GetName(typeof(BaseBoard.boardType), this.state)), this.pieceFrameColor);
            }
        }

        /// <summary>
        /// 绘制落子
        /// </summary>
        /// <param name="form"></param>
        /// <param name="point"></param>
        /// <param name="pieceFrameColor"></param>
        private void DrawSetPiece(Form form, Point point, int pieceRadius, Color pieceColor, Color pieceFrameColor)
        {
            Graphics graphics = form.CreateGraphics();

            SolidBrush brush = new SolidBrush(pieceColor);
            graphics.FillEllipse(brush, point.X - pieceRadius, point.Y - pieceRadius, 2 * pieceRadius, 2 * pieceRadius);

            Pen pen = new Pen(pieceFrameColor);
            graphics.DrawEllipse(pen, point.X - pieceRadius, point.Y - pieceRadius, 2 * pieceRadius, 2 * pieceRadius);

            graphics.Dispose();
        }
    }
}
