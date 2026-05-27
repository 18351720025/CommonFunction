using SemiCutHelper.Model;
namespace SemiCutHelper.Utils
{
    /// <summary>
    /// 切割数据工具类：提供切割线检索、状态查询、参数计算等静态方法。
    /// </summary>
    public static class CutDataUtils
    {
        // ============================================================
        //  状态查询
        // ============================================================

        /// <summary>
        /// 判断是否可以恢复切割
        /// 条件：晶圆状态为 Paused 且存在未完成的通道
        /// </summary>
        /// <param name="waferData">当前晶圆加工数据</param>
        /// <param name="cutSeq">切割序列</param>
        /// <returns>true: 可以恢复切割; false: 不可恢复</returns>
        public static bool CanResumeCut(WaferCutData waferData, string cutSeq)
        {
            if (waferData.WaferStatus == WaferProcessStatus.Idle || string.IsNullOrEmpty(waferData.ActiveChannelId) || 
                waferData.FindChannel(waferData.ActiveChannelId) is not ChannelCutData chCutData)
            {
                return false;
            }
            if (chCutData.DefinedLines.Count <= 0 ||
                chCutData.CommandSequence.Count <= 0 ||
                !cutSeq.Contains(waferData.ActiveChannelId.Last()))
            {
                return false;
            }
            return true;
        }
    }
}
