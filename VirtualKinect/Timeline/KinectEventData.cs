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
        [XmlAttribute]
        public const String eventDataDirectory = "events";


        public KinectEventLineData loadIndexEvent(string eventRootFolder)
        {
            String epath = Path.Combine(eventRootFolder, eventDataDirectory);
            string loadPath = Path.Combine(epath, indexFileName);
            return (KinectEventLineData)IO.load(loadPath);
        }
        public KinectEventLineData loadEventBySequenceNumber(string eventRootFolder, int index)
        {
            string loadFileName = KinectEventLineData.indexFileName(index);
            String epath = Path.Combine(eventRootFolder, eventDataDirectory);

            string loadPath = Path.Combine(epath, loadFileName);
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