using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using System.Globalization;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace Onway.AutoProjectFiles
{
    public static class VsUtil
    {
        public static IVsUIShell VsUIShell
        {
            get;
            set;
        }

        /// <summary>
        /// 获取相对路径的所有文件
        /// </summary>
        /// <param name="rootDir"></param>
        /// <param name="relativeFolders"></param>
        /// <returns>相对路径内的所有文件</returns>
        public static List<string> DirectoryTraversal(string rootDir, List<string> relativeFolders)
        {
            List<string> retList = new List<string>();
            foreach (string folder in relativeFolders)
            {
                if (!Directory.Exists(Path.Combine(rootDir, folder)))
                {
                    continue;
                }

                retList.Add(folder);
                foreach (string entry in Directory.EnumerateFileSystemEntries(
                    Path.Combine(rootDir, folder), "*", SearchOption.AllDirectories))
                {
                    retList.Add(entry.Substring(rootDir.Length + 1));
                }
            }
            return retList;
        }

        public static int MessageBox(string title, string content, 
            OLEMSGBUTTON button = OLEMSGBUTTON.OLEMSGBUTTON_OK, 
            OLEMSGICON icon = OLEMSGICON.OLEMSGICON_INFO)
        {
            Guid clsid = Guid.Empty;
            int result;
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(VsUIShell.ShowMessageBox(
                       0,
                       ref clsid,
                       title,
                       content,
                       string.Empty,
                       0,
                       button,
                       OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST,
                       icon,
                       0,        // false  
                       out result));
            return result;
        }

        public static Project GetSelectedProject()
        {
            DTE2 dte2 = VsUtil.GetCurrentDTE2();
            UIHierarchy uih = (UIHierarchy)dte2.Windows.Item(EnvDTE.Constants.vsWindowKindSolutionExplorer).Object;

            Array selectedItems = (Array)uih.SelectedItems;
            UIHierarchyItem uihItem = (UIHierarchyItem)selectedItems.GetValue(0);

            Array projects = (Array)uihItem.DTE.ActiveSolutionProjects;
            return (Project)projects.GetValue(0);
        }

        public static IVsOutputWindowPane GetVsOutputWindowPane()
        {
            IVsOutputWindow outputWindow =
                Package.GetGlobalService(typeof(SVsOutputWindow)) as IVsOutputWindow;
            Guid guidGeneral = Microsoft.VisualStudio.VSConstants.OutputWindowPaneGuid.GeneralPane_guid;

            IVsOutputWindowPane pane;
            outputWindow.CreatePane(guidGeneral, "General", 1, 0);
            outputWindow.GetPane(guidGeneral, out pane);

            EnvDTE80.DTE2 dte2 = Package.GetGlobalService(typeof(EnvDTE.DTE)) as EnvDTE80.DTE2;
            dte2.ToolWindows.OutputWindow.Parent.Activate();

            pane.Activate();
            return pane;
        }

        private static DTE2 GetCurrentDTE2()
        {
            string prefix = "!VisualStudio.DTE.";
            string suffix = ":" + System.Diagnostics.Process.GetCurrentProcess().Id;
            IRunningObjectTable rot;
            GetRunningObjectTable(0, out rot);
            IEnumMoniker enumMoniker;
            rot.EnumRunning(out enumMoniker);
            enumMoniker.Reset();
            IntPtr fetched = IntPtr.Zero;
            IMoniker[] moniker = new IMoniker[1];
            while (enumMoniker.Next(1, moniker, fetched) == 0)
            {
                IBindCtx bindCtx;
                CreateBindCtx(0, out bindCtx);
                string displayName;
                moniker[0].GetDisplayName(bindCtx, null, out displayName);
                if (displayName.StartsWith(prefix) && displayName.EndsWith(suffix))
                {
                    object comObject;
                    rot.GetObject(moniker[0], out comObject);
                    return (EnvDTE80.DTE2)comObject;
                }
            }
            return null;
        }

        [DllImport("ole32.dll")]
        private static extern void CreateBindCtx(int reserved, out IBindCtx ppbc);
        [DllImport("ole32.dll")]
        private static extern void GetRunningObjectTable(int reserved, out IRunningObjectTable prot);
    }
}
