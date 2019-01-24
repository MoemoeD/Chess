using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Board
{
    public class GobangBoard : BaseBoard
    {
        private static GobangBoard _gobangBoard = new GobangBoard();

        private GobangBoard()
        {
            this.boardX = 19;
            this.boardY = 19;
            this.gapPixel = 40;
            this.initialPixelx = 100;
            this.initialPixely = 50;
            this.mainColor = Color.Black;
            this.bgColor = Color.White;
            this.state = new boardType[this.boardX, this.boardY];
            this.records = new List<boardType[,]>();
            this.records.Add(new boardType[this.boardX, this.boardY]);
        }

        public static GobangBoard Instance()
        {
            return _gobangBoard;
        }

        /// <summary>
        /// 下棋
        /// </summary>
        /// <param name="point"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool SetState(Point Point, string type)
        {
            boardType boardType;
            if (!Enum.TryParse<boardType>(type, out boardType))
            {
                return false;
            }

            //已被占用返回false
            if (this.state[Point.X, Point.Y] != boardType.Blank)
            {
                return false;
            }

            this.state[Point.X, Point.Y] = boardType;
            this.records.Add((boardType[,])this.state.Clone());

            return true;
        }

        /// <summary>
        /// 绘制棋盘
        /// </summary>
        /// <param name="form"></param>
        public void DrawBoard(Form form)
        {
            Pen pen = new Pen(this.mainColor);

            Graphics graphics = form.CreateGraphics();
            graphics.Clear(this.bgColor);

            for (int i = 0; i < this.boardX; i++)
            {
                Point a = this.GetRealPointByBoardPoint(i, 0);
                Point b = this.GetRealPointByBoardPoint(i, this.boardY - 1);
                graphics.DrawLine(pen, a, b);
            }
            for (int i = 0; i < this.boardY; i++)
            {
                Point a = this.GetRealPointByBoardPoint(0, i);
                Point b = this.GetRealPointByBoardPoint(this.boardX - 1, i);
                graphics.DrawLine(pen, a, b);
            }

            graphics.Dispose();
        }

        /// <summary>
        /// 判断逻辑
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool DoJudgmentLogic(out string message)
        {
            message = records.Count().ToString();
            return true;
        }
    }
}
