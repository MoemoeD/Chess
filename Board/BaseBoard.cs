using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Board
{
    public abstract class BaseBoard
    {
        #region -基础-

        /// <summary>
        /// 棋盘X轴
        /// </summary>
        protected int boardX { get; set; }

        /// <summary>
        /// 棋盘Y轴
        /// </summary>
        protected int boardY { get; set; }

        /// <summary>
        /// 间隙
        /// </summary>
        protected int gapPixel { get; set; }

        /// <summary>
        /// 棋盘基准坐标X
        /// </summary>
        protected int initialPixelx { get; set; }

        /// <summary>
        /// 棋盘基准坐标Y
        /// </summary>
        protected int initialPixely { get; set; }

        /// <summary>
        /// 棋盘颜色
        /// </summary>
        protected Color mainColor { get; set; }

        /// <summary>
        /// 棋盘背景颜色
        /// </summary>
        protected Color bgColor { get; set; }

        /// <summary>
        /// 棋盘点位状态
        /// </summary>
        protected boardType[,] state { get; set; }

        /// <summary>
        /// 棋盘点位状态类型
        /// </summary>
        public enum boardType
        {
            /// <summary>
            /// 空
            /// </summary>
            Blank,

            /// <summary>
            /// 黑
            /// </summary>
            Black,

            /// <summary>
            /// 白
            /// </summary>
            White,
        }

        /// <summary>
        /// 获得棋盘点的具体坐标
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        public Point GetRealPointByBoardPoint(int X, int Y)
        {
            int pixelx = this.initialPixelx + X * this.gapPixel;
            int pixely = this.initialPixely + Y * this.gapPixel;
            return new Point(pixelx, pixely);
        }

        /// <summary>
        /// 获取坐标所在范围的棋盘点
        /// </summary>
        /// <param name="pixelx"></param>
        /// <param name="pixely"></param>
        /// <param name="Point"></param>
        /// <returns></returns>
        public bool GetBoardRangeByRealPoint(int pixelx, int pixely, out Point Point)
        {
            //xy减去基准坐标加上半个间隙除以间隙得到XY
            int X = (pixelx - this.initialPixelx + this.gapPixel / 2) / this.gapPixel;
            int Y = (pixely - this.initialPixely + this.gapPixel / 2) / this.gapPixel;

            //判断-0
            int remainderx = (pixelx - this.initialPixelx + this.gapPixel / 2) % this.gapPixel;
            int remaindery = (pixely - this.initialPixely + this.gapPixel / 2) % this.gapPixel;

            Point = new Point(X, Y);

            //超出边界
            if (X >= this.boardX || Y >= this.boardY || remainderx < 0 || remaindery < 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获取棋盘颜色
        /// </summary>
        /// <returns></returns>
        public Color GetMainColor()
        {
            return this.mainColor;
        }

        /// <summary>
        /// 获取棋盘背景颜色
        /// </summary>
        /// <returns></returns>
        public Color GetBgColor()
        {
            return this.bgColor;
        }

        public class line
        {
            public Point p1 { get; set; }

            public Point p2 { get; set; }

            internal line(Point p1, Point p2)
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

        #endregion

        #region -日志-

        /// <summary>
        /// 棋谱
        /// </summary>
        protected List<boardType[,]> records { get; set; }

        /// <summary>
        /// 棋谱
        /// </summary>
        protected List<log> logs { get; set; }

        /// <summary>
        /// 步数
        /// </summary>
        protected int count { get; set; }

        /// <summary>
        /// 记录
        /// </summary>
        public class log
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
            /// 之前点位状态
            /// </summary>
            internal boardType lastState { get; set; }

            /// <summary>
            /// 行动状态
            /// </summary>
            public actionType action { get; set; }

            /// <summary>
            /// 步数
            /// </summary>
            internal int count { get; set; }

            internal log(int pieceX, int pieceY, boardType state, boardType lastState, actionType action, int count)
            {
                this.pieceX = pieceX;
                this.pieceY = pieceY;
                this.state = state;
                this.lastState = lastState;
                this.action = action;
                this.count = count;
            }
        }

        /// <summary>
        /// 行动状态类型
        /// </summary>
        public enum actionType
        {
            /// <summary>
            /// 添加
            /// </summary>
            Add,

            /// <summary>
            /// 移除
            /// </summary>
            Remove,

            /// <summary>
            /// 获胜
            /// </summary>
            Victory,
        }

        /// <summary>
        /// 获取当前步数记录
        /// </summary>
        /// <returns></returns>
        public List<log> GetCurrentLog()
        {
            return logs.Where(o => o.count == this.count).ToList();
        }

        #endregion

        #region -处理-

        /// <summary>
        /// 下棋
        /// </summary>
        /// <param name="pieceX"></param>
        /// <param name="pieceY"></param>
        /// <param name="boardType"></param>
        /// <returns></returns>
        public bool SetState(int pieceX, int pieceY, boardType boardType)
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
        /// <param name="pieceX"></param>
        /// <param name="pieceY"></param>
        /// <param name="boardType"></param>
        protected abstract void DoJudgmentLogic(int pieceX, int pieceY, boardType boardType);

        #endregion
    }
}
