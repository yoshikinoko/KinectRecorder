using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtualKinect
{
    [Serializable]
    public class SkeletonData
    {
        public JointsCollection Joints;
        public Vector Position;
        public SkeletonQuality Quality;
        public int TrackingID;
        public SkeletonTrackingState TrackingState;
        public int UserIndex;
        public void copy(Microsoft.Research.Kinect.Nui.SkeletonData data)
        {
            this.Joints = new JointsCollection();
            this.Joints.copy(data.Joints);
            this.Position = new Vector();
            this.Position.copy(data.Position);
            this.TrackingID = data.TrackingID;
            this.TrackingState = (SkeletonTrackingState)((int)data.TrackingState);
            this.UserIndex = data.UserIndex;
        }
        //public Microsoft.Research.Kinect.Nui.SkeletonData NUI {
        //    get {

        //        Microsoft.Research.Kinect.Nui.SkeletonData r = new Microsoft.Research.Kinect.Nui.SkeletonData();
        //        r.Joints = new Microsoft.Research.Kinect.Nui.Joint[Microsoft.Research.Kinect.Nui.JointID.Count]; 
        //        foreach (Joint j in r.Joints)
        //        {


        //        }
        //    }
        
        //}
    
    }
}
