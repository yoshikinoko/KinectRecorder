using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace VirtualKinect
{
    [Serializable]
    public class PlanarImage
    {
        public byte[] Bits;
        public int BytesPerPixel;
        public int Height;
        public int Width;
        public unsafe void copy(PlanarImage src)
        {
            com(this.Bits, src.Bits);

            this.BytesPerPixel = src.BytesPerPixel;
            this.Height = src.Height;
           this.Width = src.Width;
        
        }
        public unsafe void copy(Microsoft.Research.Kinect.Nui.PlanarImage data)
        {
            //this.Bits = new byte[data.Bits.Length];
            //fixed (byte* pbytes = Bits)
            //{
            //    for (int i = 0; i < data.Bits.Length; i++)
            //        pbytes[i] = data.Bits[i];

            //}
            com(this.Bits, data.Bits);

            this.BytesPerPixel = data.BytesPerPixel;
            this.Height = data.Height;
            this.Width = data.Width;
        }
        
        public Microsoft.Research.Kinect.Nui.PlanarImage NUI
        {
            get {
                Microsoft.Research.Kinect.Nui.PlanarImage r = new Microsoft.Research.Kinect.Nui.PlanarImage();
                com(r.Bits, this.Bits);
                r.BytesPerPixel = this.BytesPerPixel;
                r.Width = this.Width;
                r.Height = this.Height;
                return r;
            }
set{
       com(this.Bits, value.Bits);

            this.BytesPerPixel = value.BytesPerPixel;
            this.Height = value.Height;
            this.Width = value.Width;
     

}
        }


        public static unsafe void com(byte[] dst, byte[] src)
        {
            dst = new byte[src.Length];
            fixed (byte* pbytes = dst)
            {
                for (int i = 0; i < src.Length; i++)
                    pbytes[i] = src[i];
            }
        }
    }

}
