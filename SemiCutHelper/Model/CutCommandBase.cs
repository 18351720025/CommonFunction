using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace SemiCutHelper.Model
{
    /// <summary>
    /// 切割指令基类，定义了一组切割动作的执行逻辑与流控
    /// </summary>
    public class CutCommandBase
    {
        /// <summary>
        /// 执行后是否触发检查点
        /// (若为 true，则在当前指令序列完成后暂停，等待人工或视觉确认)
        /// </summary>
        public bool IsCheckpointEnabled { get; set; } = false;

        /// <summary>
        /// 是否需要执行首刀位置校验 (Alignment/Index Check)
        /// </summary>
        public bool IsFirstCutValidationRequired { get; set; } = false;

        /// <summary>
        /// 指令当前执行状态
        /// </summary>
        public CutCommandStatus ExecutionStatus { get; set; } = CutCommandStatus.Pending;

        /// <summary>
        /// 具体执行动作的数据项集合
        /// </summary>
        public List<CutCommandItem> Actions { get; set; } = new List<CutCommandItem>();
    }
}
