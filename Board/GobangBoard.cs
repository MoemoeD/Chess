using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Board
{
    public class GobangBoard : BaseBoard
    {
        /// <summary>
        /// 几子获胜
        /// </summary>
        private int winLength { get; set; }

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
            this.count = 0;
            this.winLength = 5;
        }

        public static GobangBoard Instance()
        {
            return _gobangBoard;
        }

        /// <summary>
        /// 判断逻辑
        /// </summary>
        /// <param name="pieceX"></param>
        /// <param name="pieceY"></param>
        /// <param name="boardType"></param>
        protected override void DoJudgmentLogic(int pieceX, int pieceY, boardType boardType)
        {
            List<pendingPoint> winPoints = new List<pendingPoint>();
            //横向
            List<pendingPoint> t = new List<pendingPoint>();
            //纵向
            List<pendingPoint> p = new List<pendingPoint>();
            //左
            List<pendingPoint> l = new List<pendingPoint>();
            //右
            List<pendingPoint> r = new List<pendingPoint>();
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
                                winPoints.AddRange(t.Where(o => o.pieceY == Y));
                            }
                        }
                        t.RemoveAll(o => o.pieceY == Y);
                    }
                    t.Add(new pendingPoint(X, Y, state[X, Y]));
                    if (X == this.boardX - 1)
                    {
                        if (t.Where(o => o.pieceY == Y).Count() >= this.winLength)
                        {
                            if (state[X - 1, Y] != boardType.Blank)
                            {
                                winPoints.AddRange(t.Where(o => o.pieceY == Y));
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
                                winPoints.AddRange(p.Where(o => o.pieceX == X));
                            }
                        }
                        p.RemoveAll(o => o.pieceX == X);
                    }
                    p.Add(new pendingPoint(X, Y, state[X, Y]));
                    if (Y == this.boardY - 1)
                    {
                        if (p.Where(o => o.pieceX == X).Count() >= this.winLength)
                        {
                            if (state[X, Y - 1] != boardType.Blank)
                            {
                                winPoints.AddRange(p.Where(o => o.pieceX == X));
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
                                winPoints.AddRange(l.Where(o => o.pieceY == X + Y - o.pieceX));
                            }
                        }
                        l.RemoveAll(o => o.pieceY == X + Y - o.pieceX);
                    }
                    l.Add(new pendingPoint(X, Y, state[X, Y]));
                    if (X == this.boardX - 1 || Y == 0)
                    {
                        if (l.Where(o => o.pieceY == X + Y - o.pieceX).Count() >= this.winLength)
                        {
                            if (state[X - 1, Y + 1] != boardType.Blank)
                            {
                                winPoints.AddRange(l.Where(o => o.pieceY == X + Y - o.pieceX));
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
                                winPoints.AddRange(r.Where(o => o.pieceY == Y - X + o.pieceX));
                            }
                        }
                        r.RemoveAll(o => o.pieceY == Y - X + o.pieceX);
                    }
                    r.Add(new pendingPoint(X, Y, state[X, Y]));
                    if (X == this.boardX - 1 || Y == this.boardY - 1)
                    {
                        if (r.Where(o => o.pieceY == Y - X + o.pieceX).Count() >= this.winLength)
                        {
                            if (state[X - 1, Y - 1] != boardType.Blank)
                            {
                                winPoints.AddRange(r.Where(o => o.pieceY == Y - X + o.pieceX));
                            }
                        }
                        r.RemoveAll(o => o.pieceY == Y - X + o.pieceX);
                    }
                }
            }

            foreach (var winPoint in winPoints)
            {
                logs.Add(new log(winPoint.pieceX, winPoint.pieceY, winPoint.state, winPoint.state, actionType.Victory, this.count));
            }

            this.records.Add((boardType[,])this.state.Clone());
        }
    }
}
