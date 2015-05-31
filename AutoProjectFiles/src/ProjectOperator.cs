using System;
using System.Collections.Generic;
using System.IO;

using EnvDTE;

namespace Onway.AutoProjectFiles
{
    public class ProjectOperator
    {
        public ProjectOperator(Project proj)
        {
            this.proj = proj;
        }

        public void IncludeOrExcludeFiles()
        {
            try
            {
                string msg = FileOperateService.Instance.IsOperable(proj.FullName);
                if (msg != null)
                {
                    VsUtil.MessageBox(VsUtil.PackageTitle, msg,
                        Microsoft.VisualStudio.Shell.Interop.OLEMSGBUTTON.OLEMSGBUTTON_OK,
                        Microsoft.VisualStudio.Shell.Interop.OLEMSGICON.OLEMSGICON_WARNING);
                    return;
                }

                List<string> newFiles = null;
                List<string> delFiles = null;
                FileOperateService.Instance.GetOperableFiles(proj.FullName, out newFiles, out delFiles);

                List<string> addSuccList = new List<string>();
                List<string> delSuccList = new List<string>();
                try
                {
                    ExcludeFiles(delFiles, ref delSuccList);
                    IncludeFiles(newFiles, ref addSuccList);
                }
                finally
                {
                    FileOperateService.Instance.SaveOperableFiles(proj.FullName, addSuccList, delSuccList);
                }
                
                VsUtil.MessageBox(VsUtil.PackageTitle, "Finished");
            }
            catch (Exception ex)
            {
                VsUtil.MessageBox(VsUtil.PackageTitle, ex.Message + Environment.NewLine + ex.StackTrace,
                    Microsoft.VisualStudio.Shell.Interop.OLEMSGBUTTON.OLEMSGBUTTON_OK,
                    Microsoft.VisualStudio.Shell.Interop.OLEMSGICON.OLEMSGICON_CRITICAL);
            }
        }

        private void ExcludeFiles(List<string> delFiles, ref List<string> succList)
        {
            string dirName = Path.GetDirectoryName(proj.FullName);
            foreach (string f in delFiles)
            {
                string fPath = Path.Combine(dirName, f);
                if (Directory.Exists(fPath) || File.Exists(fPath))
                {
                    continue;
                }

                string[] pathParts = f.Split(Path.DirectorySeparatorChar);
                ProjectItem pItem = SearchProjectItem(proj.ProjectItems, pathParts, 0);
                if (pItem != null)
                {
                    pItem.Delete();
                    succList.Add(f);
                }
            }
        }

        private void IncludeFiles(List<string> newFiles, ref List<string> succList)
        {
            string dirName = Path.GetDirectoryName(proj.FullName);
            foreach (string f in newFiles)
            {
                string fPath = Path.Combine(dirName, f);
                if (Directory.Exists(fPath))
                {
                    proj.ProjectItems.AddFromDirectory(fPath);
                    succList.Add(f);
                }
                else if (File.Exists(fPath))
                {
                    proj.ProjectItems.AddFromFile(fPath);
                    succList.Add(f);
                }
            }
        }

        private ProjectItem SearchProjectItem(ProjectItems projectItems, string[] pathParts, int curIndex)
        {
            foreach (ProjectItem pItem in projectItems)
            {
                if (pItem.Name != pathParts[curIndex])
                {
                    continue;
                }

                if (curIndex == pathParts.Length - 1)
                {
                    return pItem;
                }

                return SearchProjectItem(pItem.ProjectItems, pathParts, curIndex + 1);
            }
            return null;
        }

        private Project proj;
    }
}
