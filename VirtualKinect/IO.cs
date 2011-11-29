using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
namespace VirtualKinect
{
    public class IO
    {
        public static void saveXML(KinectEventData ked, String fileName)
        {
            saveXMLSerial((object)ked, fileName);
        }
        public static KinectEventData loadXML(string fileName)
        {

            String eventDataFolderRoot = System.IO.Path.GetDirectoryName(fileName);
            KinectEventData ked = (KinectEventData)loadXMLSerial(fileName);
            ked.loadEventData(eventDataFolderRoot);
            ked.loadRawEventData(eventDataFolderRoot);
            return ked;
        }

        public static void saveXMLSerial(object obj, string fileName)
        {
            System.Xml.Serialization.XmlSerializer serializer =
                     new System.Xml.Serialization.XmlSerializer((obj).GetType());
            System.IO.FileStream fs = new System.IO.FileStream(
                fileName, System.IO.FileMode.Create);
            serializer.Serialize(fs, obj);
            fs.Close();
        }


        public static void saveXMLSerialTask(object obj, string fileName)
        {
            Task t = Task.Factory.StartNew(() =>
            {
                saveXMLSerial(obj, fileName);
            });
        }


        public static object loadXMLSerial(string fileName)
        {

  

            return loadXMLSerialType(fileName, typeof(KinectEventData));
        }
        public static object loadXMLSerialType(string fileName, System.Type type)
        {

            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(type);
            System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open);
            object obj = serializer.Deserialize(fs);
            fs.Close();

            return obj;
        }


        //This method will be removed
        public static void save(KinectEventData ked, String fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, ked);
            fs.Close();
        }
        public static KinectEventData load(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryFormatter f = new BinaryFormatter();

            object obj = f.Deserialize(fs);
            fs.Close();
            return (KinectEventData)obj;
        }


    }
}
