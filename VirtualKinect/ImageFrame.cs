using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtualKinect
{
    [Serializable]
    public class ImageFrame
    {
        public int FrameNumber;
        public PlanarImage Image;
        public ImageResolution Resolution;
        public long Timestamp;
        public ImageType Type;
        public ImageViewArea ViewArea;

        //  public ImageFrame();
        public void copy(ImageFrame data)
        {
            this.FrameNumber = data.FrameNumber;
            this.Image = new PlanarImage();
            this.Image.copy(data.Image);
            this.Resolution = (ImageResolution)((int)data.Resolution);
            this.Timestamp = data.Timestamp;
            this.Type = (ImageType)((int)data.Type);
            this.ViewArea = data.ViewArea;

        }
        public void copy(Microsoft.Research.Kinect.Nui.ImageFrame data)
        {
            this.FrameNumber = data.FrameNumber;
            this.Image = new PlanarImage();
            this.Image.copy(data.Image);
            this.Resolution = (ImageResolution)((int)data.Resolution);
            this.Timestamp = data.Timestamp;
            this.Type = (ImageType)((int)data.Type);
            this.ViewArea = new ImageViewArea();
            this.ViewArea.copy(data.ViewArea);

        }
        public Microsoft.Research.Kinect.Nui.ImageFrame NUI
        {
            get
            {

                Microsoft.Research.Kinect.Nui.ImageFrame r = new Microsoft.Research.Kinect.Nui.ImageFrame();
                r.FrameNumber = this.FrameNumber;
                r.Image = this.Image.NUI;
                r.Resolution = (Microsoft.Research.Kinect.Nui.ImageResolution)((int)this.Resolution);
                r.Timestamp = this.Timestamp;
                r.Type = (Microsoft.Research.Kinect.Nui.ImageType)((int)this.Type);
                r.ViewArea = this.ViewArea.NUI;
                return r;

            }


        }
    }
}
