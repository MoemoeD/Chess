using Board;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Piece
{
    public class GobangPiece : BasePiece
    {
        /// <summary>
        /// 获胜棋子半径
        /// </summary>
        private int pieceWinRadius { get; set; }

        public GobangPiece(int pixelx, int pixely)
        {
            board = GobangBoard.Instance();
            this.pieceRadius = 15;
            this.judgeRadius = 15;
            this.pieceWinRadius = 20;
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

            List<Board.BaseBoard.log> logs = board.GetCurrentLog().OrderBy(o => o.action).ToList();
            foreach (var log in logs)
            {
                if (log.action == BaseBoard.actionType.Add)
                {
                    DrawSetPiece(form, board.GetRealPointByBoardPoint(log.pieceX, log.pieceY), this.pieceRadius, Color.FromName(Enum.GetName(typeof(BaseBoard.boardType), log.state)), this.pieceFrameColor);
                }

                if (log.action == BaseBoard.actionType.Victory)
                {
                    DrawSetPiece(form, board.GetRealPointByBoardPoint(log.pieceX, log.pieceY), this.pieceWinRadius, Color.FromName(Enum.GetName(typeof(BaseBoard.boardType), log.state)), this.pieceFrameColor);
                }
            }
        }
    }
}
