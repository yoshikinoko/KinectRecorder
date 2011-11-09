using System;
using System.Collections;
using System.Reflection;


namespace VirtualKinect
{
    [Serializable]
    public struct JointsCollection : IEnumerable
    {
        private Joint[] _joints;

        public void copy(Microsoft.Research.Kinect.Nui.JointsCollection data)
        {
            _joints = new Joint[(int)JointID.Count];
            foreach (Microsoft.Research.Kinect.Nui.Joint i in data)
            {
                _joints[(int)(i.ID)] = new Joint();
                _joints[(int)(i.ID)].copy(i);
            }
        }

        public int Count { get { return _joints.Length; } }

        public Joint this[JointID i]
        {
            get
            {
                return _joints[(int)i];
            }
        }

        public IEnumerator GetEnumerator()
        {

            return new JointEnum(_joints);
        }

        public class JointEnum : IEnumerator
        {
            public Joint[] _joint;
            int position = -1;
            public JointEnum(Joint[] list)
            {
                _joint = list;
            }
            public bool MoveNext()
            {
                position++;
                return (position < _joint.Length);
            }
            public void Reset()
            {
                position = -1;
            }
            public object Current
            {
                get
                {
                    try
                    {
                        return _joint[position];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }
        }


    }

}

