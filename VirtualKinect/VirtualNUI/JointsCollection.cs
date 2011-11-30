using System;
using System.Collections;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

using System.Collections.Generic;

namespace VirtualKinect
{
    [Serializable]
    public struct JointsCollection : IEnumerable
    {

        [XmlIgnoreAttribute]
        public Microsoft.Research.Kinect.Nui.JointsCollection NUI
        {
            set
            {
                _joints = new ArrayList();
                foreach (Microsoft.Research.Kinect.Nui.Joint joint in value)
                {
                    Joint temp_joint = new Joint();
                    temp_joint.NUI = joint;
                    _joints.Add(temp_joint);
                }
            }
        }
        [XmlIgnoreAttribute]
        public Joint this[Microsoft.Research.Kinect.Nui.JointID i]
        {
            get
            {
                for (int k = 0; k < _joints.Count; k++)
                {
                    Joint j = (Joint)_joints[k];
                    if (j.ID.Equals(i))
                        return j;
                }
                return new Joint();
            }
        }
        [XmlIgnoreAttribute]
        public int Count { get { return _joints.Count; } }

        private ArrayList _joints;
        public IEnumerator GetEnumerator()
        {
            return new JointEnum(_joints);
        }

        public void Add(object joint)
        {
            if (_joints == null)
                _joints = new ArrayList();
            _joints.Add(joint);
        }

        private class JointEnum : IEnumerator
        {
            private ArrayList _jointList;
            int position;
            public JointEnum(ArrayList list)
            {
                _jointList = (ArrayList)list.Clone();
                Reset();
            }
            public bool MoveNext()
            {
                position++;
                return (position < _jointList.Count);
            }
            public void Reset()
            {
                position = -1;
            }
            public object Current
            {
                get
                {
                    return _jointList[position];
                }
            }
        }

    }

}

