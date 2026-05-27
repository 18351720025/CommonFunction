using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SemiCutHelper.Model.Enums;

namespace SemiCutHelper.Model
{
    /// <summary>
    /// 单个切割通道的数据定义。
    /// 一个通道包含一组切割线定义和对应的执行指令序列。
    /// </summary>
    public class ChannelCutData
    {
        /// <summary>
        /// 通道唯一标识名称 (例如: "CH1", "CH2")
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 切割策略模式 (定义了步进方向、往复切割逻辑及待机位)
        /// </summary>
        public CutMode Strategy { get; set; } = CutMode.A;

        /// <summary>
        /// 切割方向 (X方向切割 或 Y方向切割)
        /// </summary>
        public Direction CutDirection { get; set; } = Direction.POSITIVE_X;

        #region 坐标与参考位 (Coordinates & Reference)

        /// <summary>
        /// 首条切割道的起始参考坐标 X
        /// </summary>
        public double FirstLineOriginX { get; set; } = 0;

        /// <summary>
        /// 首条切割道的起始参考坐标 Y
        /// </summary>
        public double FirstLineOriginY { get; set; } = 0;

        /// <summary>
        /// 切割道间距 (相邻两条切割道中心之间的距离，即 Index Step)
        /// </summary>
        public double StreetPitch { get; set; } = 0;

        /// <summary>
        /// 切割道中心相对于特征点（Mark）的横向偏移量
        /// </summary>
        public double StreetToMarkOffsetX { get; set; } = 0;

        /// <summary>
        /// 切割道中心相对于特征点（Mark）的纵向偏移量
        /// </summary>
        public double StreetToMarkOffsetY { get; set; } = 0;

        #endregion

        #region 补偿与修正 (Compensation)

        /// <summary>
        /// 目标位置累计补偿值 (通常用于对齐修正)
        /// </summary>
        public double CumulativeTargetOffset { get; set; } = 0;

        /// <summary>
        /// 刀缝/对刀累计补偿值 (用于修正切割宽度 Kerf 引起的偏差)
        /// </summary>
        public double CumulativeKerfOffset { get; set; } = 0;

        #endregion

        #region 逻辑执行 (Execution)

        /// <summary>
        /// 预定义的切割线集合
        /// </summary>
        public List<CutLineBase> DefinedLines { get; set; } = new List<CutLineBase>();

        /// <summary>
        /// 生成的切割动作指令序列
        /// </summary>
        public List<CutCommandBase> CommandSequence { get; set; } = new List<CutCommandBase>();

        /// <summary>
        /// 当前通道的切割执行状态
        /// </summary>
        public CutState Status { get; set; } = CutState.Pending;

        #endregion

        #region 便捷访问方法

        /// <summary>
        /// 按 Id 查找切割线
        /// </summary>
        public CutLineBase? GetLineById(int lineId)
        {
            return DefinedLines.FirstOrDefault(l => l.Id == lineId);
        }

        /// <summary>
        /// 按 AlignTargetY 查找最近的切割线
        /// </summary>
        public CutLineBase? FindNearestLineByY(double y)
        {
            if (DefinedLines.Count == 0) return null;
            return DefinedLines.OrderBy(l => Math.Abs(l.AlignTargetY - y)).First();
        }

        /// <summary>
        /// 获取所有待加工的切割线
        /// </summary>
        public IEnumerable<CutLineBase> GetPendingLines()
        {
            return DefinedLines.Where(l => l.WorkStatus == CutState.Pending);
        }

        /// <summary>
        /// 获取所有待执行的指令
        /// </summary>
        public IEnumerable<CutCommandBase> GetPendingCommands()
        {
            return CommandSequence.Where(c => c.ExecutionStatus == CutCommandStatus.Pending);
        }

        /// <summary>
        /// 判断当前通道是否加工完成
        /// </summary>
        public bool IsCompleted()
        {
            return Status == CutState.Completed;
        }

        #endregion
    }
}
