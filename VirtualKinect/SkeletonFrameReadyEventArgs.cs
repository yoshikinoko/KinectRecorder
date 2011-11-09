using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtualKinect
{
    public sealed class SkeletonFrameReadyEventArgs : EventArgs
    {
       // public SkeletonFrameReadyEventArgs();

        public SkeletonFrame SkeletonFrame { get; set; }



    }
}
