using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace VirtualKinect
{
    [Serializable]
    public class Joint
    {
        [XmlAttribute]
        public Microsoft.Research.Kinect.Nui.JointID ID { get; set; }
        [XmlAttribute]
        public Microsoft.Research.Kinect.Nui.JointTrackingState TrackingState { get; set; }
   
        public Vector Position { get; set; }
        [XmlIgnoreAttribute]
        public Microsoft.Research.Kinect.Nui.Joint NUI
        {
            get
            {
                Microsoft.Research.Kinect.Nui.Joint r = new Microsoft.Research.Kinect.Nui.Joint();
                r.ID = this.ID;
                r.Position = this.Position.NUI;
                r.TrackingState = (Microsoft.Research.Kinect.Nui.JointTrackingState)((int)this.TrackingState);
                return r;

            }

            set
            {
                this.ID = value.ID; ;
                this.Position = new Vector();
                this.Position.NUI = value.Position;
                this.TrackingState = value.TrackingState;

            }

        }
    }
}
