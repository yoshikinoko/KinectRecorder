using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Threading.Tasks;


namespace VirtualKinect
{
    [Serializable]
    public class DepthFrameEventData
    {

        public DepthFrameEventData() { }
        [XmlAttribute]
        public long time = 0;
        [XmlAttribute]
        public const string rawDepthFrameDataPrefix = "depthDataRaw";
        [XmlAttribute]
        public const string rawDepthFrameDataSuffix = ".imd";
        [XmlAttribute]
        public const string DepthFrameDataPrefix = "depthData";
        [XmlAttribute]
        public const string DepthFrameDataSuffix = ".xml";
        [XmlAttribute]
        public string device_id;

        public ImageFrame imageFrame;
        [XmlIgnoreAttribute]
        public string saveFileName
        {
            get
            {

                return DepthFrameDataPrefix + time + DepthFrameDataSuffix;

            }

        }
        private string saveFilePath(string saveFolder)
        {
            String tmp = Path.Combine(saveFolder,KinectEventData.eventDataDirectory);

            return Path.Combine(tmp, saveFileName);

        }


        public DepthFrameEventData(Microsoft.Research.Kinect.Nui.ImageFrameReadyEventArgs e, long time, String saveFolder, string devide_id)
        {
            this.device_id = device_id;
            this.imageFrame = new ImageFrame();
            this.imageFrame.NUI = e.ImageFrame;
            this.time = time;
            //Save Preview Image and raw Image and store the filename in imageFrame.image.previewFile and rawFile
            String imageRawFileName = rawDepthFrameDataPrefix + time + rawDepthFrameDataSuffix;
            this.imageFrame.Image.rawFileName = imageRawFileName;
            this.imageFrame.Image.useCompressedImage = false;
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