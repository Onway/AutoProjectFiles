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
using Microsoft.VisualStudio.Shell.Interop;

namespace Onway.AutoProjectFiles
{
    public static class VsUtil
    {
        public static string PackageTitle
        {
            get { return "Auto Project Files"; }
        }

        public static IVsUIShell VsUIShell
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a relative path from one file or folder to another.
        /// http://stackoverflow.com/questions/275689/how-to-get-relative-path-from-absolute-path
        /// </summary>
        /// <param name="fromPath">Contains the directory that defines the start of the relative path.</param>
        /// <param name="toPath">Contains the path that defines the endpoint of the relative path.</param>
        /// <returns>The relative path from the start directory to the end path.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static String MakeRelativePath(String fromPath, String toPath)
        {
            if (String.IsNullOrEmpty(fromPath)) throw new ArgumentNullException("fromPath");
            if (String.IsNullOrEmpty(toPath)) throw new ArgumentNullException("toPath");

            Uri fromUri = new Uri(fromPath);
            Uri toUri = new Uri(toPath);

            if (fromUri.Scheme != toUri.Scheme) { return toPath; } // path can't be made relative.

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            String relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            if (toUri.Scheme.ToUpperInvariant() == "FILE")
            {
                relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }

            return relativePath;
        }

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

        public static DTE2 GetCurrentDTE2()
        {
            //rot entry for visual studio running under current process.
            string rotEntry = String.Format("!VisualStudio.DTE.12.0:{0}", System.Diagnostics.Process.GetCurrentProcess().Id);
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
                if (displayName == rotEntry)
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
