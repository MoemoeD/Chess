using Board;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Piece
{
    public class WeiqiPiece : BasePiece
    {
        public WeiqiPiece(int pixelx, int pixely)
        {
            board = WeiqiBoard.Instance();
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
            if (!board.SetState(this.pieceX, this.pieceY, this.state))
            {
                this.state = BaseBoard.boardType.Blank;
                return;
            }

            lastState = this.state;
        }

        /// <summary>
        /// 绘制棋子
        /// </summary>
        /// <param name="form"></param>
        public override void DrawPiece(Form form)
        {
            //暂时通过是否拥有状态来判断是否初始化成功
            if (this.state == BaseBoard.boardType.Blank)
            {
                return;
            }

            DrawSetPiece(form, board.GetRealPointByBoardPoint(this.pieceX, this.pieceY), this.pieceRadius, Color.FromName(Enum.GetName(typeof(BaseBoard.boardType), this.state)), this.pieceFrameColor);

            //foreach (var p in board.changePoints)
            //{
            //    DrawRemovePiece(form, weiqiBoard.GetRemovePieceRect(p.pieceX, p.pieceY), weiqiBoard.GetRemovePieceLines(p.pieceX, p.pieceY), weiqiBoard.GetMainColor(), weiqiBoard.GetBgColor());
            //}
        }
    }
}
