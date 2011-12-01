using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Threading.Tasks;
//using Microsoft.Research.Kinect;

namespace VirtualKinect
{
    [Serializable]

    public class SkeletonFrameEventData
    {

        public SkeletonFrameEventData() { }
        [XmlAttribute]
        public long time = 0;
        [XmlIgnoreAttribute]
        public const string ImageFrameDataPrefix = "skeletonData";
        [XmlIgnoreAttribute]
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
            String tmp = Path.Combine(saveFolder, KinectEventData.eventDataDirectory);

            return Path.Combine(tmp, saveFileName);

        }
        public SkeletonFrameEventData(Microsoft.Research.Kinect.Nui.SkeletonFrameReadyEventArgs e, long time, string saveFolder, string devide_id)
        {
            this.device_id = device_id;
            this.SkeletonFrame = new SkeletonFrame();
            this.SkeletonFrame.NUI = e.SkeletonFrame;
            this.time = time;
            string tmpEventFileName = saveFilePath(saveFolder);

            //TODO: Push to network resource
            Task t = Task.Factory.StartNew(() =>
            {
                IO.saveXMLSerialTask(this, tmpEventFileName);

            });

            //Push to network

        }
    }
}
