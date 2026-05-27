using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SemiCutHelper.Model
{
    /// <summary>
    /// 切割指令基类，定义了一组切割动作的执行逻辑与流控。
    /// 一个 CutCommand 对应一个通道的一次完整切割流程编排。
    /// </summary>
    public class CutCommandBase
    {
        /// <summary>
        /// 指令在当前通道内的唯一编号 (0-based)
        /// </summary>
        public int Id { get; set; } = 0;

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

        #region 便捷方法

        /// <summary>
        /// 获取所有待执行的动作项
        /// </summary>
        public IEnumerable<CutCommandItem> GetPendingActions()
        {
            return Actions.Where(a => a.Status == CutCommandStatus.Pending);
        }

        /// <summary>
        /// 判断该指令是否全部完成
        /// </summary>
        public bool IsAllActionsCompleted()
        {
            return Actions.Count > 0 && Actions.All(a => a.Status == CutCommandStatus.Completed);
        }

        #endregion
    }
}
