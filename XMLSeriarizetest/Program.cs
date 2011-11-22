using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Research.Kinect.Nui;
namespace XMLSeriarizetest
{
    class Program

    {
        static int vcNum = 10;
        static List<Vector> makeVectors()
        {
            List<Vector> result = new List<Vector>(vcNum);
            for (int i = 0; i++ < vcNum; i++)
            {
            Vector   resultvc= new Vector();

            resultvc.W = 3.0f * (float)i;
            resultvc.X = 2.0f * (float)i;
            resultvc.Y = 1.3f * (float)i;
            resultvc.Z = 0.0f * (float)i;
            result.Add(resultvc);

            }
            return result;
        }
        static void saveVector(List<Vector> vcs)
        {


            String filename = @"test.xml";
            //XMLファイルに保存する
            System.Xml.Serialization.XmlSerializer serializer1 =
                new System.Xml.Serialization.XmlSerializer(
              vcs.GetType());
            System.IO.FileStream fs1 =
                new System.IO.FileStream(
           filename, System.IO.FileMode.Create);
            serializer1.Serialize(fs1, vcs);
            fs1.Close();

            ////保存した内容を復元する
            //System.Xml.Serialization.XmlSerializer serializer2 =
            //    new System.Xml.Serialization.XmlSerializer(
            //        typeof(Vector[]));
            //System.IO.FileStream fs2 =
            //    new System.IO.FileStream(
            //filename, System.IO.FileMode.Open);
            //Vector[] loadClasses;
            //loadClasses = (Vector[])serializer2.Deserialize(fs2);

            ////復元した内容を表示する
            //foreach (Vector loadClass in loadClasses)
            //{
            //    Console.WriteLine("W:"+loadClass.W +" X:"+loadClass.X +" Y:"+loadClass.Y +" Z:"+loadClass.Z);
            //}
            //fs2.Close();
        }
        static void Main(string[] args)
        {

            List<Vector> vcs = makeVectors();

            saveVector(vcs);


        }
    }
}
