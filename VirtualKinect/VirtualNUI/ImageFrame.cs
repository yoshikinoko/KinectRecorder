using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace VirtualKinect
{
    [Serializable]
    public class ImageFrame
    {
        [XmlAttribute]
        public int FrameNumber;
        [XmlAttribute]
        public Microsoft.Research.Kinect.Nui.ImageResolution Resolution;
        [XmlAttribute]
        public long Timestamp;
        [XmlAttribute]
        public Microsoft.Research.Kinect.Nui.ImageType Type;

        public ImageViewArea ViewArea;

        public PlanarImage Image;

        [XmlIgnoreAttribute]
        public Microsoft.Research.Kinect.Nui.ImageFrame NUI
        {
            get
            {
                Microsoft.Research.Kinect.Nui.ImageFrame r = new Microsoft.Research.Kinect.Nui.ImageFrame();
                r.FrameNumber = this.FrameNumber;
                r.Image = this.Image.NUI;
                r.Resolution = (Microsoft.Research.Kinect.Nui.ImageResolution)((int)this.Resolution);
                r.Timestamp = this.Timestamp;
                r.Type = this.Type;
                r.ViewArea = this.ViewArea.NUI;
                return r;
            }

            set
            {
                this.Image = new PlanarImage();
                this.Image.NUI = value.Image;
                this.FrameNumber = value.FrameNumber;
                this.Resolution = value.Resolution;
                this.Timestamp = value.Timestamp;
                this.Type = value.Type;
                this.ViewArea = new ImageViewArea();
                this.ViewArea.NUI = value.ViewArea;
            }
        }
    }
}
