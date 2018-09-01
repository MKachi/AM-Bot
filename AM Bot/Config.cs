using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace AM_Bot
{
    public static class Config
    {
        public static string Token { get; private set; }
        public static string LanguageFile { get; private set; }

        public static void Load()
        {
            Console.WriteLine("[System] Load Config...");
            string filePath = string.Format("{0}/{1}", Directory.GetCurrentDirectory(), "Config.json");
            if (!File.Exists(filePath))
            {
                JObject json = new JObject();
                json.Add("token", "Your bot token");
                json.Add("Language", "ko.json");
                FileStream fileStream = new FileStream(filePath, FileMode.CreateNew);
                StreamWriter writer = new StreamWriter(fileStream);
                writer.Write(json.ToString());
                writer.Close();
                fileStream.Close();

                throw new Exception("[Error] Please configure Config.json file.");
            }
            else
            {
                Console.WriteLine("[System] Load Config...");
                FileStream fileStream = new FileStream(filePath, FileMode.Open);
                StreamReader reader = new StreamReader(fileStream);
                string jsonData = reader.ReadToEnd();
                reader.Close();
                fileStream.Close();

                JObject json = JObject.Parse(jsonData);
                Token = json.Value<string>("token");
                LanguageFile = json.Value<string>("Language");
                Console.WriteLine("[System] Config load complite!");
                Messages.Load(LanguageFile);
            }
        }
    }
}
