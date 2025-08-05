using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Drawing.Drawing2D;
using System.Linq;

namespace Fingerprint_Voting_System
{
    public partial class AddCandidate : Form
    {
	    SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-2NV2G1M\SQLEXPRESS;Integrated Security=True;Encrypt=False");       
        public AddCandidate()
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
                Color.FromArgb(100, Color.White), // Light color (adjust transparency as needed)
                Color.FromArgb(100, Color.LightGray), // Darker color (adjust transparency as needed)
                LinearGradientMode.Vertical);

            // Fill the entire panel with the gradient brush
            e.Graphics.FillPath(gradientBrush, path);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.jpeg; *.bmp; *.png)|*.jpg; *.jpeg; *.png; *.bmp";
            if (open.ShowDialog() == DialogResult.OK)
            {
                pictureBox2.Image = new Bitmap(open.FileName);
                pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Texts == "" || textBox2.Texts == "" || textBox3.Texts == "" || pictureBox2.Image == null)
            {
                MessageBox.Show("Please Fill all Data and Select an Image", "Error !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            else
            {
                try
                {
                    if (textBox2.Texts.Any(char.IsDigit))
                    {
                        throw new Exception("Name should not contain numbers.");
                    }

                    MemoryStream ms = new MemoryStream();
                    pictureBox2.Image.Save(ms, pictureBox2.Image.RawFormat);
                    byte[] imageBytes = ms.ToArray();

                    SqlCommand cmd = new SqlCommand("Insert into Candidate (CandId, Name, Address, Photo) " +
                        "Values (@CandId, @Name, @Address, @Photo)", con);
                    cmd.Parameters.AddWithValue("@CandId", textBox1.Texts);
                    cmd.Parameters.AddWithValue("@Name", textBox2.Texts);
                    cmd.Parameters.AddWithValue("@Address", textBox3.Texts);
                    cmd.Parameters.AddWithValue("@Photo", imageBytes);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Candidate Registered Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clear input fields
                    textBox1.Texts = "";
                    textBox2.Texts = "";
                    textBox3.Texts = "";
                    pictureBox2.Image = null;
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627) // Unique constraint violation error number
                    {
                        MessageBox.Show("Candidate with the same ID already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Candidate ID Should be in numeric. " , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(  ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
