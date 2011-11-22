using Microsoft.Research.Kinect.Nui;
using System;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
namespace VirtualKinect
{
    [Serializable]
    [XmlRoot("KinectEventData")]
    public class KinectEventData
    {
        [XmlAttribute]
        public String datafolder;


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
            for (int i = 0; i < imageFrameEvents.Length; i++)
            {
                imageFrameEvents[i].imageFrame.Image.loadImage(eventRootFolder);
            }
        }
        private void loadRawDepthEventData(String eventRootFolder)
        {

            for (int i = 0; i < depthFrameEvents.Length; i++)
            {

                depthFrameEvents[i].imageFrame.Image.loadImage(eventRootFolder);
            }
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
