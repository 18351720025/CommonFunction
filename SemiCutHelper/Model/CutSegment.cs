using System;
using System.Collections.Generic;
using System.Text;

namespace SemiCutHelper.Model
{
    public struct Point
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
    /// <summary>
    /// 切割线段：定义单次加工行程中的具体起止区间
    /// </summary>
    public class CutSegment
    {
        /// <summary>
        /// 段起始坐标 (加工开启点)
        /// </summary>
        public Point StartPoint { get; set; }

        /// <summary>
        /// 段结束坐标 (加工关闭点)
        /// </summary>
        public Point EndPoint { get; set; }

        /* 如果是 X 轴方向的切割，建议增加以下只读属性便于逻辑调用 */

        /// <summary>
        /// 段长度
        /// </summary>
        public double Length => Math.Sqrt(Math.Pow(EndPoint.X - StartPoint.X, 2) + Math.Pow(EndPoint.Y - StartPoint.Y, 2));
    }
}
