using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace Fingerprint_Voting_System
{
    public partial class ViewCandidate : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-2NV2G1M\SQLEXPRESS;Integrated Security=True;Encrypt=False");

        public ViewCandidate()
        {
            InitializeComponent();
        }

        string cname = "";
        public ViewCandidate(string cname1)
        {
            InitializeComponent();
            cname = cname1;
        }

        private void ViewCandidate_Load(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("Select * from Candidate where Name ='"+cname+"'",con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
            textBox1.Text = cname;
            byte[] imageBytes = dr["Photo"] as byte[];
            if (imageBytes != null && imageBytes.Length > 0)
            {
                using (MemoryStream memoryStream = new MemoryStream(imageBytes))
                {
                    pictureBox2.Image = Image.FromStream(memoryStream);
                }
            }
            textBox2.Text = dr["Address"].ToString();
            con.Close();
        }
    }
}
