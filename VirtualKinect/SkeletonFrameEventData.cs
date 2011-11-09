using System;
using System.Xml;
using System.Xml.Serialization;
//using Microsoft.Research.Kinect;

namespace VirtualKinect
{
    [Serializable]
    public class SkeletonFrameEventData
    {
        [XmlAttribute]
        public long time;
        // public SkeletonFrameReadyEventArgs data;
        //  public SkeletonFrame skeletonFrame;
        public SkeletonFrame SkeletonFrame;
        public SkeletonFrameEventData(Microsoft.Research.Kinect.Nui.SkeletonFrameReadyEventArgs e, long time)
        {
            //  skeletonFrame = e.SkeletonFrame;
            //   this.data = e;
            this.SkeletonFrame = new SkeletonFrame();
            this.SkeletonFrame.copy(e.SkeletonFrame);
            this.time = time;
        }
    }
}
