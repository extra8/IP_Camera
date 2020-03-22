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
            stream = new MJPEGStream("http://192.168.1.9:4747/video");
            stream.NewFrame += StreamNewFrame;
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
            stream.Start();
        }

        private void DisconnectButton_Clicked(object sender, RoutedEventArgs e)
        {
            stream.Stop();
            VideoDisplay.Source = null;
        }
    }
}
