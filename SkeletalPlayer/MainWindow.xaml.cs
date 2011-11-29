/////////////////////////////////////////////////////////////////////////
//
// This module contains code to do Kinect NUI initialization and
// processing and also to display NUI streams on screen.
//
// Copyright © Microsoft Corporation.  All rights reserved.  
// This code is licensed under the terms of the 
// Microsoft Kinect for Windows SDK (Beta) from Microsoft Research 
// License Agreement: http://research.microsoft.com/KinectSDK-ToU
//
/////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Research.Kinect.Nui;
using VirtualKinect;
using System.Windows.Forms;
using System.Windows.Threading;
using System.Diagnostics;

namespace SkeletalViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            stopwatch = new Stopwatch();
            dispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, timerSpanInMilliseconds);
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);

        }
        private bool reachEndOfSequence;
        DispatcherTimer dispatcherTimer;
        Microsoft.Research.Kinect.Nui.Runtime nui;
        int totalFrames = 0;
        int lastFrames = 0;
        DateTime lastTime = DateTime.MaxValue;
        private int timerSpanInMilliseconds = 100;
        private bool playingSequenceNow;

        //For Virtual Kinect Recording and playing
        //  VirtualKinect.Recorder recorder;
        VirtualKinect.Player player;

        Stopwatch stopwatch;

        // We want to control how depth data gets converted into false-color data
        // for more intuitive visualization, so we keep 32-bit color frame buffer versions of
        // these, to be updated whenever we receive and process a 16-bit frame.
        const int RED_IDX = 2;
        const int GREEN_IDX = 1;
        const int BLUE_IDX = 0;
        byte[] depthFrame32 = new byte[320 * 240 * 4];


        Dictionary<Microsoft.Research.Kinect.Nui.JointID, Brush> jointColors = new Dictionary<Microsoft.Research.Kinect.Nui.JointID, Brush>() { 
            {Microsoft.Research.Kinect.Nui.JointID.HipCenter, new SolidColorBrush(Color.FromRgb(169, 176, 155))},
            {Microsoft.Research.Kinect.Nui.JointID.Spine, new SolidColorBrush(Color.FromRgb(169, 176, 155))},
            {Microsoft.Research.Kinect.Nui.JointID.ShoulderCenter, new SolidColorBrush(Color.FromRgb(168, 230, 29))},
            {Microsoft.Research.Kinect.Nui.JointID.Head, new SolidColorBrush(Color.FromRgb(200, 0,   0))},
            {Microsoft.Research.Kinect.Nui.JointID.ShoulderLeft, new SolidColorBrush(Color.FromRgb(79,  84,  33))},
            {Microsoft.Research.Kinect.Nui.JointID.ElbowLeft, new SolidColorBrush(Color.FromRgb(84,  33,  42))},
            {Microsoft.Research.Kinect.Nui.JointID.WristLeft, new SolidColorBrush(Color.FromRgb(255, 126, 0))},
            {Microsoft.Research.Kinect.Nui.JointID.HandLeft, new SolidColorBrush(Color.FromRgb(215,  86, 0))},
            {Microsoft.Research.Kinect.Nui.JointID.ShoulderRight, new SolidColorBrush(Color.FromRgb(33,  79,  84))},
            {Microsoft.Research.Kinect.Nui.JointID.ElbowRight, new SolidColorBrush(Color.FromRgb(33,  33,  84))},
            {Microsoft.Research.Kinect.Nui.JointID.WristRight, new SolidColorBrush(Color.FromRgb(77,  109, 243))},
            {Microsoft.Research.Kinect.Nui.JointID.HandRight, new SolidColorBrush(Color.FromRgb(37,   69, 243))},
            {Microsoft.Research.Kinect.Nui.JointID.HipLeft, new SolidColorBrush(Color.FromRgb(77,  109, 243))},
            {Microsoft.Research.Kinect.Nui.JointID.KneeLeft, new SolidColorBrush(Color.FromRgb(69,  33,  84))},
            {Microsoft.Research.Kinect.Nui.JointID.AnkleLeft, new SolidColorBrush(Color.FromRgb(229, 170, 122))},
            {Microsoft.Research.Kinect.Nui.JointID.FootLeft, new SolidColorBrush(Color.FromRgb(255, 126, 0))},
            {Microsoft.Research.Kinect.Nui.JointID.HipRight, new SolidColorBrush(Color.FromRgb(181, 165, 213))},
            {Microsoft.Research.Kinect.Nui.JointID.KneeRight, new SolidColorBrush(Color.FromRgb(71, 222,  76))},
            {Microsoft.Research.Kinect.Nui.JointID.AnkleRight, new SolidColorBrush(Color.FromRgb(245, 228, 156))},
            {Microsoft.Research.Kinect.Nui.JointID.FootRight, new SolidColorBrush(Color.FromRgb(77,  109, 243))}
        };

        private void Window_Loaded(object sender, EventArgs e)
        {
            nui = new Microsoft.Research.Kinect.Nui.Runtime();
            player = new VirtualKinect.Player();
            try
            {

                nui.Initialize(RuntimeOptions.UseDepthAndPlayerIndex | RuntimeOptions.UseSkeletalTracking | RuntimeOptions.UseColor);
            }
            catch (InvalidOperationException)
            {
                // System.Windows.MessageBox.Show("Runtime initialization failed. Please make sure Kinect device is plugged in.");
                //  return;
            }


            try
            {
                //player.VideoStream.Open(ImageStreamType.Video, 2, ImageResolution.Resolution640x480, ImageType.Color);
                //player.DepthStream.Open(ImageStreamType.Depth, 2, ImageResolution.Resolution320x240, ImageType.DepthAndPlayerIndex);
            }
            catch (InvalidOperationException)
            {
                System.Windows.MessageBox.Show("Failed to open stream. Please make sure to specify a supported image type and resolution.");
                return;
            }

            lastTime = DateTime.Now;

            player.DepthFrameReady += new EventHandler<VirtualKinect.ImageFrameReadyEventArgs>(nui_DepthFrameReady);
            player.SkeletonFrameReady += new EventHandler<VirtualKinect.SkeletonFrameReadyEventArgs>(nui_SkeletonFrameReady);
            player.VideoFrameReady += new EventHandler<VirtualKinect.ImageFrameReadyEventArgs>(nui_ColorFrameReady);
        }

        // Converts a 16-bit grayscale depth frame which includes player indexes into a 32-bit frame
        // that displays different players in different colors
        byte[] convertDepthFrame(byte[] depthFrame16)
        {
            for (int i16 = 0, i32 = 0; i16 < depthFrame16.Length && i32 < depthFrame32.Length; i16 += 2, i32 += 4)
            {
                int player = depthFrame16[i16] & 0x07;
                int realDepth = (depthFrame16[i16 + 1] << 5) | (depthFrame16[i16] >> 3);
                // transform 13-bit depth information into an 8-bit intensity appropriate
                // for display (we disregard information in most significant bit)
                byte intensity = (byte)(255 - (255 * realDepth / 0x0fff));

                depthFrame32[i32 + RED_IDX] = 0;
                depthFrame32[i32 + GREEN_IDX] = 0;
                depthFrame32[i32 + BLUE_IDX] = 0;

                // choose different display colors based on player
                switch (player)
                {
                    case 0:
                        depthFrame32[i32 + RED_IDX] = (byte)(intensity / 2);
                        depthFrame32[i32 + GREEN_IDX] = (byte)(intensity / 2);
                        depthFrame32[i32 + BLUE_IDX] = (byte)(intensity / 2);
                        break;
                    case 1:
                        depthFrame32[i32 + RED_IDX] = intensity;
                        break;
                    case 2:
                        depthFrame32[i32 + GREEN_IDX] = intensity;
                        break;
                    case 3:
                        depthFrame32[i32 + RED_IDX] = (byte)(intensity / 4);
                        depthFrame32[i32 + GREEN_IDX] = (byte)(intensity);
                        depthFrame32[i32 + BLUE_IDX] = (byte)(intensity);
                        break;
                    case 4:
                        depthFrame32[i32 + RED_IDX] = (byte)(intensity);
                        depthFrame32[i32 + GREEN_IDX] = (byte)(intensity);
                        depthFrame32[i32 + BLUE_IDX] = (byte)(intensity / 4);
                        break;
                    case 5:
                        depthFrame32[i32 + RED_IDX] = (byte)(intensity);
                        depthFrame32[i32 + GREEN_IDX] = (byte)(intensity / 4);
                        depthFrame32[i32 + BLUE_IDX] = (byte)(intensity);
                        break;
                    case 6:
                        depthFrame32[i32 + RED_IDX] = (byte)(intensity / 2);
                        depthFrame32[i32 + GREEN_IDX] = (byte)(intensity / 2);
                        depthFrame32[i32 + BLUE_IDX] = (byte)(intensity);
                        break;
                    case 7:
                        depthFrame32[i32 + RED_IDX] = (byte)(255 - intensity);
                        depthFrame32[i32 + GREEN_IDX] = (byte)(255 - intensity);
                        depthFrame32[i32 + BLUE_IDX] = (byte)(255 - intensity);
                        break;
                }
            }
            return depthFrame32;
        }

        void nui_DepthFrameReady(object sender, VirtualKinect.ImageFrameReadyEventArgs e)
        {
            VirtualKinect.PlanarImage Image = e.ImageFrame.Image;
            byte[] convertedDepthFrame = convertDepthFrame(Image.Bits);

            depth.Source = BitmapSource.Create(
                Image.Width, Image.Height, 96, 96, PixelFormats.Bgr32, null, convertedDepthFrame, Image.Width * 4);

        }

        private Point getDisplayPosition(VirtualKinect.Joint joint)
        {
            float depthX, depthY;
            Microsoft.Research.Kinect.Nui.Vector Position = joint.Position.NUI;
            nui.SkeletonEngine.SkeletonToDepthImage(Position, out depthX, out depthY);
            depthX = depthX * 320; //convert to 320, 240 space
            depthY = depthY * 240; //convert to 320, 240 space
            int colorX, colorY;
            Microsoft.Research.Kinect.Nui.ImageViewArea iv = new Microsoft.Research.Kinect.Nui.ImageViewArea();
            // only ImageResolution.Resolution640x480 is supported at this point
            nui.NuiCamera.GetColorPixelCoordinatesFromDepthPixel(Microsoft.Research.Kinect.Nui.ImageResolution.Resolution640x480, iv, (int)depthX, (int)depthY, (short)0, out colorX, out colorY);

            // map back to skeleton.Width & skeleton.Height
            return new Point((int)(skeleton.Width * colorX / 640.0), (int)(skeleton.Height * colorY / 480));
        }

        Polyline getBodySegment(VirtualKinect.JointsCollection joints, Brush brush, params Microsoft.Research.Kinect.Nui.JointID[] ids)
        {
            PointCollection points = new PointCollection(ids.Length);
            for (int i = 0; i < ids.Length; ++i)
            {
                points.Add(getDisplayPosition(joints[ids[i]]));
            }

            Polyline polyline = new Polyline();
            polyline.Points = points;
            polyline.Stroke = brush;
            polyline.StrokeThickness = 5;
            return polyline;
        }

        void nui_SkeletonFrameReady(object sender, VirtualKinect.SkeletonFrameReadyEventArgs e)
        {
            //For VirtualKinect Recording

            VirtualKinect.SkeletonFrame skeletonFrame = e.SkeletonFrame;
            int iSkeleton = 0;
            Brush[] brushes = new Brush[6];
            brushes[0] = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            brushes[1] = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            brushes[2] = new SolidColorBrush(Color.FromRgb(64, 255, 255));
            brushes[3] = new SolidColorBrush(Color.FromRgb(255, 255, 64));
            brushes[4] = new SolidColorBrush(Color.FromRgb(255, 64, 255));
            brushes[5] = new SolidColorBrush(Color.FromRgb(128, 128, 255));

            skeleton.Children.Clear();



            foreach (VirtualKinect.SkeletonData data in skeletonFrame.Skeletons)
            {
                if (SkeletonTrackingState.Tracked == data.TrackingState)
                {
                    // Draw bones
                    Brush brush = brushes[iSkeleton % brushes.Length];
                    skeleton.Children.Add(getBodySegment(data.Joints, brush, JointID.HipCenter, JointID.Spine, JointID.ShoulderCenter, JointID.Head));
                    skeleton.Children.Add(getBodySegment(data.Joints, brush, JointID.ShoulderCenter, JointID.ShoulderLeft, JointID.ElbowLeft, JointID.WristLeft, JointID.HandLeft));
                    skeleton.Children.Add(getBodySegment(data.Joints, brush, JointID.ShoulderCenter, JointID.ShoulderRight, JointID.ElbowRight, JointID.WristRight, JointID.HandRight));
                    skeleton.Children.Add(getBodySegment(data.Joints, brush, JointID.HipCenter, JointID.HipLeft, JointID.KneeLeft, JointID.AnkleLeft, JointID.FootLeft));
                    skeleton.Children.Add(getBodySegment(data.Joints, brush, JointID.HipCenter, JointID.HipRight, JointID.KneeRight, JointID.AnkleRight, JointID.FootRight));

                    // Draw joints
                    foreach (VirtualKinect.Joint joint in data.Joints)
                    {
                        Point jointPos = getDisplayPosition(joint);
                        Line jointLine = new Line();
                        jointLine.X1 = jointPos.X - 3;
                        jointLine.X2 = jointLine.X1 + 6;
                        jointLine.Y1 = jointLine.Y2 = jointPos.Y;
                        jointLine.Stroke = jointColors[joint.ID];
                        jointLine.StrokeThickness = 6;
                        skeleton.Children.Add(jointLine);
                    }
                }
                iSkeleton++;
            } // for each skeleton
        }

        void nui_ColorFrameReady(object sender, VirtualKinect.ImageFrameReadyEventArgs e)
        {
            VirtualKinect.PlanarImage Image = e.ImageFrame.Image;

            video.Source = BitmapSource.Create(
                Image.Width, Image.Height, 96, 96, PixelFormats.Bgr32, null, Image.Bits, Image.Width * Image.BytesPerPixel);

        }



        private void Window_Closed(object sender, EventArgs e)
        {
            player.Uninitialize();
            Environment.Exit(0);
        }

        private void Button_openFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = "VirtualKinect files(*.vkd)|*.vkd|All files(*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                playingFileName.Text = ofd.FileName;
            }
            loadContents();
        }

        private void Button_play_Click(object sender, RoutedEventArgs e)
        {
            if (!player.fileLoaded)
                return;
            if (reachEndOfSequence)
                player.resetPlaying();
            if (!playingSequenceNow)
                startSequencePlayingTimer();
            else
                stopSequencePlayingTimer();
        }

        private void startSequencePlayingTimer()
        {
            if (!playingSequenceNow)
                player.resetPlaying();

            stopwatch.Reset();
            stopwatch.Start();
            dispatcherTimer.Start();
            Button_Status.Content = "Stop";
            playingSequenceNow = true;
        }
        private void stopSequencePlayingTimer()
        {

            dispatcherTimer.Stop();
            stopwatch.Stop();
            Button_Status.Content = "Play";
            playingSequenceNow = false;

        }

        private bool loadContents()
        {
            String fileName = playingFileName.Text;
            if (!System.IO.File.Exists(fileName))
            {
                playingStatus.Text = "File does not exist";

                return false;
            }
            player.load(playingFileName.Text);
            playingStatus.Text = fileName;
            TimeSpan ts = new TimeSpan(0, 0, 0, 0, (int)player.duration);
            TimeSpan tsShow = new TimeSpan(ts.Hours, ts.Minutes, ts.Seconds);
            SeaquenceDurationTime.Content = ts.ToString();
            return true;

        }
        void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            SeaquenceTime.Content = DateTime.Now.ToLongTimeString();
            long currentPlayingSequenceTime = stopwatch.ElapsedMilliseconds;

            if (player.duration < currentPlayingSequenceTime)
                stopSequencePlayingTimer();

            player.executeEvents(currentPlayingSequenceTime);
            TimeSpan span = new TimeSpan(0, 0, 0, 0, (int)currentPlayingSequenceTime);
            TimeSpan tsToShow = new TimeSpan(span.Hours, span.Minutes, span.Seconds);
            SeaquenceTime.Content = tsToShow.ToString();
           
       

        }

        private void SeaquencePosition_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }
}
