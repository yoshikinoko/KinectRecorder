using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtualKinect
{
    [Serializable]
    public class Joint
    {
        public JointID ID { get; set; }
        public Vector Position { get; set; }
        public JointTrackingState TrackingState { get; set; }
        public void copy(Microsoft.Research.Kinect.Nui.Joint data)
        {
            this.ID = (JointID)((int)data.ID);
            this.Position = new Vector();
            this.Position.copy(data.Position);
            this.TrackingState = (JointTrackingState)((int)data.TrackingState);
        }
        public Microsoft.Research.Kinect.Nui.Joint NUI
        {
            get
            {
                Microsoft.Research.Kinect.Nui.Joint r = new Microsoft.Research.Kinect.Nui.Joint();
                r.ID = (Microsoft.Research.Kinect.Nui.JointID)((int)(this.ID));
                r.Position = this.Position.NUI;
                r.TrackingState = (Microsoft.Research.Kinect.Nui.JointTrackingState)((int)this.TrackingState);
                return r;

            }

        }
    }
}
