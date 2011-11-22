using System;
using System.Xml;
using System.Xml.Serialization;

//using Microsoft.Research.Kinect;

namespace VirtualKinect
{
    [Serializable]

    public class SkeletonFrameEventData
    {

        public SkeletonFrameEventData() { }
          [XmlAttribute]  
        public long time;
        public SkeletonFrame SkeletonFrame;

        public SkeletonFrameEventData(Microsoft.Research.Kinect.Nui.SkeletonFrameReadyEventArgs e, long time)
        {
            this.SkeletonFrame = new SkeletonFrame();
            this.SkeletonFrame.NUI = e.SkeletonFrame;
            this.time = time;
        }
    }
}
