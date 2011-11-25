using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace VirtualKinect
{
    [Serializable]
    public class DepthFrameEventData
    {

        public DepthFrameEventData() { }
        [XmlAttribute]
        public long time;
        [XmlAttribute]
        public const string rawDepthFrameDataPrefix = "depthDataRaw";
        [XmlAttribute]
        public const string rawDepthFrameDataSuffix = ".imd";
        [XmlAttribute]
        public const string previewDepthFrameDataPrefix = "depthDataPrev";
        [XmlAttribute]
        public const string previewDepthFrameDataSuffix = ".png";
        [XmlAttribute]
        public const string DepthFrameDataPrefix = "depthData";
        [XmlAttribute]
        public const string DepthFrameDataSuffix = ".xml";
        [XmlAttribute]
        public string eventFileName;
        [XmlAttribute]
        public string device_id;


        public ImageFrame imageFrame;


        public DepthFrameEventData(Microsoft.Research.Kinect.Nui.ImageFrameReadyEventArgs e, long time, String saveFolder,string devide_id)
        {
            this.device_id = device_id;
            this.imageFrame = new ImageFrame();
            this.imageFrame.NUI = e.ImageFrame;
            this.time = time;
            //Save Preview Image and raw Image and store the filename in imageFrame.image.previewFile and rawFile
            String imageRawFileName = rawDepthFrameDataPrefix + time + rawDepthFrameDataSuffix;
            this.imageFrame.Image.rawFileName = imageRawFileName;
            this.imageFrame.Image.useCompressedImage = false;
            this.imageFrame.Image.saveImage(saveFolder);
            this.eventFileName = DepthFrameDataPrefix + time + DepthFrameDataSuffix;
            String tmpEventFileName = Path.Combine(saveFolder, eventFileName);
            IO.saveXMLSerialTask(this, tmpEventFileName);
        }






    }
}