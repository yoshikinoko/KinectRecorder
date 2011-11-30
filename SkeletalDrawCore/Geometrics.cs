using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Research.Kinect.Nui;

using System.Windows.Forms;

namespace SkeletalCore
{
    public class Geometrics
    {

        public static Point getDisplayPosition(VirtualKinect.Joint joint, Microsoft.Research.Kinect.Nui.Runtime nui, Canvas skeleton)
        {
            return getDiplayPosition(joint.NUI.Position, nui, skeleton);
        }

        public static Point getDisplayPosition(Joint joint, Microsoft.Research.Kinect.Nui.Runtime nui, Canvas skeleton)
        {
            return getDiplayPosition(joint.Position, nui, skeleton);
        }
        private static Point getDiplayPosition(Microsoft.Research.Kinect.Nui.Vector Position, Microsoft.Research.Kinect.Nui.Runtime nui, Canvas skeleton)
        {

            float depthX, depthY;
            nui.SkeletonEngine.SkeletonToDepthImage(Position, out depthX, out depthY);
            depthX = depthX * 320; //convert to 320, 240 space
            depthY = depthY * 240; //convert to 320, 240 space
            int colorX, colorY;
            ImageViewArea iv = new ImageViewArea();
            // only ImageResolution.Resolution640x480 is supported at this point
            nui.NuiCamera.GetColorPixelCoordinatesFromDepthPixel(ImageResolution.Resolution640x480, iv, (int)depthX, (int)depthY, (short)0, out colorX, out colorY);

            // map back to skeleton.Width & skeleton.Height
            return new Point((int)(skeleton.Width * colorX / 640.0), (int)(skeleton.Height * colorY / 480));


        }

    }
}
