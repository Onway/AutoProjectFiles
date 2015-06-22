namespace Onway.AutoProjectFiles
{
    partial class SnapshotForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SnapshotForm));
            this.label1 = new System.Windows.Forms.Label();
            this.txtProjectFile = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSnapshotFolders = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.labelHelp = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.labelLastUpdate = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            this.toolTip.SetToolTip(this.label1, resources.GetString("label1.ToolTip"));
            // 
            // txtProjectFile
            // 
            resources.ApplyResources(this.txtProjectFile, "txtProjectFile");
            this.txtProjectFile.Name = "txtProjectFile";
            this.txtProjectFile.ReadOnly = true;
            this.toolTip.SetToolTip(this.txtProjectFile, resources.GetString("txtProjectFile.ToolTip"));
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            this.toolTip.SetToolTip(this.label2, resources.GetString("label2.ToolTip"));
            // 
            // txtSnapshotFolders
            // 
            resources.ApplyResources(this.txtSnapshotFolders, "txtSnapshotFolders");
            this.txtSnapshotFolders.Name = "txtSnapshotFolders";
            this.toolTip.SetToolTip(this.txtSnapshotFolders, resources.GetString("txtSnapshotFolders.ToolTip"));
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Name = "btnClose";
            this.toolTip.SetToolTip(this.btnClose, resources.GetString("btnClose.ToolTip"));
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnCreate
            // 
            resources.ApplyResources(this.btnCreate, "btnCreate");
            this.btnCreate.Name = "btnCreate";
            this.toolTip.SetToolTip(this.btnCreate, resources.GetString("btnCreate.ToolTip"));
            this.btnCreate.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            this.toolTip.SetToolTip(this.label3, resources.GetString("label3.ToolTip"));
            // 
            // labelHelp
            // 
            resources.ApplyResources(this.labelHelp, "labelHelp");
            this.labelHelp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelHelp.Name = "labelHelp";
            this.toolTip.SetToolTip(this.labelHelp, resources.GetString("labelHelp.ToolTip"));
            // 
            // labelLastUpdate
            // 
            resources.ApplyResources(this.labelLastUpdate, "labelLastUpdate");
            this.labelLastUpdate.Name = "labelLastUpdate";
            this.toolTip.SetToolTip(this.labelLastUpdate, resources.GetString("labelLastUpdate.ToolTip"));
            // 
            // SnapshotForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelLastUpdate);
            this.Controls.Add(this.labelHelp);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.txtSnapshotFolders);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtProjectFile);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SnapshotForm";
            this.ShowIcon = false;
            this.toolTip.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtProjectFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSnapshotFolders;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelHelp;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label labelLastUpdate;
    }
}