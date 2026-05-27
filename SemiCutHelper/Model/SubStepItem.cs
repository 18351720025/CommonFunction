using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SemiCutHelper.Model
{
    /// <summary>
    /// 切割子指令项：定义单次行程（Pass）的具体运动参数与工艺参数。
    /// 一个 SubStep 是切割的最小执行单元，描述一次完整的"快速趋近 → 切割 → 退刀"动作。
    /// </summary>
    public class SubStepItem
    {
        /// <summary>
        /// 子步骤在当前 CommandItem 内的唯一编号 (0-based)
        /// </summary>
        public int Id { get; set; } = 0;

        #region 运动轨迹与位置 (Motion Trajectory & Positioning)

        /// <summary>
        /// X轴进给起点 (加速/趋近起点)
        /// </summary>
        public double ApproachPosX { get; set; } = 0;

        /// <summary>
        /// 切割开启起点 (入刀点)
        /// </summary>
        public double CutEntryX { get; set; } = 0;

        /// <summary>
        /// 切割关闭终点 (出刀点)
        /// </summary>
        public double CutExitX { get; set; } = 0;

        /// <summary>
        /// X轴运动终点 (减速/停止点)
        /// </summary>
        public double RetractPosX { get; set; } = 0;

        /// <summary>
        /// 步进轴 (Index Axis) 最终目标物理位置
        /// </summary>
        public double TargetIndexY { get; set; } = 0;

        /// <summary>
        /// 配方预设的步进偏移量 (针对特定 Pass 的 Offset)
        /// </summary>
        public double RecipeIndexOffset { get; set; } = 0;

        /// <summary>
        /// 相机与加工中心在 Y 轴的系统修正偏置
        /// </summary>
        public double ToolToCamIndexOffset { get; set; } = 0;

        /// <summary>
        /// 加工高度 (Z轴实时工作高度)
        /// </summary>
        public double WorkHeightZ { get; set; } = 0;

        /// <summary>
        /// 设定高度参考点 (Focus/Clearance Setpoint)
        /// </summary>
        public double SetPointZ { get; set; } = 0;

        /// <summary>
        /// 切割进给速度 (Feed Speed)
        /// </summary>
        public double FeedRate { get; set; } = 0;

        #endregion

        #region 复杂逻辑扩展 (Advanced Logic)

        /// <summary>
        /// 细分切割段集合 (用于非连续加工场景)
        /// </summary>
        public List<CutSegment> Segments { get; set; } = new List<CutSegment>();

        /// <summary>
        /// 步骤执行状态
        /// </summary>
        public CutCommandStatus Status { get; set; } = CutCommandStatus.Pending;

        #endregion

        #region 计算属性

        /// <summary>
        /// 实际切割行程长度 (入刀 → 出刀)
        /// </summary>
        public double CutStrokeLength => Math.Abs(CutExitX - CutEntryX);

        #endregion
    }
}
