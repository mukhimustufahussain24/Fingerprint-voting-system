using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using OfficeOpenXml; // for Excel export
using iTextSharp.text; // for PDF export
using iTextSharp.text.pdf;
using System.IO;
using System.Drawing.Printing;
using System.Xml.Linq;

namespace Fingerprint_Voting_System
{
    public partial class ViewResult : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-2NV2G1M\SQLEXPRESS;Integrated Security=True;Encrypt=False");

        public ViewResult()
        {
            InitializeComponent();
        }

        private void ViewResult_Load(object sender, EventArgs e)
        {
            SqlDataAdapter da = new SqlDataAdapter("Select * from Election where Result <> 'N/A' ", con);
            DataTable ds = new DataTable();
            da.Fill(ds);
            dataGridView1.DataSource = ds;

            // Configure DataGridView columns
            ConfigureDataGridView();
        }

        private void ConfigureDataGridView()
        {
            DataGridViewColumn c1 = dataGridView1.Columns[0];
            c1.HeaderText = "Election ID";
            c1.Width = 110;
            DataGridViewColumn c2 = dataGridView1.Columns[1];
            c2.Width = 325;
            c2.HeaderText = "Topic";
            DataGridViewColumn c3 = dataGridView1.Columns[2];
            c3.HeaderText = "NoOfCand";
            c3.Width = 100;
            c3.Visible = false;
            DataGridViewColumn c4 = dataGridView1.Columns[3];
            c4.HeaderText = "Candidate 1";
            c4.Width = 130;
            DataGridViewColumn c5 = dataGridView1.Columns[4];
            c5.HeaderText = "Candidate 2";
            c5.Width = 130;
            DataGridViewColumn c6 = dataGridView1.Columns[5];
            c6.Width = 130;
            c6.HeaderText = "Candidate 3";
            DataGridViewColumn c7 = dataGridView1.Columns[6];
            c7.Width = 130;
            c7.HeaderText = "Candidate 4";
            DataGridViewColumn c8 = dataGridView1.Columns[7];
            c8.Width = 120;
            c8.HeaderText = "Date";
            DataGridViewColumn c9 = dataGridView1.Columns[8];
            c9.Width = 130;
            c9.HeaderText = "Winner";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Save data to Excel
            SaveResultsToExcelOrPdf();

            
        }

        private void SaveResultsToExcelOrPdf()
        {
            // Set the license context for EPPlus
            ExcelPackage.LicenseContext = LicenseContext.Commercial; // or LicenseContext.NonCommercial

            using (SaveFileDialog sfd = new SaveFileDialog { Filter = "Excel Workbook|*.xlsx|PDF File|*.pdf" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (sfd.FilterIndex == 1) // Excel
                    {
                        using (ExcelPackage pck = new ExcelPackage())
                        {
                            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Results");

                            // Assuming dataGridView1 is your grid
                            for (int i = 0; i < dataGridView1.Columns.Count; i++)
                            {
                                ws.Cells[1, i + 1].Value = dataGridView1.Columns[i].HeaderText;
                            }

                            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                            {
                                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                                {
                                    ws.Cells[i + 2, j + 1].Value = dataGridView1.Rows[i].Cells[j].Value;
                                }
                            }

                            File.WriteAllBytes(sfd.FileName, pck.GetAsByteArray());
                        }
                    }
                    else if (sfd.FilterIndex == 2) // PDF
                    {
                        using (FileStream fs = new FileStream(sfd.FileName, FileMode.Create))
                        {
                            Document doc = new Document(PageSize.A4);
                            PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                            doc.Open();

                            PdfPTable table = new PdfPTable(dataGridView1.Columns.Count);
                            foreach (DataGridViewColumn column in dataGridView1.Columns)
                            {
                                table.AddCell(new Phrase(column.HeaderText));
                            }

                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                foreach (DataGridViewCell cell in row.Cells)
                                {
                                    table.AddCell(new Phrase(cell.Value?.ToString()));
                                }
                            }

                            doc.Add(table);
                            doc.Close();
                        }
                    }
                }
            }
        }

    }
}
