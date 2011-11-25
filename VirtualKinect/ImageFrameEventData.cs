using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
namespace VirtualKinect
{
    [Serializable]
    public class ImageFrameEventData
    {
        public ImageFrameEventData() { }
        [XmlAttribute]
        public const String rawImageFrameDataPrefix = "imgDataRaw";
        [XmlAttribute]
        public const String rawImageFrameDataSuffix = ".png";
        [XmlAttribute]
        public const String previewImageFrameDataPrefix = "imgDataPrev";
        [XmlAttribute]
        public const String previewImageFrameDataSuffix = ".png";
        [XmlAttribute]
        public const string ImageFrameDataPrefix = "imageData";
        [XmlAttribute]
        public const string ImageFrameDataSuffix = ".xml";
        [XmlAttribute]
        public string eventFileName;

        [XmlAttribute]
        public long time;
        [XmlAttribute]
        public string device_id;


        public ImageFrame imageFrame;

        public ImageFrameEventData(Microsoft.Research.Kinect.Nui.ImageFrameReadyEventArgs e, long time, String saveFolder,string devide_id)
        {
            this.device_id = device_id;
            this.imageFrame = new ImageFrame();
            this.imageFrame.NUI = e.ImageFrame;
            this.time = time;
            //Save Preview Image and raw Image and store the filename in imageFrame.image.previewFile and rawFile
            String imageRawFileName = rawImageFrameDataPrefix + time + rawImageFrameDataSuffix;
            this.imageFrame.Image.rawFileName = imageRawFileName;
            this.imageFrame.Image.useCompressedImage = true;
            this.imageFrame.Image.saveImage(saveFolder);
            this.eventFileName = ImageFrameDataPrefix + time + ImageFrameDataSuffix;
            string tmpEventFileName = Path.Combine(saveFolder, eventFileName);
            IO.saveXMLSerialTask(this, tmpEventFileName);
        }
    }
}
