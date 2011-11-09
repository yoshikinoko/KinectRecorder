using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtualKinect
{
    [Serializable]
    public enum JointTrackingState
    {
        NotTracked = 0,
        Inferred = 1,
        Tracked = 2,
    }
}
