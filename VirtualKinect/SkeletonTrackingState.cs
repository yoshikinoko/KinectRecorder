using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtualKinect
{
    [Serializable]
    public enum SkeletonTrackingState
    {
        NotTracked = 0,
        PositionOnly = 1,
        Tracked = 2,
    }
}
