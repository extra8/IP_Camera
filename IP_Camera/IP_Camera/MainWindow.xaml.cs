using AForge.Video;
using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace IP_Camera
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MJPEGStream stream;

        public MainWindow()
        {
            InitializeComponent();
        }

        void StreamNewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            { 
                Dispatcher.Invoke(() =>
                {
                    Bitmap bmp = (Bitmap)eventArgs.Frame.Clone();
                    VideoDisplay.Source = Convert(bmp);
                });   
            }
            catch(Exception)
            {
            }
        }

        public BitmapImage Convert(Bitmap src)
        {
            MemoryStream ms = new MemoryStream();
            ((System.Drawing.Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }

        private void ConnectButton_Clicked(object sender, RoutedEventArgs e)
        {
            var streamSource = ProcessCameraUrl(CameraUrlTextBox.Text);

            stream = new MJPEGStream(streamSource);
            stream.NewFrame += StreamNewFrame;

            stream.Start();
        }

        // http://192.168.1.9:4747/video
        private string ProcessCameraUrl(string cameraUrl)
        {
            var lowerCameraUrl = cameraUrl.ToLower();
            if (lowerCameraUrl.Contains("http://") ||  lowerCameraUrl.Contains("https://"))
            {
                return cameraUrl;
            }
            else
            {
                return $"http://{cameraUrl}";
            }
        }

        private void DisconnectButton_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                stream.Stop();
                stream = null;
                VideoDisplay.Source = null;
            }
            catch(NullReferenceException)
            {

            }
        }
    }
}
