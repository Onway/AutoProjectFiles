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
            //oper = new FileOperateTest();
            oper = new SnapshotOperator();
        }

        private IFileOperator oper;

        private static FileOperateService instance;
    }

    public class FileOperateTest : IFileOperator
    {
        public FileOperateTest()
        {
            projFile = @"E:\Codes\VisualStudio\Projects\Hamt\Hamt\Hamt.csproj";
            watchFile = Path.Combine(Path.GetDirectoryName(projFile), "WatchFile.txt");
        }

        public string IsOperable(string projFullPath)
        {
            if (projFullPath != projFile)
            {
                return "不支持的项目";
            }
            return null;
        }

        public void GetOperableFiles(string projFullPath, out List<string> newFiles, out List<string> delFiles)
        {
            newFiles = new List<string>();
            delFiles = new List<string>();

            if (!File.Exists(watchFile))
            {
                return;
            }
            foreach (string line in File.ReadAllLines(watchFile))
            {
                string[] parts = line.Split(',');
                if (parts[0] == "A")
                {
                    newFiles.Add(parts[1]);
                }
                else
                {
                    delFiles.Add(parts[1]);
                }
            }
        }

        public void SaveOperableFiles(string projFullPath, List<string> newFiles, List<string> delFiles)
        {
        }

        private string projFile;

        private string watchFile;
    }
}
