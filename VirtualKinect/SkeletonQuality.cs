using System;

namespace VirtualKinect
{
    [Serializable]
    public enum SkeletonQuality
    {
        ClippedRight = 1,
        ClippedLeft = 2,
        ClippedTop = 4,
        ClippedBottom = 8,
    }
}
