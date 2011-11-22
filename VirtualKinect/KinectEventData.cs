using Microsoft.Research.Kinect.Nui;
using System;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

namespace VirtualKinect
{
    [Serializable]
    public class KinectEventData
    {
            [XmlIgnoreAttribute]
        public const String extension = ".vkd";
       [XmlIgnoreAttribute]
        public DepthFrameEventData[] depthFrameEvents;
             [XmlIgnoreAttribute]
        public ImageFrameEventData[] imageFrameEvents;

        public SkeletonFrameEventData[] skeletonFrameEvents;
     
        public DateTime date;
        public long duration;
        public void set(DateTime date, long duration, DepthFrameEventData[] depthFrameEvents, ImageFrameEventData[] imageFrameEvents, SkeletonFrameEventData[] skeletonFrameEvents)
        {
            this.date = date;
            this.depthFrameEvents = depthFrameEvents;
            this.imageFrameEvents = imageFrameEvents;
            this.skeletonFrameEvents = skeletonFrameEvents;
            this.duration = duration;
        }
    }
}
