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
            this.winLength = 5;
        }

        public static GobangBoard Instance()
        {
            return _gobangBoard;
        }

        /// <summary>
        /// 几子获胜
        /// </summary>
        private int winLength { get; set; }

        /// <summary>
        /// 获胜高亮
        /// </summary>
        private List<Point> winPoint { get; set; }

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
            winPoint = new List<Point>();
            //横向
            List<Point> t = new List<Point>();
            for (int X = 0; X < this.boardX; X++)
            {
                for (int Y = 0; Y < this.boardY; Y++)
                {
                    //横向
                    if (X != 0 && state[X, Y] != state[X - 1, Y])
                    {
                        if (t.Where(o => o.Y == Y).Count() >= this.winLength)
                        {
                            if (state[X - 1, Y] != boardType.Blank)
                            {
                                winPoint.AddRange(t.Where(o => o.Y == Y));
                            }
                        }
                        t.RemoveAll(o => o.Y == Y);
                    }
                    t.Add(new Point(X, Y));


                }
            }

            message = "";
            foreach (Point p in winPoint)
            {
                message += "X:" + p.X + ",Y:" + p.Y + ";";
            }


            if (message == "")
            {
                return false;
            }
            return true;
        }
    }
}
