using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Onway.AutoProjectFiles
{
    public partial class SnapshotForm : Form
    {
        public SnapshotForm(string projFullPath)
        {
            InitializeComponent();
            btnClose.Click += BtnClose_Click;
            btnCreate.Click += BtnCreate_Click;
            Init(projFullPath);
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.None;
            if (string.IsNullOrEmpty(txtProjectFile.Text)
                || !File.Exists(txtProjectFile.Text))
            {
                MessageBox.Show("Project file not found!", this.Text, 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(txtSnapshotFolders.Text))
            {
                MessageBox.Show("Snapshot folders not specified!", this.Text, 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string[] folders = txtSnapshotFolders.Text.Split(
                new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string f in folders)
            {
                if (!Directory.Exists(Path.Combine(Path.GetDirectoryName(txtProjectFile.Text), f)))
                {
                    MessageBox.Show("Snapshot folder not exist :" + Environment.NewLine + Path.Combine(Path.GetDirectoryName(txtProjectFile.Text), f),
                        this.Text,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            if (folders.Length == 0)
            {
                MessageBox.Show("Snapshot folders not specified!", this.Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            CreateSnapshot(txtProjectFile.Text, folders);
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void Init(string projFullPath)
        {
            txtProjectFile.Text = projFullPath;
            ProjectSnapshot ps = SnapshotManager.Instance.GetSnapshot(projFullPath);
            if (ps != null)
            {
                txtSnapshotFolders.Text = string.Join(Environment.NewLine, ps.Folders);
            }
        }

        private void CreateSnapshot(string projFullPath, string[] folders)
        {
            ProjectSnapshot ps = SnapshotManager.Instance.NewSnapshot(projFullPath);
            ps.Folders.Clear();
            foreach (string f in folders)
            {
                ps.Folders.Add(f.Replace('/', '\\'));
            }

            HashSet<string> hSet = ps.Entries;
            hSet.Clear();
            List<string> curEntries = VsUtil.DirectoryTraversal(
                Path.GetDirectoryName(ps.ProjectFile), ps.Folders);
            curEntries.ForEach(i => hSet.Add(i));

            ps.Save();
        }
    }
}
