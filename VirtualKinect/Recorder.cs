using System;
using System.IO;
using System.Collections;
using System.Diagnostics;

namespace VirtualKinect
{
    public class Recorder
    {
        private KinectEventLineData lastEvent;

        public  const String saveFileDirectory = "data";
        public  const String _eventDataDirectory = "events";
        private const String _eventDataFolder = "_KinectEventData";
        private const string device_id = "kinect1";

        public String eventDataFolder
        {
            get
            {
                return Path.Combine(recordDirecotory, _eventDataDirectory);
            }
        }

        public String recordDirecotory
        {
            get
            {
                return Path.Combine(saveFileDirectory, date.ToString(KinectEventData.dateFormatStyle) + _eventDataFolder);
            }
        }


        private int sequenceNumber;
        private DateTime date;
        private bool isStartRecording = false;
        private string kinectEventIndexFileName;
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
            mkDir(recordDirecotory);

            mkDir(eventDataFolder);
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

        public long recordTimeElapsedMilliseconds
        {
            get { return stopwatch.ElapsedMilliseconds; }
        }

        private void saveData()
        {
            KinectEventData ked = new KinectEventData();
            ked.set(device_id, date, duration, sequenceNumber, kinectEventIndexFileName);
            String fileName = date.ToString(KinectEventData.dateFormatStyle) + KinectEventData.extension;
            String relativefileName = Path.Combine(recordDirecotory, fileName);
            makeSaveDir();
            IO.saveXML(ked, relativefileName);
        }

        public void addImageFrameEvent(Microsoft.Research.Kinect.Nui.ImageFrameReadyEventArgs e)
        {
            if (!_recording)
                return;
            ImageFrameEventData ife = new ImageFrameEventData(e, stopwatch.ElapsedMilliseconds, eventDataFolder, device_id);
            saveNextEvent(ife.time, ife.saveFileName, EventType.ImageFrameEvent);
        }

        public void addSkeletonFrameEvent(Microsoft.Research.Kinect.Nui.SkeletonFrameReadyEventArgs e)
        {
            if (!_recording)
                return;
            SkeletonFrameEventData sfe = new SkeletonFrameEventData(e, stopwatch.ElapsedMilliseconds, eventDataFolder, device_id);
            saveNextEvent(sfe.time, sfe.saveFileName, EventType.SkeletonFrameEvent);
        }

        public void addDepthFrameEvent(Microsoft.Research.Kinect.Nui.ImageFrameReadyEventArgs e)
        {
            if (!_recording)
                return;
            DepthFrameEventData dfe = new DepthFrameEventData(e, stopwatch.ElapsedMilliseconds, eventDataFolder, device_id);
            saveNextEvent(dfe.time, dfe.saveFileName, EventType.DepthFrameEvent);

        }

        private void saveNextEvent(long time, string kinectEventFileName, EventType eventType)
        {
            if (!isStartRecording)
            {
                isStartRecording = true;
                lastEvent = new KinectEventLineData();
                lastEvent.set(1, "", time, kinectEventFileName, eventType);
                kinectEventIndexFileName = lastEvent.saveFileName;
                sequenceNumber = 2;
            }


            KinectEventLineData nextEvent = new KinectEventLineData();
            nextEvent.set(sequenceNumber, lastEvent.saveFileName, time, kinectEventFileName, eventType);
            lastEvent.nextFileName = nextEvent.saveFileName;
            sequenceNumber++;

            lastEvent.save(eventDataFolder);


            lastEvent = nextEvent;
        }

        private void saveFinilizedEvent()
        {
            lastEvent.finish();
            lastEvent.save(eventDataFolder);
        }

    }

}
