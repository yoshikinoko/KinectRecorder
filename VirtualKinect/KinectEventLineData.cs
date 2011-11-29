using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace VirtualKinect
{
    class KinectEventLineData
    {
        [XmlAttribute]
        public int sequenceNumber;
        [XmlAttribute]
        public string nextFileName;
        public KinectEventData ked;
    }
}
