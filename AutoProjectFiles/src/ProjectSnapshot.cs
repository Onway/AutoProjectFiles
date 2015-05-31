using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Onway.AutoProjectFiles
{
    public class SnapshotManager
    {
        public static SnapshotManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SnapshotManager();
                }
                return instance;
            }
        }

        public ProjectSnapshot GetSnapshot(string projFullPath)
        {
            foreach (ProjectSnapshot ps in Snapshots())
            {
                if (ps.ProjectFile == projFullPath)
                {
                    return ps;
                }
            }
            return null;
        }

        public ProjectSnapshot NewSnapshot(string projFullPath)
        {
            ProjectSnapshot ps = GetSnapshot(projFullPath);
            if (ps != null)
            {
                return ps;
            }

            string saveFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                @"AutoProjectFiles\Snapshots\" + Guid.NewGuid().ToString() + ".xml");
            ps = new ProjectSnapshot(saveFile);
            ps.ProjectFile = projFullPath;
            ps.Save();
            Snapshots().Add(ps);
            return ps;
        }

        private SnapshotManager()
        {
        }

        private List<ProjectSnapshot> Snapshots()
        {
            if (snapshots == null)
            {
                LoadSnapshots();
            }
            return snapshots;
        }

        private void LoadSnapshots()
        {
            string appDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AutoProjectFiles");
            if (!Directory.Exists(appDir))
            {
                Directory.CreateDirectory(appDir);
            }

            string snapshotDir = Path.Combine(appDir, "Snapshots");
            if (!Directory.Exists(snapshotDir))
            {
                Directory.CreateDirectory(snapshotDir);
            }

            snapshots = new List<ProjectSnapshot>();
            foreach (string f in Directory.GetFiles(snapshotDir, "*.xml"))
            {
                ProjectSnapshot ps = new ProjectSnapshot(f);
                ps.Load();
                snapshots.Add(ps);
            }
        }

        private List<ProjectSnapshot> snapshots;

        private static SnapshotManager instance;
    }

    public class ProjectSnapshot
    {
        public ProjectSnapshot(string fullPath)
        {
            this.fullPath = fullPath;
            Extensions = new List<string>();
            Folders = new List<string>();
            entries = null;
        }

        public string ProjectFile
        {
            get;
            set;
        }

        public List<string> Extensions
        {
            get;
            set;
        }

        public List<string> Folders
        {
            get;
            set;
        }

        public HashSet<string> Entries
        {
            get
            {
                if (entries == null)
                {
                    LoadEntries();
                }
                return entries;
            }
        }

        public void Load()
        {
            XDocument xDoc = XDocument.Load(fullPath);
            XElement rootNode = xDoc.Element("Snapshot");

            this.ProjectFile = rootNode.Element("ProjectFile").Value;

            foreach (XElement item in rootNode.Element("Extensions").Elements("Item"))
            {
                this.Extensions.Add(item.Value);
            }

            foreach (XElement item in rootNode.Element("Folders").Elements("Item"))
            {
                this.Folders.Add(item.Value);
            }
        }

        public void Save()
        {
            if (entries == null)
            {
                LoadEntries();
            }

            XDocument xDoc = new XDocument();
            XElement rootNode = new XElement("Snapshot");
            xDoc.Add(rootNode);

            XElement projectFileNode = new XElement("ProjectFile", this.ProjectFile);
            rootNode.Add(projectFileNode);

            XElement extensionsNode = new XElement("Extensions");
            rootNode.Add(extensionsNode);
            foreach (string item in Extensions)
            {
                extensionsNode.Add(new XElement("Item", item));
            }

            XElement foldersNode = new XElement("Folders");
            rootNode.Add(foldersNode);
            foreach (string item in Folders)
            {
                foldersNode.Add(new XElement("Item", item));
            }

            XElement entriesNode = new XElement("Entries");
            rootNode.Add(entriesNode);
            foreach (string item in entries)
            {
                entriesNode.Add(new XElement("Item", item));
            }

            xDoc.Save(fullPath);
            entries = null;
        }

        private void LoadEntries()
        {
            entries = new HashSet<string>();
            if (!File.Exists(fullPath))
            {
                return;
            }

            XDocument xDoc = XDocument.Load(fullPath);
            foreach (XElement item in xDoc.Element("Snapshot").Element("Entries").Elements("Item"))
            {
                entries.Add(item.Value);
            }
        }

        private string fullPath;

        private HashSet<string> entries;
    }

    public class SnapshotOperator : IFileOperator
    {
        public string IsOperable(string projFullPath)
        {
            if (SnapshotManager.Instance.GetSnapshot(projFullPath) != null)
            {
                return null;
            }
            return "There is no snapshot for this project:" + Environment.NewLine + projFullPath;
        }

        public void GetOperableFiles(string projFullPath, out List<string> newFiles, out List<string> delFiles)
        {
            ProjectSnapshot ps = SnapshotManager.Instance.GetSnapshot(projFullPath);
            if (ps == null)
            {
                newFiles = new List<string>();
                delFiles = new List<string>();
            }
            else
            {
                GetOperableFiles(ps, out newFiles, out delFiles);
            }
        }

        public void SaveOperableFiles(string projFullPath, List<string> newFiles, List<string> delFiles)
        {
            ProjectSnapshot ps = SnapshotManager.Instance.GetSnapshot(projFullPath);
            if (ps != null)
            {
                HashSet<string> entries = ps.Entries;
                newFiles.ForEach(n => entries.Add(n));
                delFiles.ForEach(d => entries.Remove(d));
                ps.Save();
            }
        }

        private void GetOperableFiles(ProjectSnapshot ps, out List<string> newFiles, out List<string> delFiles)
        {
            List<string> currentSet = VsUtil.DirectoryTraversal(Path.GetDirectoryName(ps.ProjectFile), ps.Folders);

            HashSet<string> newSet = new HashSet<string>(currentSet);
            newSet.ExceptWith(ps.Entries);
            newFiles = new List<string>(newSet);

            HashSet<string> delSet = new HashSet<string>(ps.Entries);
            delSet.ExceptWith(currentSet);
            delFiles = new List<string>(delSet);
        }
    }
}
