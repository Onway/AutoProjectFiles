using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onway.AutoProjectFiles
{
    public interface Ilogger
    {
        void Open();

        void Close();

        void LogMsg(string fmt, params object[] args);

        void LogMsgs(List<string> msgLines);
    }

    public class LogService : Ilogger
    {
        public static LogService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LogService();
                }
                return instance;
            }
        }

        public void Open()
        {
            logger.Open();
        }

        public void Close()
        {
            logger.Close();
        }

        public void LogMsg(string fmt, params object[] args)
        {
            logger.LogMsg(fmt + Environment.NewLine, args);
        }

        public void LogMsgs(List<string> msgLines)
        {
            logger.LogMsgs(msgLines);
        }

        private LogService()
        {
            logger = new VsLogger();
        }

        private Ilogger logger = null;

        private static LogService instance;
    }

    public class VsLogger : Ilogger
    {
        public void Open()
        {
            outputPane = VsUtil.GetVsOutputWindowPane();
            outputPane.Clear();
        }

        public void Close()
        {
            outputPane = null;
        }

        public void LogMsg(string fmt, params object[] args)
        {
            outputPane.OutputString(string.Format(fmt, args));
        }

        public void LogMsgs(List<string> msgLines)
        {
            StringBuilder sb = new StringBuilder();
            msgLines.ForEach(line => sb.AppendLine(line));
            outputPane.OutputString(sb.ToString());
        }

        private Microsoft.VisualStudio.Shell.Interop.IVsOutputWindowPane outputPane;
    }
}
