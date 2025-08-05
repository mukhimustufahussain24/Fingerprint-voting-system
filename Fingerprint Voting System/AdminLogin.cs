using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using Fingerprint_Voting_System;
using System.Drawing.Drawing2D;
using System.Runtime.Remoting.Contexts;

namespace Fingerprint_Voting_Sysyem
{    
    public partial class AdminLogin : Form
    {
        public AdminLogin()
        {
            InitializeComponent();
            panel1.BackColor = Color.Transparent;
            panel1.Paint += new PaintEventHandler(panel1_Paint);
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // Get the panel's bounds
            Rectangle panelBounds = panel1.ClientRectangle; // Use ClientRectangle to get the size without borders

            // Create a graphics path with rounded corners
            GraphicsPath path = new GraphicsPath();
            int radius = 20; // Adjust the radius to change the border radius

            // Create a rounded rectangle path
            path.AddArc(panelBounds.X, panelBounds.Y, radius * 2, radius * 2, 180, 90);
            path.AddArc(panelBounds.Right - radius * 2, panelBounds.Y, radius * 2, radius * 2, 270, 90);
            path.AddArc(panelBounds.Right - radius * 2, panelBounds.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
            path.AddArc(panelBounds.X, panelBounds.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
            path.CloseAllFigures();

            // Create a region with the rounded rectangle path
            Region region = new Region(path);

            // Set the panel's region to the rounded rectangle region
            panel1.Region = region;

            // Create a gradient brush for the plastic-like effect
            LinearGradientBrush gradientBrush = new LinearGradientBrush(
                panelBounds,
                Color.FromArgb(100, Color.White), // Light color (adjust transparency as needed)
                Color.FromArgb(100, Color.LightGray), // Darker color (adjust transparency as needed)
                LinearGradientMode.Vertical);

            // Fill the entire panel with the gradient brush
            e.Graphics.FillPath(gradientBrush, path);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Texts = "";
            textBox2.Texts = "";
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Texts) && !string.IsNullOrEmpty(textBox2.Texts))
            {
                string query = "SELECT COUNT(*) FROM Login WHERE username = @Username COLLATE SQL_Latin1_General_CP1_CS_AS AND password = @Password COLLATE SQL_Latin1_General_CP1_CS_AS";
                string connectionString = @"Data Source=DESKTOP-2NV2G1M\SQLEXPRESS;Integrated Security=True;Encrypt=False";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@Username", textBox1.Texts);
                        command.Parameters.AddWithValue("@Password", textBox2.Texts);

                        int result = Convert.ToInt32(command.ExecuteScalar());

                        if (result > 0)
                        {
                            AdminMenu form2 = new AdminMenu();
                            this.Hide();
                            form2.Show();
                        }
                        else
                        {
                            MessageBox.Show("Username or Password should be correct", "Error !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    conn.Close();
                }
            }
            else
            {
                MessageBox.Show("Both Username and Password must be provided", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}