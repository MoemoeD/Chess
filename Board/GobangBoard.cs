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
        public List<winPoint> winPoints { get; set; }

        public class winPoint
        {
            /// <summary>
            /// X
            /// </summary>
            public int pieceX { get; set; }

            /// <summary>
            /// Y
            /// </summary>
            public int pieceY { get; set; }

            /// <summary>
            /// 点位状态
            /// </summary>
            public boardType state { get; set; }

            public winPoint(int pieceX, int pieceY, boardType state)
            {
                this.pieceX = pieceX;
                this.pieceY = pieceY;
                this.state = state;
            }
        }

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
            this.count = 0;
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
            this.count++;
            this.logs.Add(new log(pieceX, pieceY, boardType, boardType.Blank, actionType.Add, this.count));

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
        public void DoJudgmentLogic()
        {
            this.winPoints = new List<winPoint>();
            //横向
            List<winPoint> t = new List<winPoint>();
            //纵向
            List<winPoint> p = new List<winPoint>();
            //左
            List<winPoint> l = new List<winPoint>();
            //右
            List<winPoint> r = new List<winPoint>();
            for (int X = 0; X < this.boardX; X++)
            {
                for (int Y = 0; Y < this.boardY; Y++)
                {
                    //横向
                    if (X != 0 && state[X, Y] != state[X - 1, Y])
                    {
                        if (t.Where(o => o.pieceY == Y).Count() >= this.winLength)
                        {
                            if (state[X - 1, Y] != boardType.Blank)
                            {

                                this.winPoints.AddRange(t.Where(o => o.pieceY == Y));
                            }
                        }
                        t.RemoveAll(o => o.pieceY == Y);
                    }
                    t.Add(new winPoint(X, Y, state[X, Y]));
                    if (X == this.boardX - 1)
                    {
                        if (t.Where(o => o.pieceY == Y).Count() >= this.winLength)
                        {
                            if (state[X - 1, Y] != boardType.Blank)
                            {
                                this.winPoints.AddRange(t.Where(o => o.pieceY == Y));
                            }
                        }
                        t.RemoveAll(o => o.pieceY == Y);
                    }

                    //纵向
                    if (Y != 0 && state[X, Y] != state[X, Y - 1])
                    {
                        if (p.Where(o => o.pieceX == X).Count() >= this.winLength)
                        {
                            if (state[X, Y - 1] != boardType.Blank)
                            {
                                this.winPoints.AddRange(p.Where(o => o.pieceX == X));
                            }
                        }
                        p.RemoveAll(o => o.pieceX == X);
                    }
                    p.Add(new winPoint(X, Y, state[X, Y]));
                    if (Y == this.boardY - 1)
                    {
                        if (p.Where(o => o.pieceX == X).Count() >= this.winLength)
                        {
                            if (state[X, Y - 1] != boardType.Blank)
                            {
                                this.winPoints.AddRange(p.Where(o => o.pieceX == X));
                            }
                        }
                        p.RemoveAll(o => o.pieceX == X);
                    }

                    //左
                    if (X != 0 && Y < this.boardY - 1 && state[X, Y] != state[X - 1, Y + 1])
                    {
                        if (l.Where(o => o.pieceY == X + Y - o.pieceX).Count() >= this.winLength)
                        {
                            if (state[X - 1, Y + 1] != boardType.Blank)
                            {
                                this.winPoints.AddRange(l.Where(o => o.pieceY == X + Y - o.pieceX));
                            }
                        }
                        l.RemoveAll(o => o.pieceY == X + Y - o.pieceX);
                    }
                    l.Add(new winPoint(X, Y, state[X, Y]));
                    if (X == this.boardX - 1 || Y == 0)
                    {
                        if (l.Where(o => o.pieceY == X + Y - o.pieceX).Count() >= this.winLength)
                        {
                            if (state[X - 1, Y + 1] != boardType.Blank)
                            {
                                this.winPoints.AddRange(l.Where(o => o.pieceY == X + Y - o.pieceX));
                            }
                        }
                        l.RemoveAll(o => o.pieceY == X + Y - o.pieceX);
                    }

                    //右
                    if (X != 0 && Y != 0 && state[X, Y] != state[X - 1, Y - 1])
                    {
                        if (r.Where(o => o.pieceY == Y - X + o.pieceX).Count() >= this.winLength)
                        {
                            if (state[X - 1, Y - 1] != boardType.Blank)
                            {
                                this.winPoints.AddRange(r.Where(o => o.pieceY == Y - X + o.pieceX));
                            }
                        }
                        r.RemoveAll(o => o.pieceY == Y - X + o.pieceX);
                    }
                    r.Add(new winPoint(X, Y, state[X, Y]));
                    if (X == this.boardX - 1 || Y == this.boardY - 1)
                    {
                        if (r.Where(o => o.pieceY == Y - X + o.pieceX).Count() >= this.winLength)
                        {
                            if (state[X - 1, Y - 1] != boardType.Blank)
                            {
                                this.winPoints.AddRange(r.Where(o => o.pieceY == Y - X + o.pieceX));
                            }
                        }
                        r.RemoveAll(o => o.pieceY == Y - X + o.pieceX);
                    }
                }
            }

            this.records.Add((boardType[,])this.state.Clone());

            if (this.winPoints.Count() > 0)
            {
                log log = logs.Where(o => o.action == actionType.Add && o.count == this.count).First();
                logs.Add(new log(log.pieceX, log.pieceY, log.state, log.lastState, actionType.Victory, log.count));
            }
        }
    }
}
