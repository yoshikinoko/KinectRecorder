using System;
using System.Collections;
using System.Reflection;


namespace VirtualKinect
{
    [Serializable]
    public struct JointsCollection : IEnumerable
    {
        private Joint[] _joints;


        public Microsoft.Research.Kinect.Nui.JointsCollection NUI
        {
            set
            {

                _joints = new Joint[value.Count];

                foreach (Microsoft.Research.Kinect.Nui.Joint joint in value)
                {
                    _joints[(int)joint.ID] = new Joint();
                    _joints[(int)joint.ID].NUI = joint;
                }
            }
        }

        public int Count { get { return _joints.Length; } }

        public Joint this[Microsoft.Research.Kinect.Nui.JointID i]
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

