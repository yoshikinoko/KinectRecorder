using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace VirtualKinect
{
    public class IO
    {
        public static void saveXML(KinectEventData ked, String fileName)
        {
            System.Xml.Serialization.XmlSerializer serializer =
                     new System.Xml.Serialization.XmlSerializer(typeof(KinectEventData));
            System.IO.FileStream fs = new System.IO.FileStream(
                fileName, System.IO.FileMode.Create);
            serializer.Serialize(fs, fileName);
            fs.Close();
        }
        public static KinectEventData loadXML(string fileName)
        {

            //XmlSerializerオブジェクトの作成
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(KinectEventData));
            //ファイルを開く
            System.IO.FileStream fs = new System.IO.FileStream(
                fileName, System.IO.FileMode.Open);
            //XMLファイルから読み込み、逆シリアル化する
            KinectEventData obj = (KinectEventData)serializer.Deserialize(fs);
            //閉じる
            fs.Close();

            return (KinectEventData)obj;
        }
        public static void save(KinectEventData ked, String fileName)
        {
            FileStream fs = new FileStream(fileName,
       FileMode.Create,
       FileAccess.Write);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, ked);
            fs.Close();
        }
        public static KinectEventData load(string fileName)
        {
            FileStream fs = new FileStream(fileName,
                FileMode.Open,
                FileAccess.Read);
            BinaryFormatter f = new BinaryFormatter();
   
            object obj = f.Deserialize(fs);
            fs.Close();

            return (KinectEventData)obj;
        }
    }
}
