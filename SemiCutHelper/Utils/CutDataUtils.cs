using SemiCutHelper.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace SemiCutHelper.Utils
{
    public static class CutDataUtils
    {

        // ============================================================
        //  切割线检索
        // ============================================================

        /// <summary>
        /// 按索引获取指定切割线
        /// </summary>
        /// <param name="chData">CH通道数据</param>
        /// <param name="index">切割线索引（从0开始）</param>
        /// <param name="outCutLine">[out] 切割线数据</param>
        /// <returns>true: 成功; false: 索引越界</returns>
        public static bool GetCutLineByIndex(const ChannelCutData<> chData, int index, CutLineBase&outCutLine)
        {

        }

        /// <summary>
        /// 按Y坐标查找最近的切割线及其索引
        /// </summary>
        /// <param name="chData">CH通道数据</param>
        /// <param name="y">目标Y坐标</param>
        /// <param name="outIndex">[out] 最近切割线的索引</param>
        /// <param name="outCutLine">[out] 最近的切割线数据</param>
        /// <returns>true: 成功; false: 无切割线数据</returns>
        bool GetCutLineByY(const CHCutLineBase& chData, double y, int& outIndex,
                           CutLineBase& outCutLine);

        // ============================================================
        //  状态查询
        // ============================================================

        /// <summary>
        /// 查询指定通道是否已完成切割
        /// </summary>
        /// <param name="chDataList">CH数据列表</param>
        /// <param name="chName">通道名称</param>
        /// <returns>true: 已完成切割（或通道不存在）; false: 未完成</returns>
        bool IsChCutFinished(const std::vector<CHCutLineBase>& chDataList, const char* chName);

        /// <summary>
        /// 判断是否可以恢复切割
        /// </summary>
        /// <param name="cutPosData">当前晶圆加工数据</param>
        /// <param name="cutSeq">切割顺序字符串</param>
        /// <returns>true: 可以恢复切割; false: 不可恢复</returns>
        bool CanResumeCut(const WaferProcData& cutPosData, const std::string& cutSeq);

        // ============================================================
        //  参数计算
        // ============================================================

        /// <summary>
        /// 计算通道对应的Die宽度（X方向）
        /// 通过查找正交通道（角度差90°）的Index累加得到
        /// </summary>
        /// <param name="chParam">当前通道参数</param>
        /// <param name="chListParam">通道列表参数</param>
        /// <param name="outDieWidth">[out] Die宽度</param>
        /// <returns>true: 计算成功; false: 未找到正交通道</returns>
        bool GetChDieWidth(const ChannelParameter* chParam,
                           const ChannelListParameter* chListParam,
                           double& outDieWidth);
    }
}
