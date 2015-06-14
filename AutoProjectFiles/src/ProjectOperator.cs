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
                LogService.Instance.Open();
                LogService.Instance.LogMsg(">> Auto Project Files...");

                if (IsOperableProject())
                {
                    OperateProjectFiles();
                }
            }
            catch (Exception ex)
            {
                LogService.Instance.LogMsg("Error>> " + ex.Message + Environment.NewLine + ex.StackTrace);
            }
            finally
            {
                LogService.Instance.Close();
            }
        }

        private bool IsOperableProject()
        {
            string msg = FileOperateService.Instance.IsOperable(proj.FullName);
            if (msg != null)
            {
                LogService.Instance.LogMsg("Error>> " + msg);
            }
            return msg == null;
        }

        private void OperateProjectFiles()
        {
            List<string> newFiles = null;
            List<string> delFiles = null;

            LogService.Instance.LogMsg(DateTime.Now.ToString("HH:mm:ss.fff"));
            FileOperateService.Instance.GetOperableFiles(proj.FullName, out newFiles, out delFiles);
            List<string> addSuccList = new List<string>();
            List<string> delSuccList = new List<string>();
            try
            {
                LogService.Instance.LogMsg(DateTime.Now.ToString("HH:mm:ss.fff"));
                LogService.Instance.LogMsg(">> Excluding {0} entries from project...", delFiles.Count);
                ExcludeFiles(delFiles, ref delSuccList);
                LogService.Instance.LogMsgs(delSuccList);
                LogService.Instance.LogMsg(">> {0} entries done!", delSuccList.Count);

                LogService.Instance.LogMsg(DateTime.Now.ToString("HH:mm:ss.fff"));
                LogService.Instance.LogMsg(">> Including {0} entries to project...", newFiles.Count);
                IncludeFiles(newFiles, ref addSuccList);
                LogService.Instance.LogMsgs(addSuccList);
                LogService.Instance.LogMsg(">> {0} entries done!", addSuccList.Count);
                LogService.Instance.LogMsg(DateTime.Now.ToString("HH:mm:ss.fff"));
            }
            finally
            {
                FileOperateService.Instance.SaveOperableFiles(proj.FullName, addSuccList, delSuccList);
            }
        }

        private void ExcludeFiles(List<string> delFiles, ref List<string> succList)
        {
            delFiles.Sort();
            delFiles.Reverse();

            string dirName = Path.GetDirectoryName(proj.FullName);
            foreach (string f in delFiles)
            {
                string fPath = Path.Combine(dirName, f);
                if (Directory.Exists(fPath) || File.Exists(fPath))
                {
                    continue;
                }

                string[] pathParts = f.Split(Path.DirectorySeparatorChar);
                ProjectItem pItem = SearchProjectItem(proj.ProjectItems, pathParts, 0, pathParts.Length - 1);
                if (pItem != null)
                {
                    pItem.Delete();
                    succList.Add(f);
                }
            }
        }

        private void IncludeFiles(List<string> newFiles, ref List<string> succList)
        {
            newFiles.Sort();
            string dirName = Path.GetDirectoryName(proj.FullName);
            foreach (string f in newFiles)
            {
                string fPath = Path.Combine(dirName, f);
                if (Directory.Exists(fPath))
                {
                    string[] pathParts = f.Split(Path.DirectorySeparatorChar);
                    ProjectItem parentItem = SearchProjectItem(proj.ProjectItems, pathParts, 0, pathParts.Length - 2);
                    if (parentItem == null)
                    {
                        continue;
                    }

                    parentItem.ProjectItems.AddFromDirectory(fPath);
                    succList.Add(f);
                }
                else if (File.Exists(fPath))
                {
                    proj.ProjectItems.AddFromFile(fPath);
                    succList.Add(f);
                }
            }
        }

        private ProjectItem SearchProjectItem(ProjectItems projectItems, string[] pathParts, int curIndex, int endIndex)
        {
            foreach (ProjectItem pItem in projectItems)
            {
                if (pItem.Name != pathParts[curIndex])
                {
                    continue;
                }

                if (curIndex == endIndex)
                {
                    return pItem;
                }

                return SearchProjectItem(pItem.ProjectItems, pathParts, curIndex + 1, endIndex);
            }
            return null;
        }

        private Project proj;
    }
}
