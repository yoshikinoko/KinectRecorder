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
        [XmlAttribute]
        public string indexFileName;
        [XmlAttribute]
        public int totalEvents;

        public KinectEventLineData loadIndexEvent(string eventRootFolder)
        {
            string loadPath = Path.Combine(eventRootFolder, indexFileName);
            return (KinectEventLineData)IO.load(loadPath);
        }
        public void set(string device_id, DateTime date, long duration, int totalevents, string kinectEventIndexFileName)
        {
            this.device_id = device_id;
            this.date = date;
            this.indexFileName = kinectEventIndexFileName;
            this.duration = duration;
            this.totalEvents = totalevents;
        }
    }
}