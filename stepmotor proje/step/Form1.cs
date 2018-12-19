using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Drawing.Imaging;
using System.IO.Ports;

namespace step
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private FilterInfoCollection video;
        private VideoCaptureDevice final;
        int x;
        int R, G, B;
        int i;

        private void checBox1_CheckedChanged(object sender, EventArgs e)
        {
            R = 200;
            G = 20;
            B = 20;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            R = 20;
            G = 200;
            B = 20;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            R = 20;
            G = 20;
            B = 200;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
                final = new VideoCaptureDevice(video[comboBox1.SelectedIndex].MonikerString);
                final.NewFrame += new NewFrameEventHandler(final_NewFrame);
                final.Start();
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            serialPort1.BaudRate = 9600;
            serialPort1.PortName = comboBox2.SelectedItem.ToString();
            serialPort1.Open();
            if (serialPort1.IsOpen == true)
            {
                checkBox5.Enabled = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            video = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo kamera in video)
            {
                comboBox1.Items.Add(kamera.Name);
            }
            comboBox1.SelectedIndex = 0;
            comboBox2.DataSource = SerialPort.GetPortNames();
            checkBox4.Enabled = true;
            int port = 0;
            port = comboBox2.Items.Count;
        }
        public void dongu(Bitmap image)
        {
            if(x>=0 && x<=640 )
            {
                if (x <= 210)
                {
                    serialPort1.Write("2");
                }

                else if (x > 210 && x <= 480)
                {
                    serialPort1.Write("0");
                }
                else
                {
                    serialPort1.Write("1");
                }
            }
            else
            {
                serialPort1.Write("0");
            }
            
        }

        

        private void final_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap goruntu = (Bitmap)eventArgs.Frame.Clone();
            Bitmap goruntu2 = (Bitmap)eventArgs.Frame.Clone();
            Mirror filter2 = new Mirror(false, true);
            filter2.ApplyInPlace(goruntu);
            pictureBox1.Image = goruntu;

            EuclideanColorFiltering filter = new EuclideanColorFiltering();
            filter.CenterColor = new RGB(Color.FromArgb(R, G, B));
            filter.Radius = 100;
            Mirror filter1 = new Mirror(false, true);
            filter1.ApplyInPlace(goruntu2);
            filter.ApplyInPlace(goruntu2);
            nesnebul(goruntu2);
            dongu(goruntu2);

            OtsuThreshold otsuFiltre = new OtsuThreshold();
        }
        private void nesnebul(Bitmap image)
        {
            BlobCounter bc = new BlobCounter();
            bc.MinWidth = 5;
            bc.MinHeight = 5;
            bc.FilterBlobs = true;
            bc.ObjectsOrder = ObjectsOrder.Size;

            bc.ProcessImage(image);
            Rectangle[] rects = bc.GetObjectsRectangles();
            Blob[] blobs = bc.GetObjectsInformation();
            pictureBox2.Image = image;

            foreach (Rectangle rect in rects)
            {
                Rectangle obje = rects[i];
                int obX = obje.X + (obje.Width/2);
                x = obX;
            }
        }
    }
}
