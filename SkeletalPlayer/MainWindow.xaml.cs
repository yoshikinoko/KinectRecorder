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

        }
        Microsoft.Research.Kinect.Nui.Runtime nui;

        SkeletalCore.Draw drawCore;

        //For Virtual Kinect Recording and playing
        //  VirtualKinect.Recorder recorder;
        VirtualKinect.Player player;
        private void Window_Loaded(object sender, EventArgs e)
        {
            nui = new Microsoft.Research.Kinect.Nui.Runtime();
            drawCore = new SkeletalCore.Draw();
            player = new VirtualKinect.Player();
            try
            {

                nui.Initialize(RuntimeOptions.UseDepthAndPlayerIndex | RuntimeOptions.UseSkeletalTracking | RuntimeOptions.UseColor);
            }
            catch (InvalidOperationException)
            {
            }

            player.DepthFrameReady += new EventHandler<VirtualKinect.ImageFrameReadyEventArgs>(nui_DepthFrameReady);
            player.SkeletonFrameReady += new EventHandler<VirtualKinect.SkeletonFrameReadyEventArgs>(nui_SkeletonFrameReady);
            player.VideoFrameReady += new EventHandler<VirtualKinect.ImageFrameReadyEventArgs>(nui_ColorFrameReady);
        }

        void nui_DepthFrameReady(object sender, VirtualKinect.ImageFrameReadyEventArgs e)
        {

            VirtualKinect.PlanarImage image = e.ImageFrame.Image;
            drawCore.nui_DepthFrameReady(image.Bits, image.Width, image.Height, depth);


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



        void nui_SkeletonFrameReady(object sender, VirtualKinect.SkeletonFrameReadyEventArgs e)
        {
            drawCore.nui_SkeletonFrameReadyV(e.SkeletonFrame, skeleton, nui);
        }

        void nui_ColorFrameReady(object sender, VirtualKinect.ImageFrameReadyEventArgs e)
        {
            VirtualKinect.PlanarImage image = e.ImageFrame.Image;
            drawCore.nui_ColorFrameReady(image.Bits, image.Width, image.Height, image.BytesPerPixel, video);
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
            double currentVal = sequencer.Value;

            player.setPlayerStatusByRatio(currentVal);
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

        private void SeaquencePosition_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void stepPlay_Click(object sender, RoutedEventArgs e)
        {
            player.stepPlay();
        }

        private void sequencer_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double currentVal = e.NewValue;
            if (player.fileLoaded)
            {
                int currentPlayingSequenceTime = (int)((double)(player.duration) * currentVal);

                TimeSpan span = new TimeSpan(0, 0, 0, 0, (int)currentPlayingSequenceTime);
                TimeSpan tsToShow = new TimeSpan(span.Hours, span.Minutes, span.Seconds);
                SeaquenceTime.Content = tsToShow.ToString();
            }

        }

    }
}
