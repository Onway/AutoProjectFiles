using System;
using System.Collections.Generic;
using System.IO;

namespace Onway.AutoProjectFiles
{
    public interface IFileOperator
    {
        string IsOperable(string projFullPath);

        void GetOperableFiles(string projFullPath, out List<string> newFiles, out List<string> delFiles);

        void SaveOperableFiles(string projFullPath, List<string> newFiles, List<string> delFiles);
    }

    public class FileOperateService : IFileOperator
    {
        public static FileOperateService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FileOperateService();
                }
                return instance;
            }
        }

        public string IsOperable(string projFullPath)
        {
            return oper.IsOperable(projFullPath);
        }

        public void GetOperableFiles(string projFullPath, out List<string> newFiles, out List<string> delFiles)
        {
            oper.GetOperableFiles(projFullPath, out newFiles, out delFiles);
        }

        public void SaveOperableFiles(string projFullPath, List<string> newFiles, List<string> delFiles)
        {
            oper.SaveOperableFiles(projFullPath, newFiles, delFiles);
        }

        private FileOperateService()
        {
            oper = new SnapshotOperator();
        }

        private IFileOperator oper;

        private static FileOperateService instance;
    }
}
