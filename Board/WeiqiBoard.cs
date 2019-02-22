using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Board
{
    public class WeiqiBoard : BaseBoard
    {
        /// <summary>
        /// 更改集合
        /// </summary>
        public List<changePoint> changePoints { get; set; }

        public class changePoint
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

            /// <summary>
            /// 有几口气
            /// </summary>
            internal int life { get; set; }

            public changePoint(int pieceX, int pieceY, boardType state)
            {
                this.pieceX = pieceX;
                this.pieceY = pieceY;
                this.state = state;
                this.life = 0;
            }

            /// <summary>
            /// 续命
            /// </summary>
            internal void setLife()
            {
                this.life++;
            }
        }

        private static WeiqiBoard _weiqiBoard = new WeiqiBoard();

        private WeiqiBoard()
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
        }

        public static WeiqiBoard Instance()
        {
            return _weiqiBoard;
        }

        public class line
        {
            public Point p1 { get; set; }

            public Point p2 { get; set; }

            public line(Point p1, Point p2)
            {
                this.p1 = p1;
                this.p2 = p2;
            }
        }

        /// <summary>
        /// 获取移除棋子后的线
        /// </summary>
        /// <param name="pieceX"></param>
        /// <param name="pieceY"></param>
        /// <returns></returns>
        public List<line> GetRemovePieceLines(int pieceX, int pieceY)
        {
            int halfGapPixel = Convert.ToInt32(0.5 * this.gapPixel);

            Point p = GetRealPointByBoardPoint(pieceX, pieceY);

            line l1 = new line(new Point(pieceX == 0 ? p.X : p.X - halfGapPixel, p.Y), new Point(pieceX == this.boardX - 1 ? p.X : p.X + halfGapPixel, p.Y));
            line l2 = new line(new Point(p.X, pieceY == 0 ? p.Y : p.Y - halfGapPixel), new Point(p.X, pieceY == this.boardY - 1 ? p.Y : p.Y + halfGapPixel));

            return new List<line>() { l1, l2 };
        }

        /// <summary>
        /// 获取移除棋子覆盖的长方形
        /// </summary>
        /// <param name="pieceX"></param>
        /// <param name="pieceY"></param>
        /// <returns></returns>
        public Rectangle GetRemovePieceRect(int pieceX, int pieceY)
        {
            int halfGapPixel = Convert.ToInt32(0.5 * this.gapPixel);

            Point p = GetRealPointByBoardPoint(pieceX, pieceY);

            return new Rectangle(p.X - halfGapPixel, p.Y - halfGapPixel, gapPixel, gapPixel);
        }

        /// <summary>
        /// 判断逻辑
        /// </summary>
        /// <param name="pieceX"></param>
        /// <param name="pieceY"></param>
        /// <param name="boardType"></param>
        protected override void DoJudgmentLogic(int pieceX, int pieceY, boardType boardType)
        {
            this.changePoints = new List<changePoint>();
            List<changePoint> equalPoints = new List<changePoint>();

            changePoint pOriginal = new changePoint(pieceX, pieceY, boardType);
            equalPoints.Add(pOriginal);

            for (int X = -1; X <= 1; X++)
            {
                for (int Y = -1; Y <= 1; Y++)
                {
                    //只剩下上下左右四个位置
                    if (X != 0 && Y != 0 || X == 0 && Y == 0)
                        continue;
                    if (pieceX + X < 0 || pieceX + X >= this.boardX || pieceY + Y < 0 || pieceY + Y >= this.boardY)
                        continue;

                    if (state[pieceX + X, pieceY + Y] == boardType.Blank)
                    {
                        pOriginal.setLife();
                    }
                    else if (state[pieceX + X, pieceY + Y] == boardType)
                    {
                        CheckPoint(pieceX + X, pieceY + Y, state[pieceX + X, pieceY + Y], equalPoints);
                    }
                    else if (state[pieceX + X, pieceY + Y] != boardType)
                    {
                        List<changePoint> unequalPoints = new List<changePoint>();
                        CheckPoint(pieceX + X, pieceY + Y, state[pieceX + X, pieceY + Y], unequalPoints);
                        if (unequalPoints.Where(o => o.life > 0).Count() == 0)
                        {
                            changePoints.AddRange(unequalPoints);
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            if (changePoints.Count() == 0 && equalPoints.Where(o => o.life > 0).Count() == 0)
            {
                changePoints.AddRange(equalPoints);
            }

            foreach (var i in changePoints.GroupBy(o => new { o.pieceX, o.pieceY, o.state }).Select(o => o.First()))
            {
                this.logs.Add(new log(i.pieceX, i.pieceY, boardType.Blank, i.state, actionType.Remove, this.count));
                this.state[i.pieceX, i.pieceY] = boardType.Blank;
            }

            this.records.Add((boardType[,])this.state.Clone());
        }

        /// <summary>
        /// 递归判断
        /// </summary>
        /// <param name="pieceX"></param>
        /// <param name="pieceY"></param>
        /// <param name="boardType"></param>
        private void CheckPoint(int pieceX, int pieceY, boardType boardType, List<changePoint> cp)
        {
            if (cp.Where(o => o.life > 0).Count() > 0)
                return;

            changePoint p = new changePoint(pieceX, pieceY, boardType);
            cp.Add(p);

            for (int X = -1; X <= 1; X++)
            {
                for (int Y = -1; Y <= 1; Y++)
                {
                    //只剩下上下左右四个位置
                    if (X != 0 && Y != 0 || X == 0 && Y == 0)
                        continue;
                    if (pieceX + X < 0 || pieceX + X >= this.boardX || pieceY + Y < 0 || pieceY + Y >= this.boardY)
                        continue;
                    if (cp.Where(o => o.pieceX == pieceX + X && o.pieceY == pieceY + Y).Count() > 0)
                    {
                        continue;
                    }

                    if (state[pieceX + X, pieceY + Y] == boardType.Blank)
                    {
                        p.setLife();
                    }
                    else if (state[pieceX + X, pieceY + Y] == boardType)
                    {
                        CheckPoint(pieceX + X, pieceY + Y, state[pieceX + X, pieceY + Y], cp);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }
    }
}
