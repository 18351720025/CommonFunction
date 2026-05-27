using SemiCutHelper.Model;
using SemiCutHelper.Model.Enums;
using System.Buffers;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices; // 引入内联命名空间

namespace SemiCutHelper.Utils
{
    public static class CutGeometryUtils
    {
        private const double DefaultAcc = 10.0; // 默认加速度值（单位：mm/s^2）
        private const double Epsilon = 1e-9; // 数值比较的容差
        private const int DecimalPlaces = 5; // 坐标计算结果的小数位数

        // ============================================================
        //  椭圆/圆交点计算
        // ============================================================

        public static bool CalculateEllipseFoci(in Point center, double xLRadius, double xRRadius,
                                  double yRadius, double y, out double xStart, out double xEnd)
        {
            xStart = 0; xEnd = 0;
            double dy = y - center.Y;
            // 优化：除法提前，尽量用乘法
            double yComponent = (dy * dy) / (yRadius * yRadius);

            if (yComponent > 1.0)
            {
                LogNotify.NotifyLogChanged("交点计算", $"水平线Y={y}在椭圆外，无交点。");
                return false;
            }

            double invYComp = 1.0 - yComponent;

            // 左侧椭圆交点X坐标
            double resLeft = Math.Sqrt(invYComp * xLRadius * xLRadius);
            xStart = Math.Round(center.X - resLeft, DecimalPlaces, MidpointRounding.AwayFromZero);

            // 右侧椭圆交点X坐标
            double resRight = Math.Sqrt(invYComp * xRRadius * xRRadius);
            xEnd = Math.Round(center.X + resRight, DecimalPlaces, MidpointRounding.AwayFromZero);

            return true;
        }

        // ============================================================
        //  运动学计算
        // ============================================================

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double CalcAccelDecelDistance(double speed, double acc)
        {
            if (acc <= 0) acc = DefaultAcc;
            // 极致优化：0.5 * acc * (speed/acc)^2 等价于 0.5 * speed * speed / acc
            // 减少了一次除法和一次乘法
            return (0.5 * speed * speed) / acc + 5.0;
        }

        // ============================================================
        //  向量/几何计算
        // ============================================================

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double CrossProduct(in Point a, in Point b, in Point c)
        {
            // in 关键字避免结构体拷贝
            return (b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsSegmentsIntersect(in Point p1, in Point p2, in Point p3, in Point p4)
        {
            // 跨立实验 (去除 Math.Max/Min 的额外方法调用开销，直接使用内在逻辑并结合短路)
            double cp1 = CrossProduct(in p1, in p2, in p3);
            double cp2 = CrossProduct(in p1, in p2, in p4);

            if (cp1 * cp2 >= -Epsilon) return false; // 提前短路

            double cp3 = CrossProduct(in p3, in p4, in p1);
            double cp4 = CrossProduct(in p3, in p4, in p2);

            return cp3 * cp4 < -Epsilon;
        }

        // ============================================================
        //  多边形校验
        // ============================================================

        public static bool ValidatePolygon(in List<Point> points)
        {
            int count = points.Count;
            if (count < 3)
            {
                LogNotify.NotifyLogChanged("设定多边形", "边缘点少于3个，请检查！");
                return false;
            }

            double totalArea = 0.0;
            // 使用 Span 消除列表索引器的边界检查
            ReadOnlySpan<Point> span = CollectionsMarshal.AsSpan(points);

            for (int i = 0; i < count; ++i)
            {
                ref readonly var p1 = ref span[i];
                // 极致优化：用三元运算符取代模运算 (i + 1) % count
                ref readonly var p2 = ref span[i == count - 1 ? 0 : i + 1];

                if (!double.IsFinite(p1.X) || !double.IsFinite(p1.Y) || !double.IsFinite(p2.X) || !double.IsFinite(p2.Y))
                {
                    LogNotify.NotifyLogChanged("设定多边形", $"边缘点值非法({p1.X:F5}, {p1.Y:F5})->({p2.X:F5} ,{p2.Y:F5})");
                    return false;
                }

                double dx = p1.X - p2.X;
                double dy = p1.Y - p2.Y;
                if (Math.Abs(dx) < Epsilon && Math.Abs(dy) < Epsilon)
                {
                    LogNotify.NotifyLogChanged("设定多边形", $"相邻点存在重合({p1.X:F5}, {p1.Y:F5})->({p2.X:F5} ,{p2.Y:F5})");
                    return false;
                }

                totalArea += p1.X * p2.Y - p2.X * p1.Y;
            }

            if (Math.Abs(totalArea) < Epsilon) return false;

            // 自相交检查
            for (int i = 0; i < count; ++i)
            {
                ref readonly var p1 = ref span[i];
                ref readonly var p2 = ref span[i == count - 1 ? 0 : i + 1];

                for (int j = i + 2; j < count; ++j)
                {
                    if (i == 0 && j == count - 1) continue;

                    ref readonly var p3 = ref span[j];
                    ref readonly var p4 = ref span[j == count - 1 ? 0 : j + 1];

                    if (IsSegmentsIntersect(in p1, in p2, in p3, in p4))
                    {
                        LogNotify.NotifyLogChanged("设定多边形", "点位存在自相交，请检查！");
                        return false;
                    }
                }
            }
            return true;
        }

        // ============================================================
        //  碎片切割边缘交点 (保留原有的极佳优化，补充 Span 排序)
        // ============================================================

        public static bool GetFragmentIntersectionsAtY(in ReadOnlySpan<Point> pointsSpan, double y, out List<double> outXPoints)
        {
            int count = pointsSpan.Length;
            if (count < 3)
            {
                outXPoints = [];
                return false;
            }
            outXPoints = new List<double>(8);
            bool usePool = count > 128;
            (double X, bool IsHorizontal)[]? poolArray = null;
            Span<(double X, bool IsHorizontal)> tempSpan = usePool
                ? (poolArray = ArrayPool<(double X, bool IsHorizontal)>.Shared.Rent(count * 2))
                : stackalloc (double X, bool IsHorizontal)[count * 2];

            int tempCount = 0;
            int i1 = 0, i2 = 1;
            int i3 = count == 3 ? 0 : 2;

            for (int i = 0; i < count; ++i)
            {
                ref readonly var p1 = ref pointsSpan[i1];
                ref readonly var p2 = ref pointsSpan[i2];
                double dy12 = p2.Y - p1.Y;

                if (Math.Abs(dy12) < Epsilon)
                {
                    if (Math.Abs(y - p1.Y) < Epsilon || Math.Abs(y - p2.Y) < Epsilon)
                    {
                        tempSpan[tempCount++] = (p1.X, true);
                        tempSpan[tempCount++] = (p2.X, true);
                    }
                }
                else
                {
                    ref readonly var p3 = ref pointsSpan[i3];
                    double lastSegmentDy = p3.Y - p2.Y;
                    if (Math.Abs(lastSegmentDy) < Epsilon) lastSegmentDy = 0;

                    double dy1 = p1.Y - p2.Y;

                    bool condition1 = dy1 * lastSegmentDy > 0 &&
                                      ((p1.Y < y - Epsilon && p2.Y > y + Epsilon) || (p2.Y < y - Epsilon && p1.Y > y + Epsilon));
                    bool condition2 = dy1 * lastSegmentDy <= 0 &&
                                      ((p1.Y < y - Epsilon && p2.Y >= y) || (p2.Y <= y && p1.Y > y + Epsilon));

                    if (condition1 || condition2)
                    {
                        double x = p1.X + (y - p1.Y) * (p2.X - p1.X) / dy12;
                        tempSpan[tempCount++] = (x, false);
                    }
                }

                i1 = i2;
                i2 = i3;
                if (++i3 >= count) i3 = 0;
            }

            if (tempCount < 2)
            {
                if (usePool) ArrayPool<(double, bool)>.Shared.Return(poolArray!);
                return false;
            }

            int mainLimit = tempCount - 2;
            for (int i = 0; i < mainLimit; ++i)
            {
                double x = tempSpan[i].X;
                double x2 = tempSpan[i + 1].X;
                if (Math.Abs(x - x2) < Epsilon || (tempSpan[i + 1].IsHorizontal && outXPoints.Count < 1))
                    continue;

                outXPoints.Add(x);
            }

            if (tempCount >= 2)
            {
                int idx1 = tempCount - 2;
                if (idx1 > 0 && !(Math.Abs(tempSpan[idx1].X - tempSpan[idx1 - 1].X) < Epsilon))
                    outXPoints.Add(tempSpan[idx1].X);
                else if (idx1 <= 0)
                    outXPoints.Add(tempSpan[idx1].X);

                int idx2 = tempCount - 1;
                if (idx2 > 0 && !(Math.Abs(tempSpan[idx2].X - tempSpan[idx2 - 1].X) < Epsilon || tempSpan[idx2 - 1].IsHorizontal))
                    outXPoints.Add(tempSpan[idx2].X);
            }

            if (usePool)
                ArrayPool<(double, bool)>.Shared.Return(poolArray!);

            if (outXPoints.Count > 0)
            {
                // 极致优化：使用 Span 对 List 的底层数组直接排序，比传统的 outXPoints.Sort() 更快
                CollectionsMarshal.AsSpan(outXPoints).Sort();
            }

            return outXPoints.Count > 0 && outXPoints.Count % 2 == 0;
        }

        // ============================================================
        //  切割检测频率判断
        // ============================================================

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNeedKerfOrTarget(int index, int waferIndex, int everyPiece,
                                int startIndex, int frequency)
        {
            if (everyPiece <= 0 || waferIndex % everyPiece != 0) return false;
            if (startIndex <= 0 && frequency <= 0) return false;

            if (index <= startIndex && startIndex > 0)
                return startIndex == index;

            if (index > startIndex && frequency > 0)
                return (index - startIndex) % frequency == 0;

            return false;
        }

        // ============================================================
        //  切割线起止坐标计算
        // ============================================================

        public static bool CalcCutLineStartEndX(WaferType waferType, bool hasFlat, double flatLength, Direction flatDirection, in Point center, double radius,
                                  double xLen, double yLen, double xStartExt, double xEndExt, double yExt, double accExt, double angle, double y, out CutLineBase cutLine)
        {
            cutLine = new()
            {
                ThetaAngle = angle,
                AlignTargetY = y
            };

            if (waferType == WaferType.Round)
            {
                // 优化：使用 * 0.5 代替 / 2.0
                if (!CalculateEllipseFoci(in center, xLen * 0.5, xLen * 0.5,
                                           yLen * 0.5 + yExt, y,
                                           out double entryX, out double exitX))
                {
                    return false;
                }

                cutLine.CutEntryX = entryX;
                cutLine.CutExitX = exitX;

                if (hasFlat)
                {
                    // 极致优化：flatLength / 2.0 * (flatLength / 2.0) -> flatLength * flatLength * 0.25
                    double flatX = Math.Sqrt(radius * radius - flatLength * flatLength * 0.25);
                    double absAngle = Math.Abs(angle);
                    double absAngle90 = Math.Abs(angle - 90.0);

                    if ((absAngle <= 45 && flatDirection == Direction.POSITIVE_X) ||
                        (absAngle90 <= 45 && flatDirection == Direction.POSITIVE_Y))
                    {
                        double endX = center.X + flatX;
                        if (cutLine.CutExitX > endX) cutLine.CutExitX = endX;
                    }
                    else if ((absAngle <= 45 && flatDirection == Direction.NEGATIVE_X) ||
                             (absAngle90 <= 45 && flatDirection == Direction.NEGATIVE_Y))
                    {
                        double startX = center.X - flatX;
                        if (cutLine.CutEntryX < startX) cutLine.CutEntryX = startX;
                    }
                }
            }
            else if (waferType == WaferType.Square)
            {
                double halfXLen = xLen * 0.5;
                cutLine.CutEntryX = center.X - halfXLen;
                cutLine.CutExitX = center.X + halfXLen;
            }
            else
            {
                return false;
            }

            cutLine.CutEntryX -= xStartExt;
            cutLine.CutExitX += xEndExt;

            if (cutLine.CutEntryX >= cutLine.CutExitX)
            {
                cutLine.CutEntryX = cutLine.CutExitX;
            }

            cutLine.RapidApproachX = cutLine.CutEntryX - accExt;
            cutLine.RapidRetractX = cutLine.CutExitX + accExt;
            return true;
        }
    }
}