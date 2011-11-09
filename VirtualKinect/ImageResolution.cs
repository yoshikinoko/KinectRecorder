using System;

namespace VirtualKinect
{
    [Serializable]
    public enum ImageResolution
    {
        Invalid = -1,
        Resolution80x60 = 0,
        Resolution320x240 = 1,
        Resolution640x480 = 2,
        Resolution1280x1024 = 3,
    }
}
