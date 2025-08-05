
namespace Fingerprint_Voting_System
{
    partial class AdminMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdminMenu));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.addCandidateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addElectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addVotersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updatetoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.takeElectiontoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.caculateResultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewResultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addCandidateToolStripMenuItem,
            this.addElectionToolStripMenuItem,
            this.addVotersToolStripMenuItem,
            this.updatetoolStripMenuItem,
            this.takeElectiontoolStripMenuItem,
            this.caculateResultToolStripMenuItem,
            this.viewResultToolStripMenuItem,
            this.logoutToolStripMenuItem});
            this.menuStrip1.Name = "menuStrip1";
            // 
            // addCandidateToolStripMenuItem
            // 
            this.addCandidateToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.addCandidateToolStripMenuItem.Name = "addCandidateToolStripMenuItem";
            resources.ApplyResources(this.addCandidateToolStripMenuItem, "addCandidateToolStripMenuItem");
            this.addCandidateToolStripMenuItem.Click += new System.EventHandler(this.addCandidateToolStripMenuItem_Click);
            // 
            // addElectionToolStripMenuItem
            // 
            this.addElectionToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.addElectionToolStripMenuItem.Name = "addElectionToolStripMenuItem";
            resources.ApplyResources(this.addElectionToolStripMenuItem, "addElectionToolStripMenuItem");
            this.addElectionToolStripMenuItem.Click += new System.EventHandler(this.addElectionToolStripMenuItem_Click);
            // 
            // addVotersToolStripMenuItem
            // 
            this.addVotersToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.addVotersToolStripMenuItem.Name = "addVotersToolStripMenuItem";
            resources.ApplyResources(this.addVotersToolStripMenuItem, "addVotersToolStripMenuItem");
            this.addVotersToolStripMenuItem.Click += new System.EventHandler(this.addVotersToolStripMenuItem_Click);
            // 
            // updatetoolStripMenuItem
            // 
            this.updatetoolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.updatetoolStripMenuItem.Name = "updatetoolStripMenuItem";
            resources.ApplyResources(this.updatetoolStripMenuItem, "updatetoolStripMenuItem");
            this.updatetoolStripMenuItem.Click += new System.EventHandler(this.updateToolStripMenuItem_Click);
            // 
            // takeElectiontoolStripMenuItem
            // 
            this.takeElectiontoolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.takeElectiontoolStripMenuItem.Name = "takeElectiontoolStripMenuItem";
            resources.ApplyResources(this.takeElectiontoolStripMenuItem, "takeElectiontoolStripMenuItem");
            this.takeElectiontoolStripMenuItem.Click += new System.EventHandler(this.takeElectionToolStripMenuItem_Click);
            // 
            // caculateResultToolStripMenuItem
            // 
            this.caculateResultToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.caculateResultToolStripMenuItem.Name = "caculateResultToolStripMenuItem";
            resources.ApplyResources(this.caculateResultToolStripMenuItem, "caculateResultToolStripMenuItem");
            this.caculateResultToolStripMenuItem.Click += new System.EventHandler(this.caculateResultToolStripMenuItem_Click);
            // 
            // viewResultToolStripMenuItem
            // 
            this.viewResultToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.viewResultToolStripMenuItem.Name = "viewResultToolStripMenuItem";
            resources.ApplyResources(this.viewResultToolStripMenuItem, "viewResultToolStripMenuItem");
            this.viewResultToolStripMenuItem.Click += new System.EventHandler(this.viewResultToolStripMenuItem_Click);
            // 
            // logoutToolStripMenuItem
            // 
            this.logoutToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.logoutToolStripMenuItem.Name = "logoutToolStripMenuItem";
            resources.ApplyResources(this.logoutToolStripMenuItem, "logoutToolStripMenuItem");
            this.logoutToolStripMenuItem.Click += new System.EventHandler(this.logoutToolStripMenuItem_Click);
            // 
            // AdminMenu
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AdminMenu";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.AdminMenu_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem addCandidateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addElectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addVotersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewResultToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem caculateResultToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updatetoolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem takeElectiontoolStripMenuItem;
    }
}