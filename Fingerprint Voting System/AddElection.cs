using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing.Drawing2D;

namespace Fingerprint_Voting_System
{
    public partial class AddElection : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-2NV2G1M\SQLEXPRESS;Integrated Security=True;Encrypt=False");

        public AddElection()
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
                // Check if any fields are empty
                if (string.IsNullOrEmpty(textBox1.Texts) || string.IsNullOrEmpty(textBox2.Texts) ||
                    string.IsNullOrEmpty(comboBox5.Texts) || string.IsNullOrEmpty(comboBox1.Texts) ||
                    string.IsNullOrEmpty(comboBox2.Texts) || string.IsNullOrEmpty(comboBox3.Texts) ||
                    string.IsNullOrEmpty(comboBox4.Texts))
                {
                    MessageBox.Show("Please Fill all Data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // Check if Election ID is numeric
                    if (!int.TryParse(textBox1.Texts, out int electionId))
                    {
                        MessageBox.Show("Election ID should be numeric.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string date = dateTimePicker1.Value.ToString("yyyy-MM-dd"); // Use ISO date format

                    using (SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-2NV2G1M\SQLEXPRESS;Integrated Security=True;Encrypt=False"))
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand(
                            "Insert into Election (EId, Topic, NoOfCand, Cand1, Cand2, Cand3, Cand4, Date) " +
                            "Values (@EId, @EName, @ECountry, @EProvince, @ECity, @ELocation, @ECandidates, @EDate)", con);

                        cmd.Parameters.AddWithValue("@EId", electionId);
                        cmd.Parameters.AddWithValue("@EName", textBox2.Texts);
                        cmd.Parameters.AddWithValue("@ECountry", comboBox5.Texts);
                        cmd.Parameters.AddWithValue("@EProvince", comboBox1.Texts);
                        cmd.Parameters.AddWithValue("@ECity", comboBox2.Texts);
                        cmd.Parameters.AddWithValue("@ELocation", comboBox3.Texts);
                        cmd.Parameters.AddWithValue("@ECandidates", comboBox4.Texts);
                        cmd.Parameters.AddWithValue("@EDate", date);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Election Registered Successfully", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Reset input fields
                    textBox1.Texts = "";
                    textBox2.Texts = "";
                    comboBox1.SelectedIndex = 0;
                    comboBox2.SelectedIndex = 0;
                    comboBox3.SelectedIndex = 0;
                    comboBox4.SelectedIndex = 0;
                    comboBox5.SelectedIndex = 0;
                    dateTimePicker1.Value = DateTime.Now; // Reset date picker to the current date
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627) // Unique constraint violation error number
                {
                    MessageBox.Show("Election ID should be unique. Please use a unique ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddElection_Load(object sender, EventArgs e)
        {
            SqlDataAdapter da = new SqlDataAdapter("Select Name From Candidate", con);
            DataSet ds = new DataSet();
            da.Fill(ds);

            int cou = ds.Tables[0].Rows.Count;
            for (int i = 0; i < cou; i++)
            {
                comboBox1.Items.Add(ds.Tables[0].Rows[i][0].ToString());
                comboBox2.Items.Add(ds.Tables[0].Rows[i][0].ToString());
                comboBox3.Items.Add(ds.Tables[0].Rows[i][0].ToString());
                comboBox4.Items.Add(ds.Tables[0].Rows[i][0].ToString());
                comboBox1.SelectedIndex = 0;
                comboBox2.SelectedIndex = 0;
                comboBox3.SelectedIndex = 0;
                comboBox4.SelectedIndex = 0;
                comboBox5.SelectedIndex = 0;
            }
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox5.Texts == "2")
            {
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
                comboBox3.Enabled = false;
                comboBox4.Enabled = false;
                comboBox3.Texts = "N/A";
                comboBox4.Texts = "N/A";
            }
            else if (comboBox5.Texts == "3")
            {
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
                comboBox3.Enabled = true;
                comboBox4.Enabled = false;
                comboBox4.Texts = "N/A";
                comboBox3.Texts = "--Select--";
            }
            else if (comboBox5.Texts == "4")
            {
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
                comboBox3.Enabled = true;
                comboBox4.Enabled = true;
                comboBox3.Texts = "--Select--";
                comboBox4.Texts = "--Select--";
            }
        }
    }
}