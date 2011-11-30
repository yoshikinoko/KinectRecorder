using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
namespace VirtualKinect
{
    public enum EventType { ImageFrameEvent, DepthFrameEvent, SkeletonFrameEvent, COUNT };
     
    [Serializable]
    public class KinectEventLineData
    {

        [XmlIgnoreAttribute]
        public const string Prefix = "tl";
        [XmlIgnoreAttribute]
        public const string Suffix = ".kel";
        [XmlIgnoreAttribute]
        public string saveFileName
        {
            get
            {
                return Prefix + sequenceNumber + Suffix;
            }
        }
        public static string indexFileName(int sequenceNumber){
            return Prefix + sequenceNumber + Suffix;
        }
     
     

        [XmlAttribute]
        public int sequenceNumber;
        [XmlAttribute]
        public string nextFileName="";
        [XmlAttribute]
        public string previousFileName="";
        [XmlAttribute]
        public long time;

        [XmlAttribute]
        public string kinectEventName;
        [XmlAttribute]
        public EventType kinectEventType;
        [XmlAttribute]
        public bool isEnd = false;


        public void set(int sequenceNumber,string previousFileName,long time, string kinectEventName, EventType kinectEvent)
        {
            this.time = time;
            this.sequenceNumber = sequenceNumber;
            this.kinectEventName = kinectEventName;
            this.kinectEventType = kinectEvent;
            this.previousFileName = previousFileName;
            this.isEnd = false;
        }
        public object loadKinectEvent(string eventRootFolder){
        
            string loadPath = Path.Combine(eventRootFolder,kinectEventName);
            object result = new object();
            switch (kinectEventType)
            {
                case EventType.DepthFrameEvent:
                    DepthFrameEventData dfe=  (DepthFrameEventData)IO.loadXMLSerialType(loadPath, typeof(DepthFrameEventData));
                    dfe.imageFrame.Image.loadImage(eventRootFolder);
                    result =  dfe;
                    break;
                case EventType.ImageFrameEvent:
                    ImageFrameEventData ife = (ImageFrameEventData)IO.loadXMLSerialType(loadPath, typeof(ImageFrameEventData));
                    ife.imageFrame.Image.loadImage(eventRootFolder);
                    result=  ife;
                    break;
                case EventType.SkeletonFrameEvent:
                    result =  IO.loadXMLSerialType(loadPath, typeof(SkeletonFrameEventData));
                    break;
                default:
                    break;
 
            }
            return result;
            
        }

        public KinectEventLineData loadNextEvent(string eventRootFolder)
        {
            string loadPath = Path.Combine(eventRootFolder, nextFileName);
            return (KinectEventLineData)IO.load(loadPath);
        }
        public KinectEventLineData loadPreviousEvent(string eventRootFolder)
        {
            string loadPath = Path.Combine(eventRootFolder, previousFileName);
            return (KinectEventLineData)IO.load(loadPath);
        }

        public void finish()
        {
            this.isEnd = true;
            nextFileName = "";
        }
        public void save(string eventDataFolder)
        {
            string savePath = Path.Combine(eventDataFolder, saveFileName);
            IO.save(this, savePath);

        }
    }
}
