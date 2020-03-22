using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using System.Runtime.Serialization.Formatters.Binary;


namespace Csh_lib
{
    [Serializable]
    public class TestTime
    {
        public List<string> Info { get; set;}

        public TestTime()
        {
            Info = new List<string>();
        }

        public void Add(string record1, string record2, string record3, string record4)
        {
            Info.Add("\n" + "Порядок блочной матрицы : " + record1 + "\nВремя выполнения С# : " + record2 +" мс" + "\nВремя выполнения С++ : " + record3 + " мс" + "\nОтношение : " + record4 + "\n");
        }

        public static bool Save (string filename, ref TestTime obj)
        {
            FileStream fileStream = null;

            try
            {
                fileStream = File.Create(filename);
                BinaryFormatter binaryFormatter = new BinaryFormatter();       
                binaryFormatter.Serialize(fileStream, obj);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Исключение : " + ex.Message);
                return false;
            }
            finally
            {
                if (fileStream != null) fileStream.Close();
            }
            return true;
        }

        public static bool Load (string filename, ref TestTime obj)
        {
            FileStream fileStream = null;

            try
            {
                fileStream = File.OpenRead(filename);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                obj = binaryFormatter.Deserialize(fileStream) as TestTime;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Исключение: " + ex.Message + " (Будет создан после вызова метода Save)");
                return false;
            }
            finally
            {
                if (fileStream != null) fileStream.Close();
            }
            return true;
        }

        public override string ToString()
        {
            return string.Join("\n", Info);
        }
    }
}
