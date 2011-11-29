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


        private DateTime date;


        private long duration;
        private Stopwatch stopwatch;
        private bool _recording;
        public bool recording
        {
            get { return _recording; }
        }
        public Recorder()
        {
            _recording = false;
        }

        public void startRecording()
        {
            if (_recording)
                return;

            depthFrameEvents = new ArrayList();
            imageFrameEvents = new ArrayList();
            skeletonFrameEvents = new ArrayList();

            date = DateTime.Now;
            makeSaveDir();
            stopwatch = new Stopwatch();
            stopwatch.Start();
            _recording = true;
        }
        public void stopRecording()
        {
            if (!_recording)
                return;

            _recording = false;
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
        public string recordingTime
        {
            get
            {
                string recordTimeStr = "";
                long currentRecordingSequenceTime = stopwatch.ElapsedMilliseconds;
                TimeSpan span = new TimeSpan(0, 0, 0, 0, (int)currentRecordingSequenceTime);
                TimeSpan tsToShow = new TimeSpan(span.Hours, span.Minutes, span.Seconds);
                recordTimeStr = tsToShow.ToString();
                return recordTimeStr;

            }
        }

        public int recordMinutes
        {

            get
            {
                //  string recordTimeStr = "";
                long currentRecordingSequenceTime = stopwatch.ElapsedMilliseconds;
                TimeSpan span = new TimeSpan(0, 0, 0, 0, (int)currentRecordingSequenceTime);
                TimeSpan tsToShow = new TimeSpan(span.Hours, span.Minutes, span.Seconds);
                //recordTimeStr = tsToShow.ToString();
                return (int)span.Minutes;

            }

        }
        public long recordTimeElapsedMilliseconds
        {
            get { return stopwatch.ElapsedMilliseconds; }
        }


        private int checkTotalEventSize()
        {
            return depthFrameEvents.Count + imageFrameEvents.Count + skeletonFrameEvents.Count;
        }

        private KinectEventData makeKinectEventData()
        {


            ArrayList saveDFE = depthFrameEvents;
            ArrayList saveIFE = imageFrameEvents;
            ArrayList saveSFE = skeletonFrameEvents;

            depthFrameEvents = new ArrayList();
            imageFrameEvents = new ArrayList();
            skeletonFrameEvents = new ArrayList();



            string[] dfe = (string[])saveDFE.ToArray(typeof(string));
            string[] ife = (string[])saveIFE.ToArray(typeof(string));
            string[] sfe = (string[])saveSFE.ToArray(typeof(string));

            KinectEventData ked = new KinectEventData();

            ked.set(device_id, date, this.duration,sfe,ife,dfe);

            return ked;
        }



        private void saveData()
        {
            KinectEventData ked = makeKinectEventData();
            String fileName = date.ToString(KinectEventData.dateFormatStyle) + KinectEventData.extension;
            String relativefileName = Path.Combine(eventDataFolder, fileName);

            makeSaveDir();
            IO.saveXML(ked, relativefileName);
            //IO.save(ked, relativefileName);

        }

        public void addImageFrameEvent(Microsoft.Research.Kinect.Nui.ImageFrameReadyEventArgs e)
        {
            if (!_recording)
                return;
            ImageFrameEventData ife = new ImageFrameEventData(e, stopwatch.ElapsedMilliseconds, eventDataFolder, device_id);
            imageFrameEvents.Add(ife.saveFileName);
        }
        public void addSkeletonFrameEvent(Microsoft.Research.Kinect.Nui.SkeletonFrameReadyEventArgs e)
        {
            if (!_recording)
                return;
            SkeletonFrameEventData sfe = new SkeletonFrameEventData(e, stopwatch.ElapsedMilliseconds, eventDataFolder, device_id);

            skeletonFrameEvents.Add(sfe.saveFileName);
        }
        public void addDepthFrameEvent(Microsoft.Research.Kinect.Nui.ImageFrameReadyEventArgs e)
        {
            if (!_recording)
                return;
            DepthFrameEventData dfe = new DepthFrameEventData(e, stopwatch.ElapsedMilliseconds, eventDataFolder, device_id);
            depthFrameEvents.Add(dfe.saveFileName);
            
        }
    }

}
