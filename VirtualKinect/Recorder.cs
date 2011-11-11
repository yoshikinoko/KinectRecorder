using System;
using System.IO;
using System.Collections;
using System.Diagnostics;

namespace VirtualKinect
{
    public class Recorder
    {
        private const String saveFileDirectory = "data";
        private ArrayList depthFrameEvents;
        private ArrayList imageFrameEvents;
        private ArrayList skeletonFrameEvents;
        private DateTime date;
        private long duration;
        private Stopwatch stopwatch;
        private bool recording;

        public Recorder()
        {
            recording = false;
        }

        public void startRecording()
        {
            if (recording)
                return;

            depthFrameEvents = new ArrayList();
            imageFrameEvents = new ArrayList();
            skeletonFrameEvents = new ArrayList();
            date = DateTime.Now;
            stopwatch = new Stopwatch();
            stopwatch.Start();
            recording = true;
        }
        public void stopRecording()
        {
            if (!recording)
                return;

            recording = false;
            this.duration = stopwatch.ElapsedMilliseconds;
            stopwatch.Stop();
            saveData();
        }
        private static void makeSaveDir()
        {
            if (!Directory.Exists(saveFileDirectory))
                Directory.CreateDirectory(saveFileDirectory);

        }
        private void saveData()
        {
            DepthFrameEventData[] dfe = (DepthFrameEventData[])depthFrameEvents.ToArray(typeof(DepthFrameEventData));
            ImageFrameEventData[] ife = (ImageFrameEventData[])imageFrameEvents.ToArray(typeof(ImageFrameEventData));
            SkeletonFrameEventData[] sfe = (SkeletonFrameEventData[])skeletonFrameEvents.ToArray(typeof(SkeletonFrameEventData));

            KinectEventData ked = new KinectEventData();

            ked.set(date, this.duration, dfe, ife, sfe);
            String fileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + KinectEventData.extension;
            String relativefileName = Path.Combine(saveFileDirectory, fileName);

            makeSaveDir();
            //IO.saveXML(ked, relativefileName);
            IO.save(ked, relativefileName);

        }

        public void addImageFrameEvent(Microsoft.Research.Kinect.Nui.ImageFrameReadyEventArgs e)
        {
            if (!recording)
                return;
            imageFrameEvents.Add(new ImageFrameEventData(e, stopwatch.ElapsedMilliseconds));
        }
        public void addSkeletonFrameEvent(Microsoft.Research.Kinect.Nui.SkeletonFrameReadyEventArgs e)
        {
            if (!recording)
                return;
            foreach (Microsoft.Research.Kinect.Nui.SkeletonData data in e.SkeletonFrame.Skeletons)
            {
                if (Microsoft.Research.Kinect.Nui.SkeletonTrackingState.Tracked == data.TrackingState)
                {
                    Console.WriteLine("goodskeleton:" + skeletonFrameEvents.Count);
                }
            }


            skeletonFrameEvents.Add(new SkeletonFrameEventData(e, stopwatch.ElapsedMilliseconds));
        }
        public void addDepthFrameEvent(Microsoft.Research.Kinect.Nui.ImageFrameReadyEventArgs e)
        {
            if (!recording)
                return;
            depthFrameEvents.Add(new DepthFrameEventData(e, stopwatch.ElapsedMilliseconds));
        }
    }

}
