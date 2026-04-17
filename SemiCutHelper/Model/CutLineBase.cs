using System;
using System.Collections.Generic;
using System.Text;

namespace SemiCutHelper.Model
{
    /// <summary>
    /// 表示单条切割道的几何定义与执行状态
    /// </summary>
    public class CutLineBase
    {
        /// <summary>
        /// 步进序号（关联 Sub-Index 参数配置）
        /// </summary>
        public int SubIndexId { get; set; } = 0;

        #region 轨迹坐标 (Trajectory / Stroke)

        /// <summary>
        /// 进刀前位移起点 (安全位置)
        /// </summary>
        public double RapidApproachX { get; set; } = 0;

        /// <summary>
        /// 切割起始位置 (入刀点)
        /// </summary>
        public double CutEntryX { get; set; } = 0;

        /// <summary>
        /// 切割结束位置 (出刀点)
        /// </summary>
        public double CutExitX { get; set; } = 0;

        /// <summary>
        /// 退刀位移终点 (安全位置)
        /// </summary>
        public double RapidRetractX { get; set; } = 0;

        #endregion

        #region 姿态控制 (Alignment & Rotation)

        /// <summary>
        /// 步进轴目标位置（相机示教/对齐后的 Y 轴坐标）
        /// </summary>
        public double AlignTargetY { get; set; } = 0;

        /// <summary>
        /// 旋转工作台角度 (Theta 轴)
        /// </summary>
        public double ThetaAngle { get; set; } = 0;

        #endregion

        #region 逻辑控制 (Process Control)

        /// <summary>
        /// 是否需要执行切割前/后的视觉检查
        /// </summary>
        public bool IsInspectionRequired { get; set; } = false;

        /// <summary>
        /// 当前切割道的作业状态 (未切、执行中、已完成、跳过等)
        /// </summary>
        public CutState WorkStatus { get; set; } = CutState.Pending;

        #endregion
    }
}
