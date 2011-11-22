using System;
using System.Text;
using System.Xml;
using System.Xml.Serialization;


namespace VirtualKinect
{
    [Serializable]
    public class Vector
    {
        [XmlAttribute]
        public float W;
        [XmlAttribute]
        public float X;
        [XmlAttribute]
        public float Y;
        [XmlAttribute]
        public float Z;
        [XmlIgnoreAttribute]
        public Microsoft.Research.Kinect.Nui.Vector NUI
        {
            get
            {

                Microsoft.Research.Kinect.Nui.Vector r = new Microsoft.Research.Kinect.Nui.Vector();
                r.W = this.W;
                r.X = this.X;
                r.Y = this.Y;
                r.Z = this.Z;
                return r;
            }
            set
            {
                this.W = value.W;
                this.X = value.X;
                this.Y = value.Y;
                this.Z = value.Z;
            }
        }
    }
}
