using System;
using System.Collections.Generic;
using System.Text;

namespace SemiCutHelper.Model
{

    /// <summary>
    /// 晶圆加工/生命周期状态
    /// </summary>
    public enum WaferProcessStatus : int
    {
        /// <summary>
        /// 待加工 (已加载但尚未开始)
        /// </summary>
        Idle = 0,

        /// <summary>
        /// 加工中 (正在执行切割程序)
        /// </summary>
        Processing = 1,

        /// <summary>
        /// 已暂停 (由人工干预或异常触发)
        /// </summary>
        Paused = 2,

        /// <summary>
        /// 加工完成 (正常结束)
        /// </summary>
        Completed = 3,

        /// <summary>
        /// 已终止 (由于错误或手动强行停止)
        /// </summary>
        Aborted = 4
    }

    public enum CutState : int
    {
        /// <summary>
        /// 待加工 (已分配但尚未开始)
        /// </summary>
        Pending = 0,

        /// <summary>
        /// 正在切割
        /// </summary>
        InProcess = 1,

        /// <summary>
        /// 切割完成
        /// </summary>
        Completed = 2,

        /// <summary>
        /// 跳过 (由于划片道坏点或用户选择不切割)
        /// </summary>
        Skipped = 3
    }

    public enum CutCommandStatus:int
    {
        /// <summary>
        /// 待执行
        /// </summary>
        Pending = 0,
        /// <summary>
        /// 执行中
        /// </summary>
        Executing,
        /// <summary>
        /// 已完成
        /// </summary>
        Completed,
    }
    public enum CutMode : int
    {
        /// <summary>
        /// A模式：从X轴正向开始，Z字形衔接每一道切割线
        /// </summary>
        A = 0,

        /// <summary>
        /// A_UP模式：从X轴负向开始，Z字形衔接每一道切割线
        /// </summary>
        A_UP = 1,

        /// <summary>
        /// B模式：第一道从X轴正向开始，第二道从X轴负向开始，依此类推，S字形衔接每一道切割线
        /// </summary>
        B = 2,

        /// <summary>
        /// B_UP模式：第一道从X轴负向开始，第二道从X轴正向开始，依此类推，S字形衔接每一道切割线
        /// </summary>
        B_UP = 3,
    }
}
