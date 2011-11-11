using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtualKinect
{
    [Serializable]
    public class SkeletonFrame
    {
        public Vector FloorClipPlane;
        public int FrameNumber;
        public Vector NormalToGravity;
        public Microsoft.Research.Kinect.Nui.SkeletonFrameQuality Quality;
        public SkeletonData[] Skeletons;
        public long TimeStamp;
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
