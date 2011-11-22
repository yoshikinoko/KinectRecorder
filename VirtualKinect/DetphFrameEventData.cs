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

        public  const string rawDepthFrameDataPrefix = "depthDataRaw";
        public  const string rawDepthFrameDataSuffix = ".imd";
        public  const string previewDepthFrameDataPrefix = "depthDataPrev";
        public  const string previewDepthFrameDataSuffix = ".png";


        public ImageFrame imageFrame;

        public DepthFrameEventData(Microsoft.Research.Kinect.Nui.ImageFrameReadyEventArgs e, long time, String saveFolder)
        {
            this.imageFrame = new ImageFrame();
            this.imageFrame.NUI = e.ImageFrame;
            this.time = time;

            //Save Preview Image and raw Image and store the filename in imageFrame.image.previewFile and rawFile
            String imageRawFileName = rawDepthFrameDataPrefix + time + rawDepthFrameDataSuffix;

            this.imageFrame.Image.rawFileName = imageRawFileName;
            this.imageFrame.Image.useCompressedImage = false;
            this.imageFrame.Image.saveImage(saveFolder);

        }
    }
}