using Microsoft.Research.Kinect.Nui;
using System;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Threading.Tasks;

namespace VirtualKinect
{
    [Serializable]
    [XmlRoot("KinectEventData")]
    public class KinectEventData
    {
        [XmlAttribute]
        public String datafolder;
        [XmlAttribute]
        public const String dateFormatStyle = "yyyy-MM-dd-HH-mm-ss";
        [XmlAttribute]
        public const String extension = ".vkd";
        [XmlAttribute]
        public DateTime date;
        [XmlAttribute]
        public long duration;

        public SkeletonFrameEventData[] skeletonFrameEvents;

        public ImageFrameEventData[] imageFrameEvents;

        public DepthFrameEventData[] depthFrameEvents;

        private void loadRawImageEventData(String eventRootFolder)
        {
            Parallel.For(0, imageFrameEvents.Length, (n) => imageFrameEvents[n].imageFrame.Image.loadImage(eventRootFolder));
        }

        private void loadRawDepthEventData(String eventRootFolder)
        {
            Parallel.For(0, depthFrameEvents.Length, (n) => depthFrameEvents[n].imageFrame.Image.loadImage(eventRootFolder));
        }

        public void loadRawEventData(String eventRootFolder)
        {
            loadRawImageEventData(eventRootFolder);
            loadRawDepthEventData(eventRootFolder);
        }

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
