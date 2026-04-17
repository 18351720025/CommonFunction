using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace SemiCutHelper.Model
{
    /// <summary>
    /// Data Off Each Channel For Cut Line
    /// </summary>
    public class ChannelCutData<TCutLineBase, TCutCmdBase>
        where TCutLineBase : CutLineBase
        where TCutCmdBase : CutCommandBase
    {
        /// <summary>
        /// 通道唯一标识名称 (例如: "CH1", "CH2")
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 切割策略模式 (定义了步进方向、往复切割逻辑及待机位)
        /// </summary>
        public CutMode Strategy { get; set; } = CutMode.A;

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
        public List<TCutLineBase> DefinedLines { get; set; } = new List<TCutLineBase>();

        /// <summary>
        /// 生成的切割动作指令序列
        /// </summary>
        public List<TCutCmdBase> CommandSequence { get; set; } = new List<TCutCmdBase>();

        /// <summary>
        /// 当前通道的切割执行状态
        /// </summary>
        public CutState Status { get; set; } = CutState.Pending;

        #endregion
    }
}
