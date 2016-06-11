using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Threading;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Windows.Threading;
using System.Drawing;



namespace VideoStreamer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Capture _capture = null;
        Image<Bgr, Byte> frame;
        
        double FrameRate = 0;
        double TotalFrames = 0;
        double Framesno = 0;
        double codec_double = 0;
        OpenFileDialog openFileDialog1 = new OpenFileDialog();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == true)
                LoadVideo(sender, e);
        }

       private void ReleaseData()
        {
            if (_capture != null)
                _capture.Dispose();
        }

        private void ProcessFrame(object sender, EventArgs arg)
        {
            try
            {
                
                if (frame != null)
                {
                        Bitmap image = frame.ToBitmap();
                        //Add streaming Logic Here
                        double time_index = _capture.GetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_POS_MSEC);
                         double framenumber = _capture.GetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_POS_FRAMES);
                          Thread.Sleep((int)(1000.0 / FrameRate));
                   }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }


        private void LoadVideo(object sender, EventArgs arg)
        {

             //dispose of old capture if one exists
            if (_capture != null)
            {
                ReleaseData();
            }
            {
                try
                {
                    _capture = null;
                    _capture = new Capture(openFileDialog1.FileName);
                     FrameRate = _capture.GetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FPS);
                    TotalFrames = _capture.GetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_COUNT);
                    codec_double = _capture.GetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FOURCC);
                    _capture.ImageGrabbed += ProcessFrame;
                    _capture.Start();
                   
                }
                catch (NullReferenceException excpt)
                {
                    MessageBox.Show(excpt.Message);
                }
            }


        }

    }
}


