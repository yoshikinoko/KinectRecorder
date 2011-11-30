using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace VirtualKinect
{
    public class Player
    {
        private KinectEventData ked;
        private EventTiming[] timeline;
        private int eventIndex;
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
            eventRootFolder = System.IO.Path.GetDirectoryName(fileName);
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
//            nextKinectEvent = kinectEventLine.loadKinectEvent(eventRootFolder);

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
            eventIndex = 0;
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
            loadIndexEvent();

            while (kinectEventLine.time < currentTime)
            {

                loadNextEvent();
            }
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


            return;

        }

        private void loadIndexEvent()
        {
            kinectEventLine = ked.loadIndexEvent(eventRootFolder);

        }


        public int InstanceIndex = 0;

        public event EventHandler<ImageFrameReadyEventArgs> DepthFrameReady;
        //public event EventHandler DepthFrameReady;
        protected virtual void executeDepthFrameEvent(DepthFrameEventData dfe)
        {
            ImageFrameReadyEventArgs e = new ImageFrameReadyEventArgs();
            e.ImageFrame = dfe.imageFrame;
            DepthFrameReady(this, e);
        }
        public event EventHandler<SkeletonFrameReadyEventArgs> SkeletonFrameReady;
        //public event EventHandler SkeletonFrameReady;
        protected virtual void executeSkeletonFrameEvent(SkeletonFrameEventData sfe)
        {
            SkeletonFrameReadyEventArgs e = new SkeletonFrameReadyEventArgs();
            e.SkeletonFrame = sfe.SkeletonFrame;
            SkeletonFrameReady(this, e);
        }
        public event EventHandler<ImageFrameReadyEventArgs> VideoFrameReady;
        //public event EventHandler VideoFrameReady;
        protected virtual void executeImageEvent(ImageFrameEventData ife)
        {
            ImageFrameReadyEventArgs e = new ImageFrameReadyEventArgs();
            ImageFrame ei = new ImageFrame();
            ei = ife.imageFrame;
            e.ImageFrame = ei;
            VideoFrameReady(this, e);
        }

        //public void Initialize(RuntimeOptions options) { }
        public void Uninitialize() { }

        #region timeline


        //private static EventTiming[] sortByTimeline(KinectEventData ked)
        //{
        //    //TODO: do we really need to compare and sort?

        //    IComparer IFEC = new IFEComparer();
        //    Array.Sort(ked.imageFrameEvents, IFEC);

        //    IComparer SFEC = new SFEComparer();
        //    Array.Sort(ked.skeletonFrameEvents, SFEC);

        //    IComparer DFEC = new DFEComparer();
        //    Array.Sort(ked.depthFrameEvents, DFEC);

        //    EventTiming[] result = new EventTiming[ked.imageFrameEvents.Length + ked.skeletonFrameEvents.Length + ked.depthFrameEvents.Length];

        //    int ii = 0;
        //    int si = 0;
        //    int di = 0;
        //    int timeIndex = 0;
        //    while (ii < ked.imageFrameEvents.Length || si < ked.skeletonFrameEvents.Length || di < ked.depthFrameEvents.Length)
        //    {

        //        EventTiming.EventType nextEvent = fastestEvent(ked.depthFrameEvents, ked.imageFrameEvents, ked.skeletonFrameEvents, di, ii, si);
        //        switch (nextEvent)
        //        {
        //            case EventTiming.EventType.SkeletonFrameEvent:
        //                result[timeIndex] = new EventTiming(nextEvent, ked.skeletonFrameEvents[si].time, si);
        //                si++;
        //                break;
        //            case EventTiming.EventType.ImageFrameEvent:
        //                result[timeIndex] = new EventTiming(nextEvent, ked.imageFrameEvents[ii].time, ii);
        //                ii++;
        //                break;
        //            case EventTiming.EventType.DepthFrameEvent:
        //                result[timeIndex] = new EventTiming(nextEvent, ked.depthFrameEvents[di].time, di);
        //                di++;
        //                break;
        //            default:
        //                break;
        //        }
        //        timeIndex++;
        //    }

        //    return result;

        //}
        private static EventTiming.EventType fastestEvent(DepthFrameEventData[] dfes, ImageFrameEventData[] ifes, SkeletonFrameEventData[] sfes, int dfeIndex, int ifeIndex, int sfeIndex)
        {
            bool compDFE = dfeIndex < dfes.Length;
            bool compIFE = ifeIndex < ifes.Length;
            bool compSFE = sfeIndex < sfes.Length;

            if (compDFE && compIFE && compSFE)
                return fastestEvent(dfes[dfeIndex], ifes[ifeIndex], sfes[sfeIndex]);
            else if (compDFE && compIFE && !compSFE)
                return fastestEvent(dfes[dfeIndex], ifes[ifeIndex]);
            else if (compDFE && !compIFE && compSFE)
                return fastestEvent(dfes[dfeIndex], sfes[sfeIndex]);
            else if (!compDFE && compIFE && compSFE)
                return fastestEvent(ifes[ifeIndex], sfes[sfeIndex]);
            else if (compDFE && !compIFE && !compSFE)
                return EventTiming.EventType.DepthFrameEvent;
            else if (!compDFE && compIFE && !compSFE)
                return EventTiming.EventType.ImageFrameEvent;
            else
                return EventTiming.EventType.SkeletonFrameEvent;


        }

        private static EventTiming.EventType fastestEvent(DepthFrameEventData dfe, ImageFrameEventData ife, SkeletonFrameEventData sfe)
        {
            EventTiming.EventType result = EventTiming.EventType.DepthFrameEvent;
            if (dfe.time > ife.time)
                result = EventTiming.EventType.ImageFrameEvent;
            if (ife.time > sfe.time)
                result = EventTiming.EventType.SkeletonFrameEvent;
            return result;
        }
        private static EventTiming.EventType fastestEvent(DepthFrameEventData dfe, ImageFrameEventData ife)
        {
            EventTiming.EventType result = EventTiming.EventType.DepthFrameEvent;
            if (dfe.time > ife.time)
                result = EventTiming.EventType.ImageFrameEvent;
            return result;
        }
        private static EventTiming.EventType fastestEvent(ImageFrameEventData ife, SkeletonFrameEventData sfe)
        {
            EventTiming.EventType result = EventTiming.EventType.ImageFrameEvent;
            if (ife.time > sfe.time)
                result = EventTiming.EventType.SkeletonFrameEvent;
            return result;
        }
        private static EventTiming.EventType fastestEvent(DepthFrameEventData dfe, SkeletonFrameEventData sfe)
        {
            EventTiming.EventType result = EventTiming.EventType.DepthFrameEvent;
            if (dfe.time > sfe.time)
                result = EventTiming.EventType.SkeletonFrameEvent;
            return result;
        }



        #endregion
        #region Comparer

        private class IFEComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                ImageFrameEventData a = (ImageFrameEventData)(x);
                ImageFrameEventData b = (ImageFrameEventData)(y);
                return (int)(a.time - b.time);
            }
        }
        private class SFEComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                SkeletonFrameEventData a = (SkeletonFrameEventData)(x);
                SkeletonFrameEventData b = (SkeletonFrameEventData)(y);
                return (int)(a.time - b.time);
            }
        }
        private class DFEComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                DepthFrameEventData a = (DepthFrameEventData)(x);
                DepthFrameEventData b = (DepthFrameEventData)(y);
                return (int)(a.time - b.time);
            }
        }
        #endregion
    }


}
