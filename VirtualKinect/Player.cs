using System;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace VirtualKinect
{
    public class Player
    {
        private KinectEventData ked;
        private bool _fileLoaded = false;
        private KinectEventLineData kinectEventLine;
        private object nextKinectEvent;
        private string eventRootFolder;
        public bool fileLoaded
        {
            get { return _fileLoaded; }
            set { }
        }
        public void load(String fileName)
        {
            eventRootFolder = Path.Combine(System.IO.Path.GetDirectoryName(fileName), Recorder._eventDataDirectory);


            ked = IO.loadXML(fileName);
            loadIndexEvent();
            _fileLoaded = true;
        }
        private readonly object lockObject = new object();


        public bool stepPlay()
        {

            if (kinectEventLine.isEnd)
                return false;

            nextKinectEvent = kinectEventLine.loadKinectEvent(eventRootFolder);

            executeKinectEvent();

            loadNextEvent();

            return true;

        }
        private void loadNextEvent()
        {
            KinectEventLineData nextEvent = kinectEventLine.loadNextEvent(eventRootFolder);
            kinectEventLine = nextEvent;
        }
        private void loadPreviousEvent()
        {
            KinectEventLineData nextEvent = kinectEventLine.loadPreviousEvent(eventRootFolder);
            kinectEventLine = nextEvent;
        }

        private void executeKinectEvent()
        {
            switch (kinectEventLine.kinectEventType)
            {
                case EventType.DepthFrameEvent:
                    DepthFrameEventData dfe = (DepthFrameEventData)nextKinectEvent;
                    executeDepthFrameEvent(dfe);
                    break;
                case EventType.ImageFrameEvent:
                    ImageFrameEventData ife = (ImageFrameEventData)nextKinectEvent;

                    executeImageEvent(ife);
                    break;
                case EventType.SkeletonFrameEvent:
                    SkeletonFrameEventData sfe = (SkeletonFrameEventData)nextKinectEvent;
                    executeSkeletonFrameEvent(sfe);
                    break;
            }
        }

        public void resetPlaying()
        {
            loadIndexEvent();
        }


        public long duration
        {
            get
            {
                if (fileLoaded)
                    return ked.duration;
                else
                    return 0;
            }
        }

        public void executeEvents(long currentTime)
        {

            if (currentTime > duration)
                return;
            while (kinectEventLine.time < currentTime)
            {

                if (!stepPlay())
                    break;
                if (kinectEventLine.isEnd)
                    break;
            }
        }
        public void setPlayerStatusByRatio(double ratio)
        {
            long time = (long)((double)duration * ratio);
            setPlayerStatus(time);
        }



        public void setPlayerStatus(long currentTime)
        {
            if (currentTime > duration)
                return;
            if (currentTime < 0)
                return;
            if (currentTime == 0)
            {
                loadIndexEvent();
            }
            else {
                searchNearestEvent(currentTime);
            }

            KinectEventLineData currentEventLine = kinectEventLine;
            executePreviousEvent();
            kinectEventLine = currentEventLine;
            return;

        }

        private void loadKinectEventBySequenceIndex(int index)
        {
            kinectEventLine = ked.loadEventBySequenceNumber(eventRootFolder, index);
        }

        private void searchNearestEvent(long currentTime)
        {
            loadIndexEvent();

            int nearIndex = (int)((double)this.ked.totalEvents * ((double)currentTime / (double)this.ked.duration));

            loadKinectEventBySequenceIndex(nearIndex);

            if (kinectEventLine.time < currentTime)
            {
                while (kinectEventLine.time > currentTime)
                {
                    loadNextEvent();
                }

            }
            else
            {
                while (kinectEventLine.time < currentTime)
                {
                    loadPreviousEvent();
                }
            }

        }
        private void executePreviousEvent()
        {
            bool findSkeleton = false;
            bool findDepth = false;
            bool findImage = false;
            SkeletonFrameEventData sfe = new SkeletonFrameEventData();
            DepthFrameEventData dfe = new DepthFrameEventData();
            ImageFrameEventData ife = new ImageFrameEventData();
            while (!findSkeleton || !findImage || !findDepth)
            {
                switch (kinectEventLine.kinectEventType)
                {
                    case EventType.SkeletonFrameEvent:
                        if (!findSkeleton)
                        {
                            findSkeleton = true;
                            sfe = (SkeletonFrameEventData)kinectEventLine.loadKinectEvent(eventRootFolder);
                        }
                        break;
                    case EventType.DepthFrameEvent:
                        if (!findDepth)
                        {
                            findDepth = true;
                            dfe = (DepthFrameEventData)kinectEventLine.loadKinectEvent(eventRootFolder);
                        }
                        break;
                    case EventType.ImageFrameEvent:
                        if (!findImage)
                        {
                            findImage = true;
                            ife = (ImageFrameEventData)kinectEventLine.loadKinectEvent(eventRootFolder);
                        }
                        break;
                    default:
                        break;

                }
                if (kinectEventLine.sequenceNumber == 1)
                {
                    break;
                }
                loadPreviousEvent();
            }
            if (findSkeleton)
                executeSkeletonFrameEvent(sfe);
            if (findDepth)
                executeDepthFrameEvent(dfe);
            if (findImage)
                executeImageEvent(ife);

        }



        private void loadIndexEvent()
        {
            kinectEventLine = ked.loadIndexEvent(eventRootFolder);
        }

        public event EventHandler<ImageFrameReadyEventArgs> DepthFrameReady;
        protected virtual void executeDepthFrameEvent(DepthFrameEventData dfe)
        {
            ImageFrameReadyEventArgs e = new ImageFrameReadyEventArgs();
            e.ImageFrame = dfe.imageFrame;
            e.eventFileName = dfe.saveFileName;
            DepthFrameReady(this, e);
        }

        public event EventHandler<SkeletonFrameReadyEventArgs> SkeletonFrameReady;
        protected virtual void executeSkeletonFrameEvent(SkeletonFrameEventData sfe)
        {
            SkeletonFrameReadyEventArgs e = new SkeletonFrameReadyEventArgs();
            e.SkeletonFrame = sfe.SkeletonFrame;
            e.eventFileName = sfe.saveFileName;
            SkeletonFrameReady(this, e);
        }
        public event EventHandler<ImageFrameReadyEventArgs> VideoFrameReady;
        protected virtual void executeImageEvent(ImageFrameEventData ife)
        {
            ImageFrameReadyEventArgs e = new ImageFrameReadyEventArgs();
            e.ImageFrame = ife.imageFrame;
            e.eventFileName = ife.saveFileName;
            VideoFrameReady(this, e);
        }

        public void Uninitialize() { }

    }


}