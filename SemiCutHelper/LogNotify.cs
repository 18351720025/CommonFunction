using System;
using System.Collections.Generic;
using System.Text;

namespace SemiCutHelper
{
    public static class LogNotify
    {
        public static event Action<string, string>? LogChanged;

        /// <summary>
        /// 日志变更通知
        /// </summary>
        /// <param name="title">日志标题</param>
        /// <param name="message">日志内容</param>
        public static void NotifyLogChanged(string title , string message)
        {
            LogChanged?.Invoke(title, message);
        }
    }
}
