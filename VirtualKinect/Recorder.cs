using System;
using System.IO;
using System.Collections;
using System.Diagnostics;

namespace VirtualKinect
{
    public class Recorder
    {
        private const String saveFileDirectory = "data";
        private const String _eventDataFolder = "_KinectEventData";
        public String eventDataFolder
        {
            get
            {
                return Path.Combine(saveFileDirectory, date.ToString(KinectEventData.dateFormatStyle) + _eventDataFolder);
            }

        }
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
            makeSaveDir();
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
        private void makeSaveDir()
        {
            mkDir(saveFileDirectory);
            mkDir(this.eventDataFolder);
        }
        private static void mkDir(String dir)
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

        }

        private void saveData()
        {
            DepthFrameEventData[] dfe = (DepthFrameEventData[])depthFrameEvents.ToArray(typeof(DepthFrameEventData));
            ImageFrameEventData[] ife = (ImageFrameEventData[])imageFrameEvents.ToArray(typeof(ImageFrameEventData));
            SkeletonFrameEventData[] sfe = (SkeletonFrameEventData[])skeletonFrameEvents.ToArray(typeof(SkeletonFrameEventData));

            KinectEventData ked = new KinectEventData();

            ked.set(date, this.duration, dfe, ife, sfe);
            String fileName = date.ToString(KinectEventData.dateFormatStyle) + KinectEventData.extension;
            String relativefileName = Path.Combine(eventDataFolder, fileName);

            makeSaveDir();
            IO.saveXML(ked, relativefileName);
            //IO.save(ked, relativefileName);

        }

        public void addImageFrameEvent(Microsoft.Research.Kinect.Nui.ImageFrameReadyEventArgs e)
        {
            if (!recording)
                return;
            imageFrameEvents.Add(new ImageFrameEventData(e, stopwatch.ElapsedMilliseconds, eventDataFolder));
        }
        public void addSkeletonFrameEvent(Microsoft.Research.Kinect.Nui.SkeletonFrameReadyEventArgs e)
        {
            if (!recording)
                return;
            skeletonFrameEvents.Add(new SkeletonFrameEventData(e, stopwatch.ElapsedMilliseconds));
        }
        public void addDepthFrameEvent(Microsoft.Research.Kinect.Nui.ImageFrameReadyEventArgs e)
        {
            if (!recording)
                return;
            depthFrameEvents.Add(new DepthFrameEventData(e, stopwatch.ElapsedMilliseconds, eventDataFolder));
        }
    }

}
