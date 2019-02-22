using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Board
{
    public class WeiqiBoard : BaseBoard
    {
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

        /// <summary>
        /// 判断逻辑
        /// </summary>
        /// <param name="pieceX"></param>
        /// <param name="pieceY"></param>
        /// <param name="boardType"></param>
        protected override void DoJudgmentLogic(int pieceX, int pieceY, boardType boardType)
        {
            List<changePoint> changePoints = new List<changePoint>();
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
        /// <param name="cp"></param>
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
