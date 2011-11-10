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
        public void copy(Microsoft.Research.Kinect.Nui.SkeletonFrame data)
        {
            this.FloorClipPlane = new Vector();
            this.FloorClipPlane.copy(data.FloorClipPlane);
            this.FrameNumber = data.FrameNumber;
            this.NormalToGravity = new Vector();
            this.NormalToGravity.copy(data.NormalToGravity);
            this.Quality =data.Quality;
            this.Skeletons = new SkeletonData[data.Skeletons.Length];       
            for (int i = 0 ; i < data.Skeletons.Length ;i++)
            {
                this.Skeletons[i] = new SkeletonData();
                this.Skeletons[i].copy(data.Skeletons[i]);
            }
            this.TimeStamp = data.TimeStamp;
        }
        //public Microsoft.Research.Kinect.Nui.SkeletonFrame NUI {
        //    get {

        //        Microsoft.Research.Kinect.Nui.SkeletonFrame r = new Microsoft.Research.Kinect.Nui.SkeletonFrame();
        //        r.FloorClipPlane = this.FloorClipPlane.NUI;
        //        r.FrameNumber = this.FrameNumber;
        //        r.NormalToGravity = this.NormalToGravity.NUI;
        //        r.Quality = (Microsoft.Research.Kinect.Nui.SkeletonFrameQuality)((int)this.Quality);
        //        r.Skeletons = 
        //    }
        //}

    }
}
