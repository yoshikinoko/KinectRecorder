using System;

namespace VirtualKinect
{
    [Serializable]
    public class ImageViewArea
    {
        public int CenterX;
        public int CenterY;
        public Microsoft.Research.Kinect.Nui.ImageDigitalZoom Zoom;
        
        public Microsoft.Research.Kinect.Nui.ImageViewArea NUI
        {
            get {
                Microsoft.Research.Kinect.Nui.ImageViewArea r = new Microsoft.Research.Kinect.Nui.ImageViewArea();
                r.CenterX = this.CenterX;
                r.CenterY = this.CenterY;
                r.Zoom = this.Zoom;
                return r;
            }
set{
            this.CenterX = value.CenterX;
            this.CenterY = value.CenterY;
            this.Zoom = value.Zoom;
}
        }
    }
}
