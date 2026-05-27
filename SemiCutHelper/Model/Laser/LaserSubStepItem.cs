using System;
using System.Collections.Generic;
using System.Text;

namespace SemiCutHelper.Model.Laser
{
    /// <summary>
    /// 激光切割子步骤：在通用 SubStepItem 基础上增加激光工艺参数。
    /// </summary>
    public class LaserSubStepItem : SubStepItem
    {
        /// <summary>
        /// 是否需要在本行程执行功率检测 (仅限激光工艺)
        /// </summary>
        public bool IsPowerCalibrationRequired { get; set; } = false;

        #region 激光工艺参数 (Laser Process Parameters)

        /// <summary>
        /// 激光输出功率 (单位: W)
        /// </summary>
        public double LaserPower { get; set; } = 0;

        /// <summary>
        /// 脉冲频率 (Pulse Frequency / Repetition Rate, 单位: kHz)
        /// </summary>
        public double PulseFrequency { get; set; } = 0;

        /// <summary>
        /// 多光束分光间距 (Beam Splitting Distance, 单位: μm)
        /// </summary>
        public double BeamPitch { get; set; } = 0;

        /// <summary>
        /// 激光扫描速度 (单位: mm/s)
        /// </summary>
        public double ScanSpeed { get; set; } = 0;

        /// <summary>
        /// 激光重复扫描次数 (Pass Count)
        /// </summary>
        public int PassCount { get; set; } = 1;

        #endregion
    }
}
