using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing.Drawing2D;

namespace Fingerprint_Voting_System
{
    public partial class Result : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-2NV2G1M\SQLEXPRESS;Integrated Security=True;Encrypt=False");

        public Result()
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
        private void Result_Load(object sender, EventArgs e)
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT EId FROM Election", con);
            DataSet ds = new DataSet();
            da.Fill(ds);

            int count = ds.Tables[0].Rows.Count;
            for (int i = 0; i < count; i++)
            {
                comboBox5.Items.Add(ds.Tables[0].Rows[i][0].ToString());
            }
        }
        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM Election WHERE EId = @EId", con))
            {
                cmd.Parameters.AddWithValue("@EId", comboBox5.Texts);
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        textBox3.Texts = dr["Topic"].ToString();
                        int count = Convert.ToInt32(dr["NoOfCand"]);

                        // Clear previous candidate names
                        textBox1.Texts = "";
                        textBox2.Texts = "";
                        textBox4.Texts = "";
                        textBox5.Texts = "";

                        string[] names = new string[count];

                        for (int i = 1; i <= count; i++)
                        {
                            string candName = dr["Cand" + i].ToString();
                            names[i - 1] = candName;

                            // Display candidate names in respective textboxes
                            switch (i)
                            {
                                case 1:
                                    textBox1.Texts = candName;
                                    break;
                                case 2:
                                    textBox2.Texts = candName;
                                    break;
                                case 3:
                                    textBox4.Texts = candName;
                                    break;
                                case 4:
                                    textBox5.Texts = candName;
                                    break;
                            }
                        }

                        int[] vote = new int[count];
                        dr.Close(); // Close the reader before executing another command

                        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Vote WHERE EID = @EId", con);
                        da.SelectCommand.Parameters.AddWithValue("@EId", comboBox5.Texts);
                        DataSet ds = new DataSet();
                        da.Fill(ds);

                        // Calculate votes for each candidate
                        for (int i = 0; i < count; i++)
                        {
                            vote[i] = ds.Tables[0].Select("Cand" + (i + 1) + " = '1'").Length;
                        }

                        // Determine the candidate with the most votes
                        int maxValue = vote.Max(); // Maximum number of votes received
                        int maxIndex = Array.IndexOf(vote, maxValue); // Index of the candidate with the most votes

                        // Display the winner's name and vote count
                        textBox9.Texts = names[maxIndex]; // Candidate's name
                        textBox8.Texts = maxValue.ToString(); // Number of votes

                        // Update the Election table with the result
                        cmd.CommandText = "UPDATE Election SET Result = @Result WHERE EId = @EId";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@Result", textBox9.Texts + "-" + textBox8.Texts);
                        cmd.Parameters.AddWithValue("@EId", comboBox5.Texts);
                        cmd.ExecuteNonQuery();
                    }
                }
                con.Close();
            }
        }

    }
}