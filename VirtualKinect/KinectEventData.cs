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

        
        public string[] skeletonFrameEventFileNames;
        public string[] imageFrameEventFileNames;
        public string[] depthFrameEventFileNames;

        //[XmlIgnoreAttribute]
        public SkeletonFrameEventData[] skeletonFrameEvents;
        //[XmlIgnoreAttribute]
        public ImageFrameEventData[] imageFrameEvents;
        //[XmlIgnoreAttribute]
        public DepthFrameEventData[] depthFrameEvents;


        private void loadSkeletonFrameEventData(string eventRootfolder)
        {
            for (int i = 0; i < skeletonFrameEventFileNames.Length; i++)
            {
                string loadFileName = Path.Combine(eventRootfolder, this.skeletonFrameEventFileNames[i]);
                skeletonFrameEvents[i] = (SkeletonFrameEventData)IO.loadXMLSerialType(loadFileName,typeof(SkeletonFrameEventData));
            }
        }
        private void loadImageFrameEventData(string eventRootfolder)
        {
            for (int i = 0; i < imageFrameEventFileNames.Length; i++)
            {
                string loadFileName = Path.Combine(eventRootfolder, this.imageFrameEventFileNames[i]);
                imageFrameEvents[i] = (ImageFrameEventData)IO.loadXMLSerialType(loadFileName,typeof(ImageFrameEventData));
            }
        }
        private void loadDepthFrameEventData(string eventRootfolder)
        {
            for (int i = 0; i < depthFrameEventFileNames.Length; i++)
            {
                string loadFileName = Path.Combine(eventRootfolder, this.depthFrameEventFileNames[i]);
                depthFrameEvents[i] = (DepthFrameEventData)IO.loadXMLSerialType(loadFileName,typeof(DepthFrameEventData));
            }
        }
        public void loadEventData(string eventRootFolder)
        {
            this.skeletonFrameEvents = new SkeletonFrameEventData[skeletonFrameEventFileNames.Length];
            loadSkeletonFrameEventData(eventRootFolder);
            this.imageFrameEvents = new ImageFrameEventData[imageFrameEventFileNames.Length];
            loadImageFrameEventData(eventRootFolder);
            this.depthFrameEvents = new DepthFrameEventData[depthFrameEventFileNames.Length];
            loadDepthFrameEventData(eventRootFolder);
        }

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

        public void setFileNames(DateTime date, long duration, string[] depthFrameEventFileNames, string[] imageFrameEventFileNames, string[] skeletonFrameEventFileNames)
        {
            this.date = date;
            this.depthFrameEventFileNames = depthFrameEventFileNames;
            this.imageFrameEventFileNames = imageFrameEventFileNames;
            this.skeletonFrameEventFileNames = skeletonFrameEventFileNames;
            this.duration = duration;
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
