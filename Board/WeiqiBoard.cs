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

        /// <summary>
        /// 下棋
        /// </summary>
        /// <param name="point"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public override bool SetState(int pieceX, int pieceY, boardType boardType)
        {
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

            DoJudgmentLogic(pieceX, pieceY, boardType);

            return true;
        }

        /// <summary>
        /// 绘制棋盘
        /// </summary>
        /// <param name="form"></param>
        public override void DrawBoard(Form form)
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
        private void DoJudgmentLogic(int pieceX, int pieceY, boardType boardType)
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
