using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace VirtualKinect
{
    [Serializable]
    public class SkeletonFrame
    {
        [XmlAttribute]
        public int FrameNumber;
        [XmlAttribute]
        public long TimeStamp;
        [XmlAttribute]
        public Microsoft.Research.Kinect.Nui.SkeletonFrameQuality Quality;

        public Vector FloorClipPlane;
        public Vector NormalToGravity;
      
        public SkeletonData[] Skeletons;
     
        [XmlIgnoreAttribute]
        public Microsoft.Research.Kinect.Nui.SkeletonFrame NUI
        {

            set
            {
                this.FloorClipPlane = new Vector();
                this.FloorClipPlane.NUI = value.FloorClipPlane;
                this.FrameNumber = value.FrameNumber;
                this.NormalToGravity = new Vector();
                this.NormalToGravity.NUI = value.NormalToGravity;
                this.Quality = value.Quality;
                this.Skeletons = new SkeletonData[value.Skeletons.Length];

                for (int i = 0; i < value.Skeletons.Length; i++)
                {
                    this.Skeletons[i] = new SkeletonData();
                    this.Skeletons[i].NUI = value.Skeletons[i];
                }
                this.TimeStamp = value.TimeStamp;
            }
        }
    }
}
