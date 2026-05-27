using SemiCutHelper.Model;
using SemiCutHelper.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace SemiCutHelper
{
    public class CutHelper
    {
        /// <summary>
        /// waferCutData 是 CutHelper 的核心数据结构，包含了当前晶圆的切割状态、已定义的切割线、通道信息等。
        /// </summary>
        private WaferCutData _waferCutData;

        public CutHelper(WaferCutData waferCutData)
        {
            _waferCutData = waferCutData;
        }

        /// <summary>
        /// Sets the first cut line for the specified channel.
        /// </summary>
        /// <param name="chName">The name of the channel for which to set the first cut line. Cannot be null or empty.</param>
        /// <param name="firstLine">The coordinates of the first cut line to assign to the channel.</param>
        /// <returns>true if the first cut line was set successfully; otherwise, false.</returns>
        public void SetChannelFirstCutLine(string chName, Point firstLine)
        {
           if(_waferCutData.FindChannel(chName) is ChannelCutData channel)
            {
                channel.FirstLineOriginX = firstLine.X;
                channel.FirstLineOriginY = firstLine.Y;
            }
            else
            {
                _waferCutData.CutChannels.Add(new ChannelCutData
                {
                    Name = chName,
                    FirstLineOriginX = firstLine.X,
                    FirstLineOriginY = firstLine.Y
                });
            }
        }

        /// <summary>
        /// Creates a cut plan for the specified channel using the provided wafer information, step collection, cut
        /// region, and X-axis acceleration.
        /// </summary>
        /// <param name="chName">The name of the channel for which to create the cut plan. Cannot be null or empty.</param>
        /// <param name="waferInfo">The wafer information used to generate the cut plan.</param>
        /// <param name="innerStepCollection">A read-only collection of inner steps that define the step sequence for the cut plan. Must not be null or
        /// empty.</param>
        /// <param name="cutRegionExt">The extended cut region parameters that specify the area to be processed.</param>
        /// <param name="accelX">The acceleration value along the X-axis to be used during the cut plan generation.</param>
        /// <returns>true if the cut plan is successfully created for the specified channel; otherwise, false.</returns>
        public bool CreateCutPlan(string chName , in WaferInfo waferInfo, in IReadOnlyCollection<InnerStep> innerStepCollection, in CutRegionExt cutRegionExt, double accelX)
        {
            if (_waferCutData.FindChannel(chName) is not ChannelCutData channel)
            {
                return false;
            }
            channel.DefinedLines.Clear();
            channel.CommandSequence.Clear();
            double yTotalStep = innerStepCollection.Sum(s => s.Step * Math.Max(s.Count, 1));
            double yStart = waferInfo.Center.Y - waferInfo.Height * 0.5 - cutRegionExt.YExt;
            double yEnd = waferInfo.Center.Y + waferInfo.Height*0.5 + cutRegionExt.YExt;
            if (waferInfo.HasFlat)
            {
                double flatY = Math.Abs(waferInfo.Height  *waferInfo.Height*0.25 - waferInfo.FlatLength * waferInfo.FlatLength *0.25);
                if(waferInfo.FlatDirection == Direction.NEGATIVE_Y && Math.Abs(waferInfo.Angle) <= 45.0 || 
                    waferInfo.FlatDirection == Direction.POSITIVE_X && Math.Abs(waferInfo.Angle - 90.0) <= 45.0 )
                {
                    yStart = waferInfo.Center.Y - flatY - cutRegionExt.YExt;
                }
                else if(waferInfo.FlatDirection == Direction.POSITIVE_Y && Math.Abs(waferInfo.Angle) <= 45.0 || 
                    waferInfo.FlatDirection == Direction.NEGATIVE_X && Math.Abs(waferInfo.Angle - 90.0) <= 45.0)
                {
                    yEnd = waferInfo.Center.Y + flatY + cutRegionExt.YExt;
                }
            }
            int count1 = (int)((channel.FirstLineOriginY - yStart) / yTotalStep);
            double yStartAdjusted = count1 * yTotalStep + channel.FirstLineOriginY;
            int count2 = (int)((channel.FirstLineOriginY - yEnd) / yTotalStep);
            double yEndAdjusted = count2 * yTotalStep + channel.FirstLineOriginY;
            int totalLineCount = count1 + count2 + 1;
            LogNotify.NotifyLogChanged("Cut Plan Generation", $"Generating cut plan for channel '{chName}' with total line count: {totalLineCount}.");

        }
    }
}
