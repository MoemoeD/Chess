using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Board
{
    public class GobangBoard : BaseBoard
    {
        /// <summary>
        /// 几子获胜
        /// </summary>
        private int winLength { get; set; }

        /// <summary>
        /// 获胜集合
        /// </summary>
        private List<Point> winPoint { get; set; }

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
            this.logs = new List<log>();
            this.winLength = 5;
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
        public bool SetState(int pieceX, int pieceY, string type)
        {
            boardType boardType;
            if (!Enum.TryParse<boardType>(type, out boardType))
            {
                return false;
            }

            //已被占用返回false
            if (this.state[pieceX, pieceY] != boardType.Blank)
            {
                return false;
            }

            //如有获胜状态，游戏结束
            if (this.logs.Where(o => o.action == actionType.Victory).Count() > 0)
            {
                return false;
            }

            this.state[pieceX, pieceY] = boardType;
            this.records.Add((boardType[,])this.state.Clone());
            this.logs.Add(new log(pieceX, pieceY, boardType, boardType.Blank, actionType.Add, logs.Count()));

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
            //纵向
            List<Point> p = new List<Point>();
            //左
            List<Point> l = new List<Point>();
            //右
            List<Point> r = new List<Point>();
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
                    if (X == this.boardX - 1)
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

                    //纵向
                    if (Y != 0 && state[X, Y] != state[X, Y - 1])
                    {
                        if (p.Where(o => o.X == X).Count() >= this.winLength)
                        {
                            if (state[X, Y - 1] != boardType.Blank)
                            {
                                winPoint.AddRange(p.Where(o => o.X == X));
                            }
                        }
                        p.RemoveAll(o => o.X == X);
                    }
                    p.Add(new Point(X, Y));
                    if (Y == this.boardY - 1)
                    {
                        if (p.Where(o => o.X == X).Count() >= this.winLength)
                        {
                            if (state[X, Y - 1] != boardType.Blank)
                            {
                                winPoint.AddRange(p.Where(o => o.X == X));
                            }
                        }
                        p.RemoveAll(o => o.X == X);
                    }

                    //左
                    if (X != 0 && Y < this.boardY - 1 && state[X, Y] != state[X - 1, Y + 1])
                    {
                        if (l.Where(o => o.Y == X + Y - o.X).Count() >= this.winLength)
                        {
                            if (state[X - 1, Y + 1] != boardType.Blank)
                            {
                                winPoint.AddRange(l.Where(o => o.Y == X + Y - o.X));
                            }
                        }
                        l.RemoveAll(o => o.Y == X + Y - o.X);
                    }
                    l.Add(new Point(X, Y));
                    if (X == this.boardX - 1 || Y == 0)
                    {
                        if (l.Where(o => o.Y == X + Y - o.X).Count() >= this.winLength)
                        {
                            if (state[X - 1, Y + 1] != boardType.Blank)
                            {
                                winPoint.AddRange(l.Where(o => o.Y == X + Y - o.X));
                            }
                        }
                        l.RemoveAll(o => o.Y == X + Y - o.X);
                    }

                    //右
                    if (X != 0 && Y != 0 && state[X, Y] != state[X - 1, Y - 1])
                    {
                        if (r.Where(o => o.Y == Y - X + o.X).Count() >= this.winLength)
                        {
                            if (state[X - 1, Y - 1] != boardType.Blank)
                            {
                                winPoint.AddRange(r.Where(o => o.Y == Y - X + o.X));
                            }
                        }
                        r.RemoveAll(o => o.Y == Y - X + o.X);
                    }
                    r.Add(new Point(X, Y));
                    if (X == this.boardX - 1 || Y == this.boardY - 1)
                    {
                        if (r.Where(o => o.Y == Y - X + o.X).Count() >= this.winLength)
                        {
                            if (state[X - 1, Y - 1] != boardType.Blank)
                            {
                                winPoint.AddRange(r.Where(o => o.Y == Y - X + o.X));
                            }
                        }
                        r.RemoveAll(o => o.Y == Y - X + o.X);
                    }
                }
            }

            message = "";
            if (this.winPoint.Count() == 0)
            {
                return false;
            }
            return true;
        }
    }
}
