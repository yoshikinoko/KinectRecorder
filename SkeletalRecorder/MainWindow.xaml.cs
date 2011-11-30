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

using System.Windows.Forms;


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
        SkeletalCore.Draw drawCore;
        Runtime nui;

        //For Virtual Kinect Recording and playing
        VirtualKinect.Recorder recorder;


        // We want to control how depth data gets converted into false-color data
        // for more intuitive visualization, so we keep 32-bit color frame buffer versions of
        // these, to be updated whenever we receive and process a 16-bit frame.
        const int RED_IDX = 2;
        const int GREEN_IDX = 1;
        const int BLUE_IDX = 0;
        byte[] depthFrame32 = new byte[320 * 240 * 4];

        private int record_min = 240;


        private void Window_Loaded(object sender, EventArgs e)
        {
            drawCore = new SkeletalCore.Draw();
            nui = new Runtime();
            recorder = new VirtualKinect.Recorder();

            try
            {
                nui.Initialize(RuntimeOptions.UseDepthAndPlayerIndex | RuntimeOptions.UseSkeletalTracking | RuntimeOptions.UseColor);
            }
            catch (InvalidOperationException)
            {
                System.Windows.MessageBox.Show("Runtime initialization failed. Please make sure Kinect device is plugged in.");
                return;
            }


            try
            {
                nui.VideoStream.Open(ImageStreamType.Video, 2, ImageResolution.Resolution640x480, ImageType.Color);
                nui.DepthStream.Open(ImageStreamType.Depth, 2, ImageResolution.Resolution320x240, ImageType.DepthAndPlayerIndex);
            }
            catch (InvalidOperationException)
            {
                System.Windows.MessageBox.Show("Failed to open stream. Please make sure to specify a supported image type and resolution.");
                return;
            }



            nui.DepthFrameReady += new EventHandler<ImageFrameReadyEventArgs>(nui_DepthFrameReady);
            nui.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(nui_SkeletonFrameReady);
            nui.VideoFrameReady += new EventHandler<ImageFrameReadyEventArgs>(nui_ColorFrameReady);
            nui.NuiCamera.ElevationAngle = 0;
        }
        void nui_DepthFrameReady(object sender, ImageFrameReadyEventArgs e)
        {
            //For VirtualKinect Recording
            recorder.addDepthFrameEvent(e);
            updateRecordingTime();
            PlanarImage image = e.ImageFrame.Image;
            drawCore.nui_DepthFrameReady(image.Bits, image.Width, image.Height, depth);

        }
        void nui_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            //For VirtualKinect Recording
            recorder.addSkeletonFrameEvent(e);
            updateRecordingTime();

            drawCore.nui_SkeletonFrameReady(e.SkeletonFrame, skeleton, nui);
            drawCore.drawFPS(frameRate);
        }

        void nui_ColorFrameReady(object sender, ImageFrameReadyEventArgs e)
        {
            //For VirtualKinect Recording
            recorder.addImageFrameEvent(e);
            updateRecordingTime();
            PlanarImage image = e.ImageFrame.Image;

            // 32-bit per pixel, RGBA image
            drawCore.nui_ColorFrameReady(image.Bits, image.Width, image.Height, image.BytesPerPixel, video);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            nui.Uninitialize();
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
                //   playingFileName.Text = ofd.FileName;
            }
        }

        private void Button_record_Click(object sender, RoutedEventArgs e)
        {
            recorder.startRecording();
            playingStatus.Text = "Recording";
        }

        private void Button_record_stop_Click(object sender, RoutedEventArgs e)
        {
            playingStatus.Text = "Saving";
            recorder.stopRecording();
            playingStatus.Text = "REC:STOP";

        }

        private void Button_loadFile_Click(object sender, RoutedEventArgs e)
        {
        }


        private void updateRecordingTime()
        {
            if (recorder.recording)
            {
                SeaquenceTime.Content = recorder.recordingTime;
                long recordMS = record_min * 60 * 1000;

                if (recorder.recordTimeElapsedMilliseconds > recordMS)
                {
                    recorder.stopRecording();
                    playingStatus.Text = "REC:STOP";

                }

            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int val = 240;
            val = (int)(e.NewValue);
            record_min = val;
            recordtimerdurationLabel.Content = val.ToString();
        }

    }
}
