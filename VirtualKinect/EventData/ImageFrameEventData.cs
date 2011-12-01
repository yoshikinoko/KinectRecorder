using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Threading.Tasks;
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
        public const string ImageFrameDataPrefix = "imageData";
        [XmlAttribute]
        public const string ImageFrameDataSuffix = ".xml";

        [XmlAttribute]
        public long time = 0;
        //Device ID used for network 
        [XmlAttribute]
        public string device_id;
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

        public ImageFrame imageFrame;

        public ImageFrameEventData(Microsoft.Research.Kinect.Nui.ImageFrameReadyEventArgs e, long time, String saveFolder, string devide_id)
        {
            this.device_id = device_id;
            this.imageFrame = new ImageFrame();
            this.imageFrame.NUI = e.ImageFrame;
            this.time = time;
            //Save Preview Image and raw Image and store the filename in imageFrame.image.previewFile and rawFile
            String imageRawFileName = rawImageFrameDataPrefix + time + rawImageFrameDataSuffix;
            this.imageFrame.Image.rawFileName = imageRawFileName;
            this.imageFrame.Image.useCompressedImage = true;
            string imgFileDirectory = Path.Combine(saveFolder, KinectEventData.eventDataDirectory);

            string tmpEventFileName = saveFilePath(saveFolder);

            //TODO: Push to network resource
            Task t = Task.Factory.StartNew(() =>
           {
               this.imageFrame.Image.saveImage(imgFileDirectory);
               IO.saveXMLSerialTask(this, tmpEventFileName);
           });


        }
    }
}
