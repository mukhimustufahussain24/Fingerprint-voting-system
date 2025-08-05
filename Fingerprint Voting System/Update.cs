using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing.Drawing2D;
using System.Linq;

namespace Fingerprint_Voting_System
{
    public partial class Update : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-2NV2G1M\SQLEXPRESS;Integrated Security=True;Encrypt=False");
        public Update()
        {
            InitializeComponent();
            panel2.Paint += new PaintEventHandler(panel2_Paint);
        }
        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            // Get the panel's bounds
            Rectangle panelBounds = panel2.ClientRectangle; // Use ClientRectangle to get the size without borders

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
            panel2.Region = region;

            // Create a gradient brush for the plastic-like effect
            LinearGradientBrush gradientBrush = new LinearGradientBrush(
                panelBounds,
                Color.FromArgb(200, Color.White), // Light color (adjust transparency as needed)
                Color.FromArgb(100, Color.LightGray), // Darker color (adjust transparency as needed)
                LinearGradientMode.Vertical);

            // Fill the entire panel with the gradient brush
            e.Graphics.FillPath(gradientBrush, path);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBox1.Texts))
                {
                    MessageBox.Show("Please enter the Voter ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!IsNumeric(textBox1.Texts))
                {
                    throw new Exception("Voter ID should be a numeric value.");
                }

                // Check if the voter ID exists in the database
                bool voterExists = CheckVoterExists(textBox1.Texts);

                if (!voterExists)
                {
                    MessageBox.Show("Voter ID not found in the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!string.IsNullOrEmpty(textBox2.Texts) && !IsAlphabet(textBox2.Texts))
                {
                    throw new Exception("Name should only contain alphabetic characters.");
                }

                if (!string.IsNullOrEmpty(textBox3.Texts) && (!IsNumeric(textBox3.Texts) || textBox3.Texts.Length != 10))
                {
                    throw new Exception("Mobile number should be a 10-digit numeric value.");
                }

                // Update voter information
                SqlCommand cmd = new SqlCommand("UPDATE Voter SET Name = @Name, Mobile = @Mobile, Address = @Address WHERE UserId = @UserId", con);
                cmd.Parameters.AddWithValue("@Name", textBox2.Texts);
                cmd.Parameters.AddWithValue("@Mobile", textBox3.Texts);
                cmd.Parameters.AddWithValue("@Address", textBox4.Texts);
                cmd.Parameters.AddWithValue("@UserId", textBox1.Texts);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Updated Successfully", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Clear input fields
                textBox1.Texts = "";
                textBox2.Texts = "";
                textBox3.Texts = "";
                textBox4.Texts = "";
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter a valid numeric value for Voter ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool CheckVoterExists(string voterId)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Voter WHERE UserId = @UserId", con))
            {
                cmd.Parameters.AddWithValue("@UserId", voterId);
                con.Open();
                int count = (int)cmd.ExecuteScalar();
                con.Close();

                return count > 0;
            }
        }
        // Check if a string contains only numeric characters
        private bool IsNumeric(string input)
        {
            return input.All(char.IsDigit);
        }

        // Check if a string contains only alphabet characters
        private bool IsAlphabet(string input)
        {
            return input.All(char.IsLetter);
        }

    }
}