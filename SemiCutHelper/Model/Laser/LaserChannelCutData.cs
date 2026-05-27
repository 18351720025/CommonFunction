using System;
using System.Collections.Generic;
using System.Text;

namespace SemiCutHelper.Model.Laser
{
    /// <summary>
    /// 激光切割工艺的通道数据。
    /// 在 ChannelCutData 基础上增加了激光特有的通道级参数。
    /// </summary>
    public class LaserChannelCutData : ChannelCutData
    {
        /// <summary>
        /// 通道级激光功率偏移量 (叠加到 GlobalLaserPower)
        /// </summary>
        public double ChannelLaserPowerOffset { get; set; } = 0;

        /// <summary>
        /// 通道级脉冲频率偏移量
        /// </summary>
        public double ChannelPulseFrequencyOffset { get; set; } = 0;

        /// <summary>
        /// 该通道是否需要执行激光功率校准
        /// </summary>
        public bool IsPowerCalibrationRequired { get; set; } = false;

        /// <summary>
        /// 激光光斑尺寸 (单位: μm)
        /// </summary>
        public double SpotSize { get; set; } = 0;

        /// <summary>
        /// 有效激光功率 (基准值 + 通道偏移)
        /// </summary>
        public double EffectiveLaserPower
        {
            get
            {
                if (CutChannelsParent is LaserWaferCutData laserWafer)
                    return laserWafer.GlobalLaserPower + ChannelLaserPowerOffset;
                return ChannelLaserPowerOffset;
            }
        }

        /// <summary>
        /// 反向引用父级晶圆数据（由外部设置）
        /// </summary>
        internal WaferCutData? CutChannelsParent { get; set; }
    }
}
