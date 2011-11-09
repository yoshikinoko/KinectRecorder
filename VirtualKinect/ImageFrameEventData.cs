using System;
using System.Xml;
using System.Xml.Serialization;

namespace VirtualKinect
{
    [Serializable]
    public class ImageFrameEventData
    {
        public long time;

        public ImageFrame imageFrame;
        //  public ImageFrameReadyEventArgs data;
        public ImageFrameEventData(Microsoft.Research.Kinect.Nui.ImageFrameReadyEventArgs e, long time)
        {
            //  this.data = e;
            // this.imageFrame = e.ImageFrame;
            this.imageFrame = new ImageFrame();
            this.imageFrame.copy(e.ImageFrame);
            this.time = time;
        }
    }
}
