using System;

namespace VirtualKinect
{
    [Serializable]
    public class ImageViewArea
    {
        public int CenterX;
        public int CenterY;
        public ImageDigitalZoom Zoom;
        public void copy(Microsoft.Research.Kinect.Nui.ImageViewArea data)
        {
            this.CenterX = data.CenterX;
            this.CenterY = data.CenterY;
            this.Zoom = (ImageDigitalZoom)((int)data.Zoom);
        }

        public Microsoft.Research.Kinect.Nui.ImageViewArea NUI
        {
            get {

                Microsoft.Research.Kinect.Nui.ImageViewArea r = new Microsoft.Research.Kinect.Nui.ImageViewArea();
                r.CenterX = this.CenterX;
                r.CenterY = this.CenterY;
                r.Zoom = (Microsoft.Research.Kinect.Nui.ImageDigitalZoom)((int)this.Zoom);
                return r;
            }
        }
    }
}
