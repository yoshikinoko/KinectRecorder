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
        private const string device_id = "kinect1";
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
        private ArrayList depthFrameEventFileNames;
        private ArrayList imageFrameEventFileNames;
        private ArrayList skeletonFrameEventFileNames;


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
            depthFrameEventFileNames = new ArrayList();
            imageFrameEventFileNames = new ArrayList();
            skeletonFrameEventFileNames = new ArrayList();



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
            string[] dfefns = (string[])depthFrameEventFileNames.ToArray(typeof(string));
            string[] ifefns = (string[])imageFrameEventFileNames.ToArray(typeof(string));
            string[] sfefns = (string[])skeletonFrameEventFileNames.ToArray(typeof(string));



            KinectEventData ked = new KinectEventData();

            ked.set(device_id, date, this.duration, dfe, ife, sfe);
            ked.setFileNames(date, this.duration, dfefns, ifefns, sfefns);
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
            ImageFrameEventData ife = new ImageFrameEventData(e, stopwatch.ElapsedMilliseconds, eventDataFolder,device_id);
            imageFrameEvents.Add(ife);
            imageFrameEventFileNames.Add(ife.eventFileName);
        }
        public void addSkeletonFrameEvent(Microsoft.Research.Kinect.Nui.SkeletonFrameReadyEventArgs e)
        {
            if (!recording)
                return;
            SkeletonFrameEventData sfe = new SkeletonFrameEventData(e, stopwatch.ElapsedMilliseconds, eventDataFolder,device_id);

            skeletonFrameEvents.Add(sfe);
            skeletonFrameEventFileNames.Add(sfe.eventFileName);
        }
        public void addDepthFrameEvent(Microsoft.Research.Kinect.Nui.ImageFrameReadyEventArgs e)
        {
            if (!recording)
                return;
            DepthFrameEventData dfe = new DepthFrameEventData(e, stopwatch.ElapsedMilliseconds, eventDataFolder,device_id);
            depthFrameEvents.Add(dfe);
            depthFrameEventFileNames.Add(dfe.eventFileName);
        }
    }

}
