using System;


namespace VirtualKinect
{
    [Serializable]
    public enum ImageType
    {
        DepthAndPlayerIndex = 0,
        Color = 1,
        ColorYuv = 2,
        ColorYuvRaw = 3,
        Depth = 4,
    }
}
