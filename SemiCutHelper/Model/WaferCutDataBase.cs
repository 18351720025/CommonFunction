using System;
using System.Collections.Generic;
using System.Text;

namespace SemiCutHelper.Model
{
    public abstract class WaferCutDataBase<TChCutData, TCutLineBase, TCutCmdBase>
        where TCutLineBase : CutLineBase
        where TCutCmdBase : CutCommandBase
        where TChCutData : ChannelCutData<TCutLineBase, TCutCmdBase>
    {

        /// <summary>
        /// Cut Line Data For Each Channel 
        /// </summary>
        public List<TChCutData> CutChannels { get; set; } = new List<TChCutData>();

        /// <summary>
        /// Gets or sets the identifier of the currently active channel.
        /// </summary>
        public string ActiveChannelId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the current status of the wafer.
        /// </summary>
        public WaferProcessStatus WaferStatus { get; set; } = WaferProcessStatus.Idle;
    }
}
