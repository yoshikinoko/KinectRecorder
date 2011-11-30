using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtualKinect
{
    public sealed class SkeletonFrameReadyEventArgs : EventArgs
    {
        public SkeletonFrame SkeletonFrame { get; set; }
        public string eventFileName { get; set; }

    }
}
