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
        public long time=0;
        [XmlAttribute]
        public const string ImageFrameDataPrefix = "skeletonData";
        [XmlAttribute]
        public const string ImageFrameDataSuffix = ".xml";
        [XmlAttribute]
        public string device_id;

        public SkeletonFrame SkeletonFrame;

        [XmlIgnoreAttribute]
        public string saveFileName
        {
            get
            {

                return ImageFrameDataPrefix + time + ImageFrameDataSuffix;

            }

        }
        private string saveFilePath(string saveFolder)
        {
            return Path.Combine(saveFolder, saveFileName);

        }
        public SkeletonFrameEventData(Microsoft.Research.Kinect.Nui.SkeletonFrameReadyEventArgs e, long time, string saveFolder, string devide_id)
        {
            this.device_id = device_id;
            this.SkeletonFrame = new SkeletonFrame();
            this.SkeletonFrame.NUI = e.SkeletonFrame;
            this.time = time;
            string tmpEventFileName = saveFilePath(saveFolder);
            //TODO: Push to network resource
            IO.saveXMLSerialTask(this, tmpEventFileName);
       
            //Push to network
            //   IO.saveXMLSerialTask(this, tmpEventFileName);
        }
    }
}
