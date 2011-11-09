using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;
namespace VirtualKinect
{
    public class EventTiming
    {
        public enum EventType { ImageFrameEvent, DepthFrameEvent, SkeletonFrameEvent, COUNT };
        public EventType type;
        public long time;
        public int index;
        public EventTiming(EventType type, long time, int index)
        {
            this.type = type;
            this.time = time;
            this.index = index;

        }
    }

}
