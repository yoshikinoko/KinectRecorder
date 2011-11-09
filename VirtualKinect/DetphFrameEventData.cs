using System;
using System.Xml;
using System.Xml.Serialization;


namespace VirtualKinect
{
    [Serializable]
    public class DepthFrameEventData
    {
        [XmlAttribute]
        public long time;
        public ImageFrame imageFrame;

        public DepthFrameEventData(Microsoft.Research.Kinect.Nui.ImageFrameReadyEventArgs e, long time)
        {
            this.imageFrame = new ImageFrame();
            this.imageFrame.copy(e.ImageFrame);
            this.time = time;
        }
    }
}
