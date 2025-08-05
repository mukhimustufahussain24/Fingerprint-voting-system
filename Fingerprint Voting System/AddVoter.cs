using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SecuGen.FDxSDKPro.Windows;
using System.Data.SqlClient;
using System.IO;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Data;
using System.Runtime.InteropServices;

namespace Fingerprint_Voting_System
{
    
    public partial class AddVoter : Form
    {
        //[DllImport(@"C:\Users\HP\Documents\SEM 5\Fingerprint Voting System\sgfplib.dll", CharSet = CharSet.Auto, SetLastError = true)]
        //public static extern int SGFPMInit();

        // Call the actual function
        //int result = SGFPMInit();

        private SGFingerPrintManager m_FPM;
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-2NV2G1M\SQLEXPRESS;Integrated Security=True;Encrypt=False"); 
        private bool m_LedOn = false;
        private Int32 m_ImageWidth;
        private Int32 m_ImageHeight;
        private Byte[] m_RegMin1;
        private Byte[] m_RegMin2;
        private Byte[] m_VrfMin;
        private SGFPMDeviceList[] m_DevList; // Used for EnumerateDevice
        public AddVoter()
        {
            InitializeComponent();
            panel2.Paint += new PaintEventHandler(panel2_Paint);
            panel1.Paint += new PaintEventHandler(panel1_Paint);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
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
        private void AddVoter_Load(object sender, EventArgs e)
        {
            m_LedOn = false;

            m_RegMin1 = new Byte[400];
            m_RegMin2 = new Byte[400];
            m_VrfMin = new Byte[400];
            m_FPM = new SGFingerPrintManager();
            EnumerateBtn_Click(sender, e);
        }
        private void EnumerateBtn_Click(object sender, System.EventArgs e)
        {
            Int32 iError;
            string enum_device;

            comboBoxDeviceName.Items.Clear();

            // Enumerate Device
            iError = m_FPM.EnumerateDevice();

            // Get enumeration info into SGFPMDeviceList
            m_DevList = new SGFPMDeviceList[m_FPM.NumberOfDevice];

            for (int i = 0; i < m_FPM.NumberOfDevice; i++)
            {
                m_DevList[i] = new SGFPMDeviceList();
                m_FPM.GetEnumDeviceInfo(i, m_DevList[i]);
                enum_device = m_DevList[i].DevName.ToString() + " : " + m_DevList[i].DevID;
                comboBoxDeviceName.Items.Add(enum_device);
            }

            if (comboBoxDeviceName.Items.Count > 0)
            {
                // Add Auto Selection
                enum_device = "Auto Selection";
                comboBoxDeviceName.Items.Add(enum_device);

                comboBoxDeviceName.SelectedIndex = 0;  //First selected one
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (m_FPM.NumberOfDevice == 0)
                return;

            Int32 iError;
            SGFPMDeviceName device_name;
            Int32 device_id;

            Int32 numberOfDevices = comboBoxDeviceName.Items.Count;
            Int32 deviceSelected = comboBoxDeviceName.SelectedIndex;
            Boolean autoSelection = (deviceSelected == (numberOfDevices - 1));  // Last index

            if (autoSelection)
            {
                // Order of search: Hamster IV(HFDU04) -> Plus(HFDU03) -> III (HFDU02)
                device_name = SGFPMDeviceName.DEV_AUTO;

                device_id = (Int32)(SGFPMPortAddr.USB_AUTO_DETECT);
            }
            else
            {
                device_name = m_DevList[deviceSelected].DevName;
                device_id = m_DevList[deviceSelected].DevID;
            }

            iError = m_FPM.Init(device_name);
            iError = m_FPM.OpenDevice(device_id);

            if (iError == (Int32)SGFPMError.ERROR_NONE)
            {
                GetBtn_Click(sender, e);
                panel1.Visible = false;
                panel2.Visible = true;
            }
            else
                DisplayError("OpenDevice()", iError);

        }
        private void GetBtn_Click(object sender, System.EventArgs e)
        {
            SGFPMDeviceInfoParam pInfo = new SGFPMDeviceInfoParam();
            Int32 iError = m_FPM.GetDeviceInfo(pInfo);

            if (iError == (Int32)SGFPMError.ERROR_NONE)
            {
                m_ImageWidth = pInfo.ImageWidth;
                m_ImageHeight = pInfo.ImageHeight;
                ASCIIEncoding encoding = new ASCIIEncoding();
            }
        }
        void DisplayError(string funcName, int iError)
        {
            string text = "";

            switch (iError)
            {
                case 0:                             //SGFDX_ERROR_NONE				= 0,
                    text = "Error none";
                    break;

                case 1:                             //SGFDX_ERROR_CREATION_FAILED	= 1,
                    text = "Can not create object";
                    break;

                case 2:                             //   SGFDX_ERROR_FUNCTION_FAILED	= 2,
                    text = "Function Failed";
                    break;

                case 3:                             //   SGFDX_ERROR_INVALID_PARAM	= 3,
                    text = "Invalid Parameter";
                    break;

                case 4:                          //   SGFDX_ERROR_NOT_USED			= 4,
                    text = "Not used function";
                    break;

                case 5:                                //SGFDX_ERROR_DLLLOAD_FAILED	= 5,
                    text = "Can not create object";
                    break;

                case 6:                                //SGFDX_ERROR_DLLLOAD_FAILED_DRV	= 6,
                    text = "Can not load device driver";
                    break;
                case 7:                                //SGFDX_ERROR_DLLLOAD_FAILED_ALGO = 7,
                    text = "Can not load sgfpamx.dll";
                    break;

                case 51:                //SGFDX_ERROR_SYSLOAD_FAILED	   = 51,	// system file load fail
                    text = "Can not load driver kernel file";
                    break;

                case 52:                //SGFDX_ERROR_INITIALIZE_FAILED  = 52,   // chip initialize fail
                    text = "Failed to initialize the device";
                    break;

                case 53:                //SGFDX_ERROR_LINE_DROPPED		   = 53,   // image data drop
                    text = "Data transmission is not good";
                    break;

                case 54:                //SGFDX_ERROR_TIME_OUT			   = 54,   // getliveimage timeout error
                    text = "Time out";
                    break;

                case 55:                //SGFDX_ERROR_DEVICE_NOT_FOUND	= 55,   // device not found
                    text = "Device not found";
                    break;

                case 56:                //SGFDX_ERROR_DRVLOAD_FAILED	   = 56,   // dll file load fail
                    text = "Can not load driver file";
                    break;

                case 57:                //SGFDX_ERROR_WRONG_IMAGE		   = 57,   // wrong image
                    text = "Wrong Image";
                    break;

                case 58:                //SGFDX_ERROR_LACK_OF_BANDWIDTH  = 58,   // USB Bandwith Lack Error
                    text = "Lack of USB Bandwith";
                    break;

                case 59:                //SGFDX_ERROR_DEV_ALREADY_OPEN	= 59,   // Device Exclusive access Error
                    text = "Device is already opened";
                    break;

                case 60:                //SGFDX_ERROR_GETSN_FAILED		   = 60,   // Fail to get Device Serial Number
                    text = "Device serial number error";
                    break;

                case 61:                //SGFDX_ERROR_UNSUPPORTED_DEV		   = 61,   // Unsupported device
                    text = "Unsupported device";
                    break;

                // Extract & Verification error
                case 101:                //SGFDX_ERROR_FEAT_NUMBER		= 101, // utoo small number of minutiae
                    text = "The number of minutiae is too small";
                    break;

                case 102:                //SGFDX_ERROR_INVALID_TEMPLATE_TYPE		= 102, // wrong template type
                    text = "Template is invalid";
                    break;

                case 103:                //SGFDX_ERROR_INVALID_TEMPLATE1		= 103, // wrong template type
                    text = "1st template is invalid";
                    break;

                case 104:                //SGFDX_ERROR_INVALID_TEMPLATE2		= 104, // vwrong template type
                    text = "2nd template is invalid";
                    break;

                case 105:                //SGFDX_ERROR_EXTRACT_FAIL		= 105, // extraction fail
                    text = "Minutiae extraction failed";
                    break;

                case 106:                //SGFDX_ERROR_MATCH_FAIL		= 106, // matching  fail
                    text = "Matching failed";
                    break;

            }

            text = funcName + " Error # " + iError + " :" + text;
            MessageBox.Show(text, "Error !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.jpeg; *.bmp; *.png)|*.jpg; *.jpeg; *.bmp; *.png";
            if (open.ShowDialog() == DialogResult.OK)
            {
                pictureBox2.Image = new Bitmap(open.FileName);
                pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            } 
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

                if (!IsNumeric(textBox3.Texts))
                {
                    MessageBox.Show("Mobile number should contain only numeric characters.", "Error !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (textBox3.Texts.Length != 10)
                {
                    MessageBox.Show("Mobile number should be exactly 10 digits.", "Error !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Int32 iError;
                Byte[] fp_image;
                Int32 img_qlty;

                fp_image = new Byte[m_ImageWidth * m_ImageHeight];
                img_qlty = 0;

                iError = m_FPM.GetImage(fp_image);
                m_FPM.GetImageQuality(m_ImageWidth, m_ImageHeight, fp_image, ref img_qlty);

                if (iError == (Int32)SGFPMError.ERROR_NONE)
                {
                    DrawImage(fp_image, pictureBox3);
                    iError = m_FPM.CreateTemplate(fp_image, m_RegMin1);

                    if (iError == (Int32)SGFPMError.ERROR_NONE)
                    {
                        pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
                        SqlCommand cmd = new SqlCommand("INSERT INTO Voter (UserId, Name, Mobile, Address, Image, Template) " +
                            "VALUES (@UserId, @Name, @Mobile, @Address, @Image, @Template)", con);

                        try
                        {
                            if (textBox2.Texts.Any(char.IsDigit))
                            {
                                throw new Exception("Name should not contain numbers.");
                            }
                            cmd.Parameters.AddWithValue("@UserId", textBox1.Texts);
                            cmd.Parameters.AddWithValue("@Name", textBox2.Texts);
                            cmd.Parameters.AddWithValue("@Mobile", textBox3.Texts);
                            cmd.Parameters.AddWithValue("@Address", textBox4.Texts);
                            cmd.Parameters.AddWithValue("@Image", ImageToByteArray(pictureBox2.Image)); // Convert image to byte array
                            cmd.Parameters.AddWithValue("@Template", fp_image);

                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();

                            MessageBox.Show("Voter Registered Successfully", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Reset input fields and images
                            textBox1.Texts = "";
                            textBox2.Texts = "";
                            textBox3.Texts = "";
                            textBox4.Texts = "";
                            pictureBox2.Image = null;
                            pictureBox3.Image = null;
                        }
                        catch (SqlException ex)
                        {
                            if (ex.Number == 2627) // Unique constraint violation error number
                            {
                                MessageBox.Show("A voter ID already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                MessageBox.Show("Voter ID Should be in numeric.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            if (con.State == ConnectionState.Open)
                            {
                                con.Close(); // Ensure the connection is closed in all cases
                            }
                        }
                    }
                    else
                    {
                        DisplayError("CreateTemplate()", iError);
                    }
                }
                else
                {
                    MessageBox.Show("Finger Capturing Failed", "Error !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter a valid 10-digit numeric mobile number.", "Error !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Check if a string contains only numeric characters
        private bool IsNumeric(string input)
        {
            return input.All(char.IsDigit);
        }

        private byte[] ImageToByteArray(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png); // You can choose a different image format
                return ms.ToArray();
            }
        }
        private void DrawImage(Byte[] imgData, PictureBox picBox)
        {
            int colorval;
            Bitmap bmp = new Bitmap(m_ImageWidth, m_ImageHeight);
            picBox.Image = (Image)bmp;

            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    colorval = (int)imgData[(j * m_ImageWidth) + i];
                    bmp.SetPixel(i, j, Color.FromArgb(colorval, colorval, colorval));
                }
            }
            picBox.Refresh();
        }
       
    }
}
