using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AM_Bot
{
    public static class Messages
    {
        private static Dictionary<string, string> _messages = null;

        public static void Load(string language)
        {
            Console.WriteLine("[System] Load Language...");

            DirectoryInfo dirInfo = new DirectoryInfo(string.Format("{0}/Lang", Directory.GetCurrentDirectory()));
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            string filePath = string.Format("{0}/Lang/{1}", Directory.GetCurrentDirectory(), language);
            if (!File.Exists(filePath))
            {
                _messages = new Dictionary<string, string>();
                _messages.Add("help",
                    @"도움말 (https://github.com/mkachi/AM-Bot/) \n" +
                    "!am open - 익명 메시지를 보낼 수 있는 개인 메시지 창을 띄웁니다.");

                _messages.Add("open",
                    @"도움말 (https://github.com/mkachi/AM-Bot/) \n" +
                    "이 곳에 치는 명령어 이외의 메시지들은 익명으로 지정된 채널에 전송됩니다.\n" +
                    "처음 사용시 !am set을 통해서 설정을 해주세요!\n" +
                    "!am code - 가입된 Guild의 channel 코드들을 보여줍니다.\n" +
                    "!am set [channel code] - 익명 메시지를 보낼 길드와 채널을 설정합니다.");

                _messages.Add("missingCommand", "없는 명령어 입니다.");

                _messages.Add("DM help", "도움말 (https://github.com/mkachi/AM-Bot/)\n" +
                    "이 곳에 치는 명령어 이외의 메시지들은 익명으로 지정된 채널에 전송됩니다.\n" +
                    "처음 사용시 !am set을 통해서 설정을 해주세요!\n" +
                    "!am code - 가입된 Guild의 channel 코드들을 보여줍니다.\n" +
                    "!am set [channel code] - 익명 메시지를 보낼 길드와 채널을 설정합니다.");

                _messages.Add("set", "설정되었습니다.");
                _messages.Add("setError", "코드를 다시 확인해주세요.");
                _messages.Add("setPlz", "!am set 명령어를 통해 채널을 설정해주세요");

                string data = JsonConvert.SerializeObject(_messages);
                JObject json = JObject.Parse(data);

                FileStream fileStream = new FileStream(filePath, FileMode.CreateNew);
                StreamWriter writer = new StreamWriter(fileStream);
                writer.Write(json.ToString());
                writer.Close();
                fileStream.Close();

                Console.WriteLine("[System] Create language file... (Default : Korean)");
            }
            else
            {
                FileStream fileStream = new FileStream(filePath, FileMode.Open);
                StreamReader reader = new StreamReader(fileStream);
                string jsonData = reader.ReadToEnd();
                reader.Close();
                fileStream.Close();

                _messages = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonData);
                Console.WriteLine("[System] Language load complite!");
            }
        }

        public static string Get(string key)
        {
            if (_messages.ContainsKey(key))
            {
                return _messages[key];
            }
            return "Error";
        }
    }
}
