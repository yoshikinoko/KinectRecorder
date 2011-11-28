using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Serialization;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Drawing; 
namespace VirtualKinect
{
    [Serializable]
    public class PlanarImage
    {

        public bool loadImage(String eventDataRootFolder)
        {
            if (rawFileName.Length == 0)
            {
                return false;
            }
            else
            {
                String openFileName = Path.Combine(eventDataRootFolder, rawFileName);

                if (useCompressedImage)
                {
                    loadCompressedImage(openFileName);
                }
                else
                {
                    loadRawImage(openFileName);
                }
                return true;
            }

        }

        public Task loadImageTask(String eventDataRootFolder)
        {
            Task t = Task.Factory.StartNew(() => { loadImage(eventDataRootFolder); });
            return t;
        }

        public Task saveImageTask(String saveRootFolder)
        {
            Task t = Task.Factory.StartNew(() => { saveImage(saveRootFolder); });
            return t;
        }

        public bool saveImage(String saveRootFolder)
        {
            if (rawFileName.Length == 0 || this.Bits == null)
            {
                return false;
            }
            else
            {
                String savePath = Path.Combine(saveRootFolder, rawFileName);

                if (useCompressedImage)
                {
                    saveCompressedImage(savePath);
                }
                else
                {
                    saveRawImageTask(savePath);
                }
                return true;
            }
        }

        private void saveCompressedImage(String savePath)
        {
            Task t = Task.Factory.StartNew(() =>
            {
                saveByImageDraw(savePath);
                // saveByBitmapSource(savePath);
            });
        }
        private void saveByImageDraw(String savePath)
        {
            BitmapSource bmp = BitmapSource.Create(Width, Height, 96, 96, System.Windows.Media.PixelFormats.Bgr32, null, this.Bits, Width * BytesPerPixel);
            FileStream stream = new FileStream(savePath, FileMode.Create);
            PngBitmapEncoder enc = new PngBitmapEncoder();
            enc.Interlace = PngInterlaceOption.Off;
            enc.Frames.Add(BitmapFrame.Create(bmp));
            enc.Save(stream);
            stream.Close(); 
        }

        //Do not Use this method. this method doesnt work!
        private void saveByBitmapSource(String savePath)
        {
            System.Windows.Media.Imaging.BitmapSource image = System.Windows.Media.Imaging.BitmapSource.Create(
        this.Width, this.Height, 96, 96, System.Windows.Media.PixelFormats.Bgra32, null, this.Bits, this.Width * this.BytesPerPixel);
            FileStream stream = new FileStream(savePath, FileMode.Create);
            System.Windows.Media.Imaging.PngBitmapEncoder encoder = new System.Windows.Media.Imaging.PngBitmapEncoder();
            encoder.Interlace = System.Windows.Media.Imaging.PngInterlaceOption.Off;
            encoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(image));
            encoder.Save(stream);
            stream.Close();
        }

        private void loadCompressedImage(String openFileName)
        {
            this.Bits = new byte[Width * Height * BytesPerPixel];
            Bitmap bmp = new Bitmap(openFileName);
            //DO NOT use System.Threading.Tasks. Parallel 
            //Because of the Bitmap Pixel access cannnot will not take multiple access
            for (int id = 0; id < Width * Height; id++)
            {
                int i = id % Width;
                int j = id / Width;

                int idx = (i + j * Width) * BytesPerPixel;
                Color color = bmp.GetPixel(i, j);
                Bits[idx] = color.B;
                Bits[idx + 1] = color.G;
                Bits[idx + 2] = color.R;
                Bits[idx + 3] = color.A;
            }
            bmp.Dispose();

        }

        private void saveRawImageTask(String savePath)
        {
            Task t = Task.Factory.StartNew(() =>
            {

                saveRawImage(savePath);
            });

        }

        private void saveRawImage(String savePath)
        {
            System.IO.FileStream fs = new System.IO.FileStream(savePath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            fs.Write(Bits, 0, Bits.Length);
            fs.Close();
        }

        private void loadRawImage(String openFileName)
        {
            System.IO.FileStream fs = new System.IO.FileStream(openFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            Bits = new byte[fs.Length];
            fs.Read(Bits, 0, Bits.Length);
            fs.Close();
        }




        [XmlAttribute]
        public int BytesPerPixel;
        [XmlAttribute]
        public int Height;
        [XmlAttribute]
        public int Width;
        [XmlAttribute]
        public string rawFileName;
        [XmlAttribute]
        public string previewFileName;
        [XmlAttribute]
        public bool useCompressedImage;

        [XmlIgnoreAttribute]
        public byte[] Bits;
        [XmlIgnoreAttribute]
        public Microsoft.Research.Kinect.Nui.PlanarImage NUI
        {
            get
            {
                Microsoft.Research.Kinect.Nui.PlanarImage r = new Microsoft.Research.Kinect.Nui.PlanarImage();
                com(r.Bits, this.Bits);
                r.BytesPerPixel = this.BytesPerPixel;
                r.Width = this.Width;
                r.Height = this.Height;
                return r;
            }
            set
            {
                // com(this.Bits, value.Bits);
                this.Bits = new byte[value.Bits.Length];
                Array.Copy(value.Bits, this.Bits, value.Bits.Length);
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
