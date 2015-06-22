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
            labelHelp.MouseLeave += LabelHelp_MouseLeave;
            labelHelp.MouseHover += LabelHelp_MouseHover;
            Init(projFullPath);
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.None;
            try
            {
                LogService.Instance.Open();
                LogService.Instance.LogMsg(">> {0}...", MyResources.StartSnapshotTitle);

                string[] folders = null;
                if (!IsValidInput(out folders))
                {
                    return;
                }

                string msg = string.Format(MyResources.SnapshotDoneFmt, CreateSnapshot(txtProjectFile.Text, folders));
                LogService.Instance.LogMsg(">> {0}", msg);
                DialogResult = System.Windows.Forms.DialogResult.OK;
                btnClose.PerformClick();
            }
            catch (Exception ex)
            {
                LogService.Instance.LogMsg("{0}>> {1}", MyResources.Error, ex.Message + Environment.NewLine + ex.StackTrace);
            }
            finally
            {
                LogService.Instance.Close();
            }
        }

        private void LabelHelp_MouseHover(object sender, EventArgs e)
        {
            toolTip.Show(helpTip, labelHelp, labelHelp.Width, labelHelp.Height ,10000);
        }

        private void LabelHelp_MouseLeave(object sender, EventArgs e)
        {
            toolTip.Hide(labelHelp);
        }

        private void Init(string projFullPath)
        {
            txtProjectFile.Text = projFullPath;
            ProjectSnapshot ps = SnapshotManager.Instance.GetSnapshot(projFullPath);
            if (ps != null)
            {
                txtSnapshotFolders.Text = string.Join(Environment.NewLine, ps.Folders);
                labelLastUpdate.Text = string.Format("{0}{1}", MyResources.LastUpdate, ps.LastUpdate);
            }
        }

        private bool IsValidInput(out string[] folders)
        {
            folders = null;

            if (string.IsNullOrEmpty(txtSnapshotFolders.Text))
            {
                LogService.Instance.LogMsg("{0}>> {1}", MyResources.Error, MyResources.NotSpecifyFolder);
                return false;
            }

            folders = txtSnapshotFolders.Text.Split(
                new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string f in folders)
            {
                if (f.IndexOf('.') != -1 || f.IndexOf("..") != -1)
                {
                    LogService.Instance.LogMsg("{0}>> {1}", MyResources.Error, MyResources.NotSupportNotation);
                    return false;
                }
                if (!Directory.Exists(Path.Combine(Path.GetDirectoryName(txtProjectFile.Text), f)))
                {
                    LogService.Instance.LogMsg("{0}>> {1}{2}", MyResources.Error, MyResources.NotExistFolder, 
                        Environment.NewLine + Path.Combine(Path.GetDirectoryName(txtProjectFile.Text), f));
                    return false;
                }
            }

            if (folders.Length == 0)
            {
                LogService.Instance.LogMsg("{0}>> {1}", MyResources.Error, MyResources.NotSpecifyFolder);
                return false;
            }

            return true;
        }

        private int CreateSnapshot(string projFullPath, string[] folders)
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

            int retCnt = ps.Entries.Count;
            ps.Save(); // it will reset ps.Entries to null; evil...
            return retCnt;
        }

        private string helpTip = MyResources.HelpTip;
    }
}
