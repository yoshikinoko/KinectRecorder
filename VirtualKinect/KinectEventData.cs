using Microsoft.Research.Kinect.Nui;
using System;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

namespace VirtualKinect
{
    [Serializable]
    [XmlRootAttribute("VirtualKinectEvent", Namespace = "http://www.cpandl.com", IsNullable = false)]
    public class KinectEventData
    {
        public const String extension = ".vkd";
        [XmlArrayAttribute("DepthFrameEvents")]
        public DepthFrameEventData[] depthFrameEvents;
        [XmlArrayAttribute("ImageFrameEvents")]
        public ImageFrameEventData[] imageFrameEvents;
        [XmlArrayAttribute("SkeletonFrameEvents")]
        public SkeletonFrameEventData[] skeletonFrameEvents;
        [XmlAttribute]
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
