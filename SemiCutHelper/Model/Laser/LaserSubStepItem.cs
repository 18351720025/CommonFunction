using System;
using System.Collections.Generic;
using System.Text;

namespace SemiCutHelper.Model.Laser
{
    internal class LaserSubStepItem: SubStepItem
    {
        /// <summary>
        /// 是否需要在本行程执行功率检测 (仅限激光工艺)
        /// </summary>
        public bool IsPowerCalibrationRequired { get; set; } = false;

        #region 激光工艺参数 (Laser Process Parameters)

        /// <summary>
        /// 激光输出功率 (单位通常为 W 或 %)
        /// </summary>
        public double LaserPower { get; set; } = 0;

        /// <summary>
        /// 脉冲频率 (Pulse Frequency / Repetition Rate)
        /// </summary>
        public double PulseFrequency { get; set; } = 0;

        /// <summary>
        /// 多光束分光间距 (Beam Splitting Distance)
        /// </summary>
        public double BeamPitch { get; set; } = 0;

        #endregion
    }
}
