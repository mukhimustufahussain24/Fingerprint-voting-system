using System;
using System.Windows.Forms;
using Fingerprint_Voting_Sysyem;

namespace Fingerprint_Voting_System
{
    public partial class AdminMenu : Form
    {
        public AdminMenu()
        {
            InitializeComponent();
        }
        private void addCandidateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddCandidate a = new AddCandidate();
            a.MdiParent = this;
            a.Show();
        }
        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            AdminLogin a = new AdminLogin();
            a.Show();
            //Application.Exit();
        }
        private void addElectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddElection a = new AddElection();
            a.MdiParent = this;
            a.Show();
        }
        private void addVotersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddVoter a = new AddVoter();
            a.MdiParent = this;
            
            a.Show();
        }
        private void viewResultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewResult a = new ViewResult();
            a.MdiParent = this;
            a.Show();
        }
        private void viewCandidateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewCandidate a = new ViewCandidate();
            a.MdiParent = this;
            a.Show();
        }
        private void caculateResultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Result a = new Result();
            a.MdiParent = this;
            a.Show();
        }
        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Update a = new Update();
            a.MdiParent = this;
            a.Show();
        }
        private void takeElectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TakeElection a = new TakeElection();
            a.MdiParent = this;
            a.Show();
        }
        private void AdminMenu_Load(object sender, EventArgs e)
        {
            
        }
    }
}
