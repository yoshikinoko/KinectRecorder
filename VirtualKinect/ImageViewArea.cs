using System;
using System.Xml;
using System.Xml.Serialization;


namespace VirtualKinect
{
    [Serializable]
    public class ImageViewArea
    {
        [XmlAttribute]
        public int CenterX;
        [XmlAttribute]
        public int CenterY;
                
        [XmlAttribute]  
        public Microsoft.Research.Kinect.Nui.ImageDigitalZoom Zoom;

        [XmlIgnoreAttribute]
        public Microsoft.Research.Kinect.Nui.ImageViewArea NUI
        {
            get
            {
                Microsoft.Research.Kinect.Nui.ImageViewArea r = new Microsoft.Research.Kinect.Nui.ImageViewArea();
                r.CenterX = this.CenterX;
                r.CenterY = this.CenterY;
                r.Zoom = this.Zoom;
                return r;
            }
            set
            {
                this.CenterX = value.CenterX;
                this.CenterY = value.CenterY;
                this.Zoom = value.Zoom;
            }
        }
    }
}
