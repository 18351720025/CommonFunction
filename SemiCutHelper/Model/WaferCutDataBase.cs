using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SemiCutHelper.Model
{
    /// <summary>
    /// 晶圆切割数据顶层容器。
    /// 一个晶圆包含多个切割通道(Channel)，每个通道有自己的切割线定义和执行指令。
    /// </summary>
    public class WaferCutData
    {
        /// <summary>
        /// 晶圆唯一标识（如 LotId 或 WaferId）
        /// </summary>
        public string WaferId { get; set; } = string.Empty;

        /// <summary>
        /// 晶圆上所有通道的切割数据
        /// </summary>
        public List<ChannelCutData> CutChannels { get; set; } = new List<ChannelCutData>();

        /// <summary>
        /// 当前活跃通道的标识名称
        /// </summary>
        public string ActiveChannelId { get; set; } = string.Empty;

        /// <summary>
        /// 晶圆整体加工状态
        /// </summary>
        public WaferProcessStatus WaferStatus { get; set; } = WaferProcessStatus.Idle;

        #region 便捷访问方法

        /// <summary>
        /// 获取当前活跃的通道数据
        /// </summary>
        public ChannelCutData? GetActiveChannel()
        {
            return CutChannels.FirstOrDefault(c => c.Name == ActiveChannelId);
        }

        /// <summary>
        /// 按名称查找通道
        /// </summary>
        public ChannelCutData? FindChannel(string channelName)
        {
            return CutChannels.FirstOrDefault(c => c.Name == channelName);
        }

        /// <summary>
        /// 获取所有未完成的通道
        /// </summary>
        public IEnumerable<ChannelCutData> GetPendingChannels()
        {
            return CutChannels.Where(c => c.Status != CutState.Completed && c.Status != CutState.Skipped);
        }

        /// <summary>
        /// 判断整个晶圆是否加工完成
        /// </summary>
        public bool IsAllChannelsCompleted()
        {
            return CutChannels.Count > 0 && CutChannels.All(c => c.Status == CutState.Completed);
        }

        #endregion
    }
}
