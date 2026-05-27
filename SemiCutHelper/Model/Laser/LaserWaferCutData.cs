using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SemiCutHelper.Model.Laser
{
    /// <summary>
    /// 激光切割工艺的晶圆数据容器。
    /// 在 WaferCutData 基础上增加了激光工艺特有的全局参数。
    /// </summary>
    public class LaserWaferCutData : WaferCutData
    {
        /// <summary>
        /// 激光配方名称
        /// </summary>
        public string LaserRecipeName { get; set; } = string.Empty;

        /// <summary>
        /// 全局激光功率基准值 (单位: W)
        /// </summary>
        public double GlobalLaserPower { get; set; } = 0;

        /// <summary>
        /// 全局脉冲频率基准值 (单位: kHz)
        /// </summary>
        public double GlobalPulseFrequency { get; set; } = 0;

        /// <summary>
        /// 获取通道数据（强类型转换辅助）
        /// </summary>
        public new List<LaserChannelCutData> CutChannels
        {
            get => base.CutChannels.OfType<LaserChannelCutData>().ToList();
            set => base.CutChannels = value.Cast<ChannelCutData>().ToList();
        }

        /// <summary>
        /// 获取激光工艺的活跃通道
        /// </summary>
        public LaserChannelCutData? GetActiveLaserChannel()
        {
            return GetActiveChannel() as LaserChannelCutData;
        }
    }
}
