using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
//using Microsoft.Research.Kinect;

namespace VirtualKinect
{
    [Serializable]

    public class SkeletonFrameEventData
    {

        public SkeletonFrameEventData() { }
        [XmlAttribute]
        public long time;
        [XmlAttribute]
        public const string ImageFrameDataPrefix = "skeletonData";
        [XmlAttribute]
        public const string ImageFrameDataSuffix = ".xml";
        [XmlAttribute]
        public string eventFileName;
        [XmlAttribute]
        public string device_id;

        public SkeletonFrame SkeletonFrame;

        public SkeletonFrameEventData(Microsoft.Research.Kinect.Nui.SkeletonFrameReadyEventArgs e, long time, string saveFolder, string devide_id)
        {
            this.device_id = device_id;
            this.SkeletonFrame = new SkeletonFrame();
            this.SkeletonFrame.NUI = e.SkeletonFrame;
            this.time = time;
            this.eventFileName = ImageFrameDataPrefix + time + ImageFrameDataSuffix;
            string tmpEventFileName = Path.Combine(saveFolder, eventFileName);
            IO.saveXMLSerialTask(this, tmpEventFileName);
        }
    }
}
