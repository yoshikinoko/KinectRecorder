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
        public string datafolder;
        [XmlAttribute]
        public const string dateFormatStyle = "yyyy-MM-dd-HH-mm-ss";
        [XmlAttribute]
        public const string extension = ".vkd";
        [XmlAttribute]
        public DateTime date;
        [XmlAttribute]
        public long duration;
        [XmlAttribute]
        public string device_id;


        //[XmlIgnoreAttribute]
        public SkeletonFrameEventData[] skeletonFrameEvents;
        //[XmlIgnoreAttribute]
        public ImageFrameEventData[] imageFrameEvents;
        //[XmlIgnoreAttribute]
        public DepthFrameEventData[] depthFrameEvents;


        private void loadRawImageEventData(string eventRootFolder)
        {
            Parallel.For(0, imageFrameEvents.Length, (n) => imageFrameEvents[n].imageFrame.Image.loadImage(eventRootFolder));
        }

        //Using Async
        private async void loadRawImageEventDataAsync(string eventRootFolder)
        {
            for (int n = 0; n < imageFrameEvents.Length; n++)
            {
                await imageFrameEvents[n].imageFrame.Image.loadImageTask(eventRootFolder);
            }
        }


        private async void loadRawDepthEventDataAsync(string eventRootFolder)
        {
            for (int n = 0; n < depthFrameEvents.Length; n++)
            {
                await depthFrameEvents[n].imageFrame.Image.loadImageTask(eventRootFolder);
            }
        }

        public void loadRawEventData(string eventRootFolder)
        {
            loadRawImageEventDataAsync(eventRootFolder);
            loadRawDepthEventDataAsync(eventRootFolder);
        }


        public void set(string device_id, DateTime date, long duration, DepthFrameEventData[] depthFrameEvents, ImageFrameEventData[] imageFrameEvents, SkeletonFrameEventData[] skeletonFrameEvents)
        {

            this.device_id = device_id;
            this.date = date;
            this.depthFrameEvents = depthFrameEvents;
            this.imageFrameEvents = imageFrameEvents;
            this.skeletonFrameEvents = skeletonFrameEvents;
            this.duration = duration;
        }
    }
}
