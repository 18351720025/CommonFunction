using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace SemiCutHelper.Model
{
    /// <summary>
    /// 切割指令项：定义单条切割任务的具体执行参数与校验逻辑
    /// </summary>
    public class CutCommandItem
    {
        /// <summary>
        /// 关联的物理切割线定义 ID
        /// </summary>
        public int TargetLineId { get; set; } = -1;

        /// <summary>
        /// 是否执行物理切割 (true: 正常切割; false: 模拟运行/干运行)
        /// </summary>
        public bool IsDryRun { get; set; } = false;

        #region 校验与补偿标志 (Validation & Compensation)

        /// <summary>
        /// 是否需要执行切缝宽度 (Kerf) 检查
        /// </summary>
        public bool IsKerfInspectionRequired { get; set; } = false;

        /// <summary>
        /// 是否需要执行对齐中心 (Target) 检查
        /// </summary>
        public bool IsTargetInspectionRequired { get; set; } = false;

        /// <summary>
        /// 是否存在待应用的偏差补偿值
        /// (当自动/手动检查发现偏差后置位，补偿动作完成后复位)
        /// </summary>
        public bool HasPendingCompensation { get; set; } = false;

        #endregion

        #region 位置与偏移 (Positioning & Offsets)

        /// <summary>
        /// 旋转台目标角度 (Theta Axis)
        /// </summary>
        public double ThetaAngle { get; set; } = 0;

        /// <summary>
        /// 步进轴 (Index Axis) 的示教偏移量
        /// </summary>
        public double TeachIndexOffset { get; set; } = 0;

        /// <summary>
        /// 步进轴 (Index Axis) 的最终目标位置 (包含通道偏移)
        /// </summary>
        public double TargetIndexPos { get; set; } = 0;

        /// <summary>
        /// 相机中心与切割刀具中心之间的物理间距 (Camera-to-Blade Offset)
        /// </summary>
        public double CameraToBladeOffset { get; set; } = 0;

        #endregion

        #region 子任务序列 (Sub-Sequences)

        /// <summary>
        /// 单次 Pass 内的具体切割步骤数据
        /// </summary>
        public List<SubStepItem> SubSteps { get; set; } = new List<SubStepItem>();

        /// <summary>
        /// 当前指令项的执行状态
        /// </summary>
        public CutCommandStatus Status { get; set; } = CutCommandStatus.Pending;

        #endregion
    }
}
