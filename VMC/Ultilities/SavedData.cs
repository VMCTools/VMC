using Newtonsoft.Json;
using System;
using System.IO;

namespace VMC.Ultilities.Save
{
    public class SavedData
    {
        public static void LoadData<T>(ref T data, string pathFile)
        {
            try
            {
                if (File.Exists(pathFile))
                {
                    using (StreamReader reader = File.OpenText(pathFile))
                    {
                        data = JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public static void SaveData<T>(T mainData, string pathFile)
        {
            try
            {
                StreamWriter Writer = new StreamWriter(pathFile);
                Writer.Write(JsonConvert.SerializeObject(mainData));
                Writer.Flush();
                Writer.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}